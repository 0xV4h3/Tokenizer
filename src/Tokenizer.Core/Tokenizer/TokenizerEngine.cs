using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Tokenizer.Core.Models;

namespace Tokenizer.Core.Tokenizer
{
    /// <summary>
    /// SQL TokenizerEngine.
    /// </summary>
    public class TokenizerEngine : ITokenizer
    {
        private readonly KeywordRegistry _keywords;
        private readonly OperatorRegistry _operators;
        private readonly PunctuatorRegistry _punctuators;
        private readonly FunctionRegistry _functions;

        private string? _input;
        private int _pos, _line, _col;

        public TokenizerEngine(
            KeywordRegistry? keywords = null,
            OperatorRegistry? operators = null,
            PunctuatorRegistry? punctuators = null,
            FunctionRegistry? functions = null)
        {
            _keywords = keywords ?? new KeywordRegistry();
            _operators = operators ?? new OperatorRegistry();
            _punctuators = punctuators ?? new PunctuatorRegistry();
            _functions = functions ?? new FunctionRegistry();
        }

        public IEnumerable<Token> Tokenize(string input)
        {
            _input = input ?? "";
            _pos = 0;
            _line = 1;
            _col = 1;

            while (_pos < _input.Length)
            {
                SkipWhitespace();
                if (_pos >= _input.Length) break;

                if (TryParseComment(out var comment)) { yield return comment; continue; }
                if (TryParseDollarQuotedString(out var dollar)) { yield return dollar; continue; }
                if (TryParseEscapeStringLiteral(out var escStr)) { yield return escStr; continue; }
                if (TryParseBinaryLiteral(out var binLit)) { yield return binLit; continue; }
                if (TryParseHexLiteral(out var hexLit)) { yield return hexLit; continue; }
                if (TryParseCStyleHex(out var cHexLit)) { yield return cHexLit; continue; }
                if (TryParseCStyleBinary(out var cBinLit)) { yield return cBinLit; continue; }
                if (TryParseArrayLiteral(out var arrLit)) { yield return arrLit; continue; }
                if (TryParseJsonLiteral(out var jsonLit)) { yield return jsonLit; continue; }
                if (TryParseXmlLiteral(out var xmlLit)) { yield return xmlLit; continue; }
                if (TryParseUuidLiteral(out var uuidLit)) { yield return uuidLit; continue; }
                if (TryParseDelimitedIdentifier(out var delimId)) { yield return delimId; continue; }
                if (TryParseStringOrDateTimeLiteral(out var strOrDateTimeLit)) { yield return strOrDateTimeLit; continue; }
                if (TryParseCharLiteral(out var charLit)) { yield return charLit; continue; }
                if (TryParseNumericLiteral(out var numLit)) { yield return numLit; continue; }
                if (TryParseWordToken(out var wordTok)) { yield return wordTok; continue; }
                if (TryParseMultiCharOperator(out var mOp)) { yield return mOp; continue; }
                if (TryParseSingleCharOperator(out var sOp)) { yield return sOp; continue; }
                if (TryParsePunctuator(out var punct)) { yield return punct; continue; }
                yield return ReadUnknownToken();
            }

            yield return new Token(TokenType.END_OF_FILE, string.Empty, _line, _col);
        }

        public async IAsyncEnumerable<Token> TokenizeAsync(string input, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
        {
            foreach (var t in Tokenize(input))
            {
                ct.ThrowIfCancellationRequested();
                await Task.Yield();
                yield return t;
            }
        }

        private void SkipWhitespace()
        {
            while (_pos < _input!.Length && char.IsWhiteSpace(_input[_pos]))
            {
                if (_input[_pos] == '\n') { _line++; _col = 1; }
                else _col++;
                _pos++;
            }
        }

        private char Peek(int ahead = 0) => (_pos + ahead < _input!.Length) ? _input[_pos + ahead] : '\0';
        private void Advance(int count = 1)
        {
            for (int i = 0; i < count && _pos < _input!.Length; i++)
            {
                if (_input[_pos] == '\n') { _line++; _col = 1; }
                else _col++;
                _pos++;
            }
        }

        private bool TryParseComment(out Token token)
        {
            token = null!;
            if (_input == null || _pos + 1 >= _input.Length) return false;
            if (_input[_pos] == '-' && _input[_pos + 1] == '-')
            {
                int start = _pos, line = _line, col = _col;
                _pos += 2; _col += 2;
                while (_pos < _input.Length && _input[_pos] != '\n') { _pos++; _col++; }
                token = new Token(TokenType.COMMENT, _input.Substring(start, _pos - start), line, col) { CommentType = CommentType.SINGLE_LINE };
                return true;
            }
            if (_input[_pos] == '/' && _input[_pos + 1] == '*')
            {
                int start = _pos, line = _line, col = _col;
                _pos += 2; _col += 2;
                while (_pos + 1 < _input.Length && !(_input[_pos] == '*' && _input[_pos + 1] == '/'))
                {
                    if (_input[_pos] == '\n') { _line++; _col = 1; }
                    else _col++;
                    _pos++;
                }
                if (_pos + 1 < _input.Length) { _pos += 2; _col += 2; }
                token = new Token(TokenType.COMMENT, _input.Substring(start, _pos - start), line, col) { CommentType = CommentType.MULTI_LINE };
                return true;
            }
            return false;
        }

        private bool TryParseDollarQuotedString(out Token token)
        {
            token = null!;
            if (_input == null || _input[_pos] != '$') return false;
            int tagEnd = _input.IndexOf('$', _pos + 1);
            if (tagEnd == -1)
            {
                int start = _pos, line = _line, col = _col;
                while (_pos < _input.Length && !char.IsWhiteSpace(_input[_pos])) _pos++;
                string val = _input.Substring(start, _pos - start);
                token = new Token(TokenType.UNKNOWN, val, line, col);
                return true;
            }
            string tag = _input.Substring(_pos, tagEnd - _pos + 1);
            int contentStart = tagEnd + 1;
            int closePos = _input.IndexOf(tag, contentStart, StringComparison.Ordinal);
            if (closePos == -1)
            {
                int start = _pos, line = _line, col = _col;
                while (_pos < _input.Length && !char.IsWhiteSpace(_input[_pos])) _pos++;
                string val = _input.Substring(start, _pos - start);
                token = new Token(TokenType.UNKNOWN, val, line, col);
                return true;
            }
            int line2 = _line, col2 = _col;
            string value = _input.Substring(_pos, closePos + tag.Length - _pos);
            int adv = closePos + tag.Length - _pos;
            Advance(adv);
            token = new Token(TokenType.LITERAL, value, line2, col2) { LiteralCategory = LiteralCategory.STRING };
            return true;
        }

        private bool TryParseEscapeStringLiteral(out Token token)
        {
            token = null!;
            if (_input == null || !((_input[_pos] == 'E' || _input[_pos] == 'e') && _pos + 1 < _input.Length && _input[_pos + 1] == '\'')) return false;
            int start = _pos, line = _line, col = _col;
            _pos += 2; _col += 2;
            StringBuilder sb = new();
            while (_pos < _input.Length)
            {
                if (_input[_pos] == '\'' && (_pos + 1 >= _input.Length || _input[_pos + 1] != '\'')) { _pos++; _col++; break; }
                if (_input[_pos] == '\\' && _pos + 1 < _input.Length)
                {
                    char next = _input[_pos + 1];
                    if (next == 'n') { sb.Append('\n'); _pos += 2; _col += 2; continue; }
                    if (next == 't') { sb.Append('\t'); _pos += 2; _col += 2; continue; }
                    if (next == '\'') { sb.Append('\''); _pos += 2; _col += 2; continue; }
                }
                sb.Append(_input[_pos]); if (_input[_pos] == '\n') { _line++; _col = 1; } else _col++; _pos++;
            }
            token = new Token(TokenType.LITERAL, sb.ToString(), line, col) { LiteralCategory = LiteralCategory.ESCAPE_STRING };
            return true;
        }

        private bool TryParseBinaryLiteral(out Token token)
        {
            token = null!;
            if (_input == null || !((_input[_pos] == 'B' || _input[_pos] == 'b') && _pos + 1 < _input.Length && _input[_pos + 1] == '\'')) return false;
            int start = _pos, line = _line, col = _col;
            _pos += 2; _col += 2;
            StringBuilder sb = new();
            while (_pos < _input.Length)
            {
                if (_input[_pos] == '\'' && (_pos + 1 >= _input.Length || _input[_pos + 1] != '\'')) { _pos++; _col++; break; }
                if (_input[_pos] != '0' && _input[_pos] != '1') { _pos++; _col++; continue; }
                sb.Append(_input[_pos]); if (_input[_pos] == '\n') { _line++; _col = 1; } else _col++; _pos++;
            }
            token = new Token(TokenType.LITERAL, sb.ToString(), line, col) { LiteralCategory = LiteralCategory.BINARY };
            return true;
        }

        private bool TryParseHexLiteral(out Token token)
        {
            token = null!;
            if (_input == null || !((_input[_pos] == 'X' || _input[_pos] == 'x') && _pos + 1 < _input.Length && _input[_pos + 1] == '\'')) return false;
            int start = _pos, line = _line, col = _col;
            _pos += 2; _col += 2;
            StringBuilder sb = new();
            while (_pos < _input.Length)
            {
                if (_input[_pos] == '\'' && (_pos + 1 >= _input.Length || _input[_pos + 1] != '\'')) { _pos++; _col++; break; }
                if (!Uri.IsHexDigit(_input[_pos])) { _pos++; _col++; continue; }
                sb.Append(_input[_pos]); if (_input[_pos] == '\n') { _line++; _col = 1; } else _col++; _pos++;
            }
            token = new Token(TokenType.LITERAL, sb.ToString(), line, col) { LiteralCategory = LiteralCategory.HEX };
            return true;
        }

        private bool TryParseCStyleHex(out Token token)
        {
            token = null!;
            if (_input == null || !(_input[_pos] == '0' && _pos + 1 < _input.Length && (_input[_pos + 1] == 'x' || _input[_pos + 1] == 'X'))) return false;
            int start = _pos, line = _line, col = _col;
            _pos += 2; _col += 2;
            StringBuilder sb = new();
            while (_pos < _input.Length && Uri.IsHexDigit(_input[_pos])) { sb.Append(_input[_pos]); _pos++; _col++; }
            token = new Token(TokenType.LITERAL, sb.ToString(), line, col) { LiteralCategory = LiteralCategory.HEX };
            return sb.Length > 0;
        }

        private bool TryParseCStyleBinary(out Token token)
        {
            token = null!;
            if (_input == null || !(_input[_pos] == '0' && _pos + 1 < _input.Length && (_input[_pos + 1] == 'b' || _input[_pos + 1] == 'B'))) return false;
            int start = _pos, line = _line, col = _col;
            _pos += 2; _col += 2;
            StringBuilder sb = new();
            while (_pos < _input.Length && (_input[_pos] == '0' || _input[_pos] == '1')) { sb.Append(_input[_pos]); _pos++; _col++; }
            token = new Token(TokenType.LITERAL, sb.ToString(), line, col) { LiteralCategory = LiteralCategory.BINARY };
            return sb.Length > 0;
        }

        private bool TryParseArrayLiteral(out Token token)
        {
            token = null!;
            if (_input == null) return false;
            int arrStart = _pos;
            if ((_input[_pos] == 'A' || _input[_pos] == 'a') && _input.Length - _pos >= 6 && _input.Substring(_pos, 5).ToUpper() == "ARRAY" && _input[_pos + 5] == '[')
            {
                int open = _pos + 5;
                int close = open + 1, depth = 1;
                while (close < _input.Length && depth > 0)
                {
                    if (_input[close] == '[') depth++;
                    if (_input[close] == ']') depth--;
                    close++;
                }
                string content = _input.Substring(arrStart, close - arrStart);
                token = new Token(TokenType.LITERAL, content, _line, _col) { LiteralCategory = LiteralCategory.ARRAY };
                Advance(close - arrStart);
                return true;
            }
            return false;
        }

        private bool TryParseJsonLiteral(out Token token)
        {
            token = null!;
            if (_input == null || _input[_pos] != '{') return false;
            int start = _pos, depth = 1, i = _pos + 1;
            while (i < _input.Length && depth > 0)
            {
                if (_input[i] == '{') depth++;
                else if (_input[i] == '}') depth--;
                i++;
            }
            if (depth != 0) return false;
            string content = _input.Substring(start, i - start);
            token = new Token(TokenType.LITERAL, content, _line, _col) { LiteralCategory = LiteralCategory.JSON };
            Advance(i - start);
            return true;
        }

        private bool TryParseXmlLiteral(out Token token)
        {
            token = null!;
            if (_input == null || _input[_pos] != '<') return false;
            int start = _pos, i = _pos + 1;
            while (i < _input.Length && _input[i] != '>') i++;
            if (i >= _input.Length) return false;
            string content = _input.Substring(start, i - start + 1);
            token = new Token(TokenType.LITERAL, content, _line, _col) { LiteralCategory = LiteralCategory.XML };
            Advance(i - start + 1);
            return true;
        }

        private bool TryParseUuidLiteral(out Token token)
        {
            token = null!;
            if (_input == null || _pos + 36 > _input.Length) return false;
            string s = _input.Substring(_pos, 36);
            if (!Regex.IsMatch(s, @"^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$")) return false;
            token = new Token(TokenType.LITERAL, s, _line, _col) { LiteralCategory = LiteralCategory.UUID };
            Advance(36);
            return true;
        }

        private bool TryParseDelimitedIdentifier(out Token token)
        {
            token = null!;
            if (_input == null || !(_input[_pos] == '"' || _input[_pos] == '[' || _input[_pos] == '`')) return false;
            char open = _input[_pos], close = open == '[' ? ']' : open;
            int start = _pos; _pos++; _col++;
            StringBuilder sb = new();
            while (_pos < _input.Length && _input[_pos] != close) { sb.Append(_input[_pos]); _pos++; _col++; }
            _pos++; _col++;
            string name = sb.ToString();
            token = new Token(TokenType.IDENTIFIER, name, _line, _col);
            return true;
        }

        private bool TryParseStringOrDateTimeLiteral(out Token token)
        {
            token = null!;
            if (_input == null || _input[_pos] != '\'') return false;
            int start = _pos, line = _line, col = _col; _pos++; _col++;
            StringBuilder sb = new();
            while (_pos < _input.Length && _input[_pos] != '\'')
            {
                if (_input[_pos] == '\\' && _pos + 1 < _input.Length) { sb.Append(_input[_pos + 1]); _pos += 2; _col += 2; continue; }
                sb.Append(_input[_pos]); if (_input[_pos] == '\n') { _line++; _col = 1; } else _col++; _pos++;
            }
            _pos++; _col++;
            string value = sb.ToString();
            if (DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                token = new Token(TokenType.LITERAL, value, line, col) { LiteralCategory = LiteralCategory.DATE };
                return true;
            }
            if (DateTime.TryParseExact(value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                token = new Token(TokenType.LITERAL, value, line, col) { LiteralCategory = LiteralCategory.DATETIME };
                return true;
            }
            if (TimeSpan.TryParseExact(value, "hh\\:mm\\:ss", CultureInfo.InvariantCulture, out _))
            {
                token = new Token(TokenType.LITERAL, value, line, col) { LiteralCategory = LiteralCategory.TIME };
                return true;
            }
            token = new Token(TokenType.LITERAL, value, line, col) { LiteralCategory = LiteralCategory.STRING };
            return true;
        }

        private bool TryParseCharLiteral(out Token token)
        {
            token = null!;
            if (_input == null || !((_input[_pos] == 'C' || _input[_pos] == 'c') && _pos + 1 < _input.Length && _input[_pos + 1] == '\'')) return false;
            int start = _pos, line = _line, col = _col;
            _pos += 2; _col += 2;
            char value = _input[_pos];
            _pos++;
            if (_input[_pos] != '\'') return false;
            _pos++; _col++;
            token = new Token(TokenType.LITERAL, value.ToString(), line, col) { LiteralCategory = LiteralCategory.CHAR };
            return true;
        }

        private bool TryParseNumericLiteral(out Token token)
        {
            token = null!;
            int start = _pos, line = _line, col = _col;
            bool isFloat = false;
            if (_input == null) return false;
            if (_input[_pos] == '-') _pos++;
            while (_pos < _input.Length && char.IsDigit(_input[_pos])) _pos++;
            if (_pos < _input.Length && _input[_pos] == '.')
            {
                isFloat = true; _pos++;
                while (_pos < _input.Length && char.IsDigit(_input[_pos])) _pos++;
            }
            if (_pos < _input.Length && (_input[_pos] == 'e' || _input[_pos] == 'E'))
            {
                isFloat = true; _pos++;
                if (_pos < _input.Length && (_input[_pos] == '+' || _input[_pos] == '-')) _pos++;
                while (_pos < _input.Length && char.IsDigit(_input[_pos])) _pos++;
            }
            if (_pos == start) return false;
            string content = _input.Substring(start, _pos - start);
            token = new Token(TokenType.LITERAL, content, line, col)
            {
                LiteralCategory = isFloat ? LiteralCategory.FLOAT : LiteralCategory.INTEGER
            };
            return true;
        }

        private bool TryParseWordToken(out Token token)
        {
            token = null!;
            if (_input == null) return false;
            int start = _pos, line = _line, col = _col;

            if (_input[_pos] == '@')
            {
                int varStart = _pos;
                int atCount = 1;
                _pos++;
                if (_pos < _input.Length && _input[_pos] == '@') { atCount++; _pos++; }
                int nameStart = _pos;
                while (_pos < _input.Length && (char.IsLetterOrDigit(_input[_pos]) || _input[_pos] == '_')) _pos++;
                if (_pos > nameStart)
                {
                    string candidate = _input.Substring(varStart, _pos - varStart);
                    token = new Token(TokenType.IDENTIFIER, candidate, line, col);
                    return true;
                }
                _pos = start;
            }

            for (int len = 4; len >= 1; len--)
            {
                int end = _pos;
                int words = 0;
                while (words < len)
                {
                    int wordStart = end;
                    while (end < _input.Length && (char.IsLetter(_input[end]) || _input[end] == '_')) end++;
                    if (end == wordStart) break;
                    words++;
                    while (end < _input.Length && char.IsWhiteSpace(_input[end])) end++;
                }
                if (words == len)
                {
                    string candidate = _input.Substring(start, end - start);
                    if (_operators.TryGet(candidate.Trim(), out var opInfo))
                    {
                        token = new Token(TokenType.OPERATOR, candidate.Trim(), line, col) { OperatorCategory = opInfo.Category, OperatorInfo = opInfo };
                        _pos = end;
                        return true;
                    }
                }
            }

            if (!(char.IsLetter(_input[_pos]) || _input[_pos] == '_')) return false;
            while (_pos < _input.Length && (char.IsLetterOrDigit(_input[_pos]) || _input[_pos] == '_')) _pos++;
            string lexeme = _input.Substring(start, _pos - start);

            if (string.Equals(lexeme, "TRUE", StringComparison.OrdinalIgnoreCase))
            {
                token = new Token(TokenType.LITERAL, lexeme, line, col) { LiteralCategory = LiteralCategory.BOOLEAN };
                return true;
            }
            if (string.Equals(lexeme, "FALSE", StringComparison.OrdinalIgnoreCase))
            {
                token = new Token(TokenType.LITERAL, lexeme, line, col) { LiteralCategory = LiteralCategory.BOOLEAN };
                return true;
            }
            if (string.Equals(lexeme, "NULL", StringComparison.OrdinalIgnoreCase))
            {
                token = new Token(TokenType.LITERAL, lexeme, line, col) { LiteralCategory = LiteralCategory.NULL_VALUE };
                return true;
            }
            if (string.Equals(lexeme, "INTERVAL", StringComparison.OrdinalIgnoreCase))
            {
                SkipWhitespace();
                if (_input != null && _pos < _input.Length && _input[_pos] == '\'')
                {
                    int tStart = _pos + 1;
                    int tEnd = _input.IndexOf('\'', tStart);
                    if (tEnd > tStart)
                    {
                        string value = _input.Substring(tStart, tEnd - tStart);
                        _pos = tEnd + 1; _col += (tEnd + 1 - tStart);
                        SkipWhitespace();
                        int unitStart = _pos;
                        while (_pos < _input.Length && char.IsLetter(_input[_pos])) _pos++;
                        string unit = _input.Substring(unitStart, _pos - unitStart);
                        token = new Token(TokenType.LITERAL, $"INTERVAL '{value}' {unit}", line, col) { LiteralCategory = LiteralCategory.INTERVAL };
                        return true;
                    }
                }
                token = new Token(TokenType.KEYWORD, lexeme, line, col) { KeywordCategory = KeywordCategory.MISC };
                return true;
            }
            if (Enum.TryParse<DateTimePart>(lexeme, true, out var dtp) && dtp != DateTimePart.UNKNOWN)
            {
                token = new Token(TokenType.DATETIMEPART, lexeme, line, col);
                return true;
            }
            if (_keywords.TryGet(lexeme, out var kinfo))
            {
                token = new Token(TokenType.KEYWORD, lexeme, line, col)
                {
                    KeywordCategory = kinfo.Category,
                    KeywordInfo = kinfo
                };
                return true;
            }
            int tmp = _pos;
            while (tmp < _input.Length && char.IsWhiteSpace(_input[tmp])) tmp++;
            if (tmp < _input.Length && _input[tmp] == '(' && _functions.TryGet(lexeme, out var finfo))
            {
                token = new Token(TokenType.FUNCTION, lexeme, line, col)
                {
                    FunctionCategory = finfo.Category,
                    FunctionInfo = finfo
                };
                return true;
            }
            token = new Token(TokenType.IDENTIFIER, lexeme, line, col);
            return true;
        }

        private bool TryParseMultiCharOperator(out Token token)
        {
            token = null!;
            if (_input == null) return false;
            for (int len = 4; len >= 2; len--)
            {
                if (_pos + len <= _input.Length)
                {
                    string op = _input.Substring(_pos, len);
                    if (_operators.TryGet(op, out var info))
                    {
                        token = new Token(TokenType.OPERATOR, op, _line, _col) { OperatorCategory = info.Category, OperatorInfo = info };
                        Advance(len);
                        return true;
                    }
                }
            }
            return false;
        }

        private bool TryParseSingleCharOperator(out Token token)
        {
            token = null!;
            if (_input == null) return false;
            string op = _input[_pos].ToString();
            if (_operators.TryGet(op, out var info))
            {
                token = new Token(TokenType.OPERATOR, op, _line, _col) { OperatorCategory = info.Category, OperatorInfo = info };
                Advance();
                return true;
            }
            return false;
        }

        private bool TryParsePunctuator(out Token token)
        {
            token = null!;
            if (_input != null && _punctuators.TryGet(Peek(), out var info))
            {
                int line = _line, col = _col;
                Advance();
                token = new Token(TokenType.PUNCTUATOR, info.Lexeme, line, col) { PunctuatorInfo = info };
                return true;
            }
            return false;
        }

        private Token ReadUnknownToken()
        {
            char c = Peek();
            int line = _line, col = _col;
            Advance();
            return new Token(TokenType.UNKNOWN, c.ToString(), line, col);
        }
    }
}
using System;
using System.Collections.Generic;
using Tokenizer.Core.Models;

namespace Tokenizer.Core.Tokenizer
{
    /// <summary>
    /// PunctuatorRegistry maps single character punctuators to metadata.
    /// </summary>
    public class PunctuatorRegistry
    {
        private readonly Dictionary<char, PunctuatorInfo> _map = new();

        public PunctuatorRegistry()
        {
            _map[','] = new PunctuatorInfo(",", CommonSymbol.COMMA);
            _map[';'] = new PunctuatorInfo(";", CommonSymbol.SEMICOLON);
            _map['('] = new PunctuatorInfo("(", CommonSymbol.LPAREN);
            _map[')'] = new PunctuatorInfo(")", CommonSymbol.RPAREN);
            _map['{'] = new PunctuatorInfo("{", CommonSymbol.LBRACE);
            _map['}'] = new PunctuatorInfo("}", CommonSymbol.RBRACE);
            _map['['] = new PunctuatorInfo("[", CommonSymbol.LBRACKET);
            _map[']'] = new PunctuatorInfo("]", CommonSymbol.RBRACKET);
            _map['.'] = new PunctuatorInfo(".", CommonSymbol.DOT);
            _map[':'] = new PunctuatorInfo(":", CommonSymbol.COLON);
            _map['?'] = new PunctuatorInfo("?", CommonSymbol.PARAM_MARKER);
            _map['$'] = new PunctuatorInfo("$", CommonSymbol.PARAM_MARKER);
            _map['@'] = new PunctuatorInfo("@", CommonSymbol.PARAM_MARKER);
            _map['`'] = new PunctuatorInfo("`", CommonSymbol.DOT);
            _map['"'] = new PunctuatorInfo("\"", CommonSymbol.DOT);
        }

        public bool TryGet(char ch, out PunctuatorInfo info) => _map.TryGetValue(ch, out info);

        public void RegisterCustom(PunctuatorInfo info)
        {
            if (info is null) throw new ArgumentNullException(nameof(info));
            if (!string.IsNullOrEmpty(info.Lexeme) && info.Lexeme.Length == 1)
            {
                _map[info.Lexeme[0]] = info;
            }
        }

        public IReadOnlyDictionary<char, PunctuatorInfo> AllPunctuators() => new Dictionary<char, PunctuatorInfo>(_map);
    }
}
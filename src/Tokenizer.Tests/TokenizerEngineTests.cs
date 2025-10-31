using System.Linq;
using Tokenizer.Core.Tokenizer;
using Tokenizer.Core.Models;
using Xunit;

namespace Tokenizer.Tests
{
    public class TokenizerEngineTests
    {
        [Fact]
        public void Tokenize_BasicKeywords_Recognized()
        {
            var engine = new TokenizerEngine();
            var tokens = engine.Tokenize("SELECT FROM WHERE").ToList();
            Assert.Equal(TokenType.KEYWORD, tokens[0].Type);
            Assert.Equal(TokenType.KEYWORD, tokens[1].Type);
            Assert.Equal(TokenType.KEYWORD, tokens[2].Type);
        }

        [Fact]
        public void Tokenize_AllLiteralTypes()
        {
            var engine = new TokenizerEngine();
            var sql = "SELECT 'str', 123, 4.56, TRUE, FALSE, NULL, X'1A2B', B'1010', 0xDEAD, 0b101, \"quoted\", [bracketed], N'str', E'str\\n';";
            var tokens = engine.Tokenize(sql).ToList();
            Assert.Contains(tokens, t => t.LiteralCategory == LiteralCategory.STRING);
            Assert.Contains(tokens, t => t.LiteralCategory == LiteralCategory.INTEGER);
            Assert.Contains(tokens, t => t.LiteralCategory == LiteralCategory.FLOAT);
            Assert.Contains(tokens, t => t.LiteralCategory == LiteralCategory.BOOLEAN);
            Assert.Contains(tokens, t => t.LiteralCategory == LiteralCategory.NULL_VALUE);
            Assert.Contains(tokens, t => t.LiteralCategory == LiteralCategory.HEX);
            Assert.Contains(tokens, t => t.LiteralCategory == LiteralCategory.BINARY);
        }

        [Fact]
        public void Tokenize_OperatorsAndPunctuators()
        {
            var engine = new TokenizerEngine();
            var sql = "a = b + 2; c != d and e || f::int";
            var tokens = engine.Tokenize(sql).ToList();
            Assert.Contains(tokens, t => t.Type == TokenType.OPERATOR && t.Lexeme == "=");
            Assert.Contains(tokens, t => t.Type == TokenType.OPERATOR && t.Lexeme == "+");
            Assert.Contains(tokens, t => t.Type == TokenType.OPERATOR && t.Lexeme == "!=");
            Assert.Contains(tokens, t => t.Type == TokenType.PUNCTUATOR && t.Lexeme == ";");
            Assert.Contains(tokens, t => t.Type == TokenType.OPERATOR && t.Lexeme == "||");
            Assert.Contains(tokens, t => t.Type == TokenType.OPERATOR && t.Lexeme == "::");
        }

        [Fact]
        public void Tokenize_Comments()
        {
            var engine = new TokenizerEngine();
            var sql = "SELECT 1 -- single line\n/* multi\nline */";
            var tokens = engine.Tokenize(sql).ToList();
            Assert.Contains(tokens, t => t.Type == TokenType.COMMENT && t.Lexeme.Contains("--"));
            Assert.Contains(tokens, t => t.Type == TokenType.COMMENT && t.Lexeme.Contains("multi"));
        }

        [Fact]
        public void Tokenize_Functions_AllCategories()
        {
            var engine = new TokenizerEngine();
            var sql = "SUM(a), LOWER(b), NOW(), RANDOM(), ROW_NUMBER() OVER()";
            var tokens = engine.Tokenize(sql).ToList();
            Assert.Contains(tokens, t => t.Type == TokenType.FUNCTION && t.Lexeme.ToUpper() == "SUM");
            Assert.Contains(tokens, t => t.Type == TokenType.FUNCTION && t.Lexeme.ToUpper() == "LOWER");
            Assert.Contains(tokens, t => t.Type == TokenType.FUNCTION && t.Lexeme.ToUpper() == "NOW");
            Assert.Contains(tokens, t => t.Type == TokenType.FUNCTION && t.Lexeme.ToUpper() == "RANDOM");
            Assert.Contains(tokens, t => t.Type == TokenType.FUNCTION && t.Lexeme.ToUpper() == "ROW_NUMBER");
        }

        [Fact]
        public void Tokenize_Identifiers_SpecialCases()
        {
            var engine = new TokenizerEngine();
            var sql = "mytable \"MyCol\" [OtherCol] _var1 @user @@sys";
            var tokens = engine.Tokenize(sql).ToList();
            Assert.Contains(tokens, t => t.Type == TokenType.IDENTIFIER && t.Lexeme == "mytable");
            Assert.Contains(tokens, t => t.Type == TokenType.IDENTIFIER && t.Lexeme == "MyCol");
            Assert.Contains(tokens, t => t.Type == TokenType.IDENTIFIER && t.Lexeme == "OtherCol");
            Assert.Contains(tokens, t => t.Type == TokenType.IDENTIFIER && t.Lexeme == "_var1");
            Assert.Contains(tokens, t => t.Type == TokenType.IDENTIFIER && t.Lexeme == "@user");
            Assert.Contains(tokens, t => t.Type == TokenType.IDENTIFIER && t.Lexeme == "@@sys");
        }

        [Fact]
        public void Tokenize_UnknownTokens()
        {
            var engine = new TokenizerEngine();
            var tokens = engine.Tokenize("SELECT $illegal$ FROM x").ToList();
            Assert.Contains(tokens, t => t.Type == TokenType.UNKNOWN);
        }

        [Fact]
        public void Tokenize_DateTimeParts_And_Interval()
        {
            var engine = new TokenizerEngine();
            var sql = "EXTRACT(YEAR FROM t), INTERVAL '1 day', DATE '2024-01-01', TIMESTAMP '2024-01-01 10:00:00'";
            var tokens = engine.Tokenize(sql).ToList();
            Assert.Contains(tokens, t => t.Type == TokenType.DATETIMEPART && t.Lexeme.ToUpper() == "YEAR");
            Assert.Contains(tokens, t => t.LiteralCategory == LiteralCategory.INTERVAL);
            Assert.Contains(tokens, t => t.LiteralCategory == LiteralCategory.DATE);
            Assert.Contains(tokens, t => t.LiteralCategory == LiteralCategory.DATETIME);
        }
    }
}
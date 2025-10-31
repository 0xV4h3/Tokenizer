using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Tokenizer.Core.Models;

namespace Tokenizer.Core.Utils
{
    /// <summary>
    /// JSON/CSV serializer for Token collection. Serializes enum values as strings.
    /// </summary>
    public static class TokenSerializer
    {
        public static string ToJson(IEnumerable<Token> tokens)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            options.Converters.Add(new JsonStringEnumConverter());
            return JsonSerializer.Serialize(tokens, options);
        }

        public static IEnumerable<Token> FromJson(string json)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new JsonStringEnumConverter());
            return JsonSerializer.Deserialize<List<Token>>(json, options) ?? new List<Token>();
        }

        public static string ToCsv(IEnumerable<Token> tokens)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Type,Lexeme,Category,Line,Column,Info");

            foreach (var t in tokens)
            {
                string type = t.Type.ToString();
                string lexeme = EscapeCsv(t.Lexeme);
                string category = GetCategoryName(t);
                string info = GetInfo(t);
                string line = t.Line.ToString();
                string col = t.Column.ToString();

                sb.AppendLine($"\"{type}\",\"{lexeme}\",\"{EscapeCsv(category)}\",\"{line}\",\"{col}\",\"{EscapeCsv(info)}\"");
            }

            return sb.ToString();
        }

        private static string EscapeCsv(string? s)
        {
            if (s is null) return string.Empty;
            return s.Replace("\"", "\"\"");
        }

        private static string GetCategoryName(Token t)
        {
            if (t.KeywordInfo is not null) return t.KeywordInfo.Category.ToString();
            if (t.FunctionInfo is not null) return t.FunctionInfo.Category.ToString();
            if (t.OperatorInfo is not null) return t.OperatorInfo.Category.ToString();
            if (t.PunctuatorInfo is not null) return t.PunctuatorInfo.CommonSymbol.ToString();
            if (t.LiteralCategory is not null) return t.LiteralCategory.ToString();
            if (t.CommentType is not null) return t.CommentType.ToString();
            return string.Empty;
        }

        private static string GetInfo(Token t)
        {
            if (t.KeywordInfo is not null) return t.KeywordInfo.Lexeme;
            if (t.FunctionInfo is not null) return $"{t.FunctionInfo.Name} ({t.FunctionInfo.Category}, args {t.FunctionInfo.MinArgs}..{(t.FunctionInfo.MaxArgs == -1 ? "∞" : t.FunctionInfo.MaxArgs.ToString())})";
            if (t.OperatorInfo is not null) return t.OperatorInfo.Symbol;
            if (t.PunctuatorInfo is not null) return t.PunctuatorInfo.Lexeme;
            return string.Empty;
        }
    }
}
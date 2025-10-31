namespace Tokenizer.Core.Models
{
    // Minimal info-holder types for richer metadata

    public sealed record KeywordInfo
    {
        public string Lexeme { get; init; }
        public KeywordCategory Category { get; init; }
        public int SubKind { get; init; }

        public KeywordInfo(string lexeme, KeywordCategory category, int subKind = 0)
        {
            Lexeme = lexeme;
            Category = category;
            SubKind = subKind;
        }
    }

    public sealed record FunctionInfo
    {
        public string Name { get; init; }               // canonical name, e.g. "COUNT"
        public FunctionCategory Category { get; init; } // aggregate/scalar/string/...
        public int MinArgs { get; init; }               // min required args, -1 = unknown/unbounded
        public int MaxArgs { get; init; }               // max args, -1 = unlimited
        public string? Description { get; init; }

        public FunctionInfo(string name, FunctionCategory category, int minArgs = -1, int maxArgs = -1, string? description = null)
        {
            Name = name;
            Category = category;
            MinArgs = minArgs;
            MaxArgs = maxArgs;
            Description = description;
        }
    }

    public sealed record OperatorInfo
    {
        public string Symbol { get; init; }
        public OperatorCategory Category { get; init; }
        public int Precedence { get; init; }
        public bool LeftAssociative { get; init; }
        public bool Unary { get; init; }

        public OperatorInfo(string symbol, OperatorCategory category, int precedence, bool leftAssoc = true, bool unary = false)
        {
            Symbol = symbol;
            Category = category;
            Precedence = precedence;
            LeftAssociative = leftAssoc;
            Unary = unary;
        }
    }

    public sealed record PunctuatorInfo
    {
        public string Lexeme { get; init; }
        public CommonSymbol CommonSymbol { get; init; }

        public PunctuatorInfo(string lexeme, CommonSymbol commonSymbol)
        {
            Lexeme = lexeme;
            CommonSymbol = commonSymbol;
        }
    }
}
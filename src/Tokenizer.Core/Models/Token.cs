namespace Tokenizer.Core.Models
{
    // Lightweight token DTO
    public record Token(
        TokenType Type,
        string Lexeme,
        int Line,
        int Column
    )
    {
        // optional detailed classification (nullable)
        public KeywordCategory? KeywordCategory { get; init; }
        public FunctionCategory? FunctionCategory { get; init; }
        public LiteralCategory? LiteralCategory { get; init; }
        public OperatorCategory? OperatorCategory { get; init; }
        public CommentType? CommentType { get; init; }

        // detailed info pointers
        public KeywordInfo? KeywordInfo { get; init; }
        public FunctionInfo? FunctionInfo { get; init; }
        public OperatorInfo? OperatorInfo { get; init; }
        public PunctuatorInfo? PunctuatorInfo { get; init; }

        public override string ToString()
        {
            var cat = KeywordCategory?.ToString() ?? FunctionCategory?.ToString() ?? OperatorCategory?.ToString() ?? LiteralCategory?.ToString() ?? (CommentType?.ToString() ?? "");
            return $"{Type}: '{Lexeme}' ({Line}:{Column}){(string.IsNullOrEmpty(cat) ? "" : $" [{cat}]")}";
        }
    }
}
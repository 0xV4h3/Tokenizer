using System.Windows.Media;
using Tokenizer.Core.Models;
using Tokenizer.UI.Utils;

namespace Tokenizer.UI.ViewModels
{
    public class TokenViewModel
    {
        public string Type { get; }
        public string Lexeme { get; }
        public string Category { get; }
        public int Line { get; }
        public int Column { get; }
        public Brush Color { get; }

        public TokenViewModel(Token t)
        {
            Type = t.Type.ToString();
            Lexeme = t.Lexeme;
            Category = t.KeywordCategory?.ToString()
                       ?? t.FunctionCategory?.ToString()
                       ?? t.OperatorCategory?.ToString()
                       ?? t.LiteralCategory?.ToString()
                       ?? t.CommentType?.ToString()
                       ?? "";
            Line = t.Line;
            Column = t.Column;
            Color = TokenColoring.GetBrush(t);
        }
    }
}
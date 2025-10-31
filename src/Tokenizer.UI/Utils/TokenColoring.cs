using System.Windows.Media;
using Tokenizer.Core.Models;

namespace Tokenizer.UI.Utils
{
    public static class TokenColoring
    {
        public static Brush GetBrush(Token t)
        {
            switch (t.Type)
            {
                case TokenType.KEYWORD:
                    return Brushes.Blue;
                case TokenType.FUNCTION:
                    return Brushes.MediumPurple;
                case TokenType.OPERATOR:
                    return Brushes.DarkRed;
                case TokenType.PUNCTUATOR:
                    return Brushes.MediumVioletRed;
                case TokenType.LITERAL:
                    return Brushes.DarkGreen;
                case TokenType.IDENTIFIER:
                    return Brushes.DimGray;
                case TokenType.COMMENT:
                    return Brushes.Gray;
                case TokenType.END_OF_FILE:
                    return Brushes.DarkGray;
                case TokenType.UNKNOWN:
                    return Brushes.Red;
                default:
                    return Brushes.Black;
            }
        }
    }
}
using System.Collections.Generic;
using System.Threading;
using Tokenizer.Core.Models;

namespace Tokenizer.Core.Tokenizer
{
    public interface ITokenizer
    {
        IEnumerable<Token> Tokenize(string input);
        IAsyncEnumerable<Token> TokenizeAsync(string input, CancellationToken ct = default);
    }
}
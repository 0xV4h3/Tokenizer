using System.Linq;
using Tokenizer.Core.Tokenizer;
using Xunit;

namespace Tokenizer.Tests
{
    public class SerializationTests
    {
        [Fact]
        public void JsonExport_And_Import_Roundtrip()
        {
            var engine = new TokenizerEngine();
            var tokens = engine.Tokenize("SELECT 1;").ToList();
            var json = Tokenizer.Core.Utils.TokenSerializer.ToJson(tokens);
            var tokens2 = Tokenizer.Core.Utils.TokenSerializer.FromJson(json).ToList();
            Assert.Equal(tokens.Count, tokens2.Count);
            Assert.Equal(tokens[0].Lexeme, tokens2[0].Lexeme);
        }
        [Fact]
        public void CsvExport_Runs()
        {
            var engine = new TokenizerEngine();
            var tokens = engine.Tokenize("SELECT 1;").ToList();
            var csv = Tokenizer.Core.Utils.TokenSerializer.ToCsv(tokens);
            Assert.Contains("SELECT", csv);
        }
    }
}
using Tokenizer.Core.Models;
using Tokenizer.Core.Tokenizer;
using Xunit;

namespace Tokenizer.Tests
{
    public class PunctuatorRegistryTests
    {
        [Fact]
        public void Registry_Contains_StandardPunctuators()
        {
            var reg = new PunctuatorRegistry();
            Assert.True(reg.TryGet(',', out var info));
            Assert.True(reg.TryGet('(', out var info2));
        }
        [Fact]
        public void Registry_Can_Add_Custom()
        {
            var reg = new PunctuatorRegistry();
            reg.RegisterCustom(new Tokenizer.Core.Models.PunctuatorInfo(":", Tokenizer.Core.Models.CommonSymbol.COLON));
            Assert.True(reg.TryGet(':', out var info));
        }
    }
}
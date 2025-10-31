using Tokenizer.Core.Tokenizer;
using Xunit;

namespace Tokenizer.Tests
{
    public class KeywordRegistryTests
    {
        [Fact]
        public void Registry_Contains_StandardKeywords()
        {
            var reg = new KeywordRegistry();
            Assert.True(reg.TryGet("SELECT", out var info));
            Assert.Equal(Tokenizer.Core.Models.KeywordCategory.DML, info.Category);
        }

        [Fact]  
        public void Registry_Insensitive_ToCase()
        {
            var reg = new KeywordRegistry();
            Assert.True(reg.TryGet("select", out var info));
            Assert.True(reg.TryGet("SeLeCt", out var info2));
        }

        [Fact]
        public void Registry_Can_Add_Custom()
        {
            var reg = new KeywordRegistry();
            reg.RegisterCustom(new Tokenizer.Core.Models.KeywordInfo("MYCUSTOM", Tokenizer.Core.Models.KeywordCategory.MISC, 0));
            Assert.True(reg.TryGet("MYCUSTOM", out _));
        }
    }
}
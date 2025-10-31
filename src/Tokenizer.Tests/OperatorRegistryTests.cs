using Tokenizer.Core.Models;
using Tokenizer.Core.Tokenizer;
using Xunit;

namespace Tokenizer.Tests
{
    public class OperatorRegistryTests
    {
        [Fact]
        public void Registry_Contains_Operators()
        {
            var reg = new OperatorRegistry();
            Assert.True(reg.TryGet("+", out var info));
            Assert.True(reg.TryGet("=", out var info2));
        }
        [Fact]
        public void Registry_Insensitive_ToCase()
        {
            var reg = new OperatorRegistry();
            Assert.True(reg.TryGet("or", out var info));
            Assert.True(reg.TryGet("AND", out var info2));
        }
        [Fact]
        public void Registry_Can_Add_Custom()
        {
            var reg = new OperatorRegistry();
            reg.RegisterCustom(new Tokenizer.Core.Models.OperatorInfo("@@", Tokenizer.Core.Models.OperatorCategory.ARRAY, 0, true, false));
            Assert.True(reg.TryGet("@@", out _));
        }
    }
}
using Tokenizer.Core.Models;
using Tokenizer.Core.Tokenizer;
using Xunit;

namespace Tokenizer.Tests
{
    public class FunctionRegistryTests
    {
        [Fact]
        public void Registry_Contains_AggregateFunctions()
        {
            var reg = new FunctionRegistry();
            Assert.True(reg.TryGet("SUM", out var info));
            Assert.Equal(Tokenizer.Core.Models.FunctionCategory.AGGREGATE, info.Category);
        }
        [Fact]
        public void Registry_Insensitive_ToCase()
        {
            var reg = new FunctionRegistry();
            Assert.True(reg.TryGet("sum", out var info));
            Assert.True(reg.TryGet("SuM", out var info2));
        }
        [Fact]
        public void Registry_Can_Add_Custom()
        {
            var reg = new FunctionRegistry();
            reg.RegisterCustom(new Tokenizer.Core.Models.FunctionInfo("MYFUNC", Tokenizer.Core.Models.FunctionCategory.SCALAR, 1, 1, "Custom"));
            Assert.True(reg.TryGet("MYFUNC", out _));
        }
    }
}
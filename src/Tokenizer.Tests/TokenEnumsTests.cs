using Tokenizer.Core.Models;
using Xunit;

namespace Tokenizer.Tests
{
    public class TokenEnumsTests
    {
        [Fact]
        public void AllEnums_AreDefined()
        {
            Assert.True(System.Enum.IsDefined(typeof(TokenType), TokenType.FUNCTION));
            Assert.True(System.Enum.IsDefined(typeof(KeywordCategory), KeywordCategory.DDL));
            Assert.True(System.Enum.IsDefined(typeof(FunctionCategory), FunctionCategory.AGGREGATE));
            Assert.True(System.Enum.IsDefined(typeof(OperatorCategory), OperatorCategory.ARITHMETIC));
            Assert.True(System.Enum.IsDefined(typeof(LiteralCategory), LiteralCategory.STRING));
        }
    }
}
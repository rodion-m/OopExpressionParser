using System;
using FluentAssertions;
using Xunit;

namespace OopExpressionParser.Test
{
    public class ExpressionsTest
    {
        [Theory]
        [InlineData("2+2*2", 6)]
        [InlineData("2+2*10", 22)]
        [InlineData("2/2*10", 10)]
        [InlineData("2*0*10", 0)]
        [InlineData("100+100", 200)]
        [InlineData("200-100", 100)]
        [InlineData("100*100", 10000)]
        [InlineData("10000/100", 100)]
        public void TestExpression(string expression, long expected)
        {
            expression.ParseExpression().Should().Be(expected);
        }
    }
}
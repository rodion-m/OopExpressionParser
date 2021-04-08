using OopExpressionParser.Parsing;

namespace OopExpressionParser
{
    public static class Extensions
    {
        public static long ParseExpression(this string text)
        {
            var tokens = new ExpressionLexer(text).Tokenize();
            return new Parser(tokens).Parse();
        }
    }
}
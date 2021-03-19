using OopExpressionParser.Parser;

namespace OopExpressionParser
{
    public static class Extension
    {
        public static long ParseExpression(this string text)
        {
            var tokens = new ExpressionLexer(text).Tokenize();
            return new Parser.Parser(tokens).Parse();
        }
    }
}
namespace OopExpressionParser.Parsing
{
    public interface IToken {}

    public record NumberToken(long Number) : IToken;
}
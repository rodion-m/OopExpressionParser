namespace OopExpressionParser.Parser
{
    public interface IToken {}

    public record NumberToken(long number) : IToken;
}
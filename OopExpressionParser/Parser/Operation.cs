namespace OopExpressionParser.Parser
{
    public abstract record Operation(string name, int priority) : IToken
    {
        public abstract long Evaluate(NumberToken token1, NumberToken token2);

        public record Plus() : Operation("+", priority: 0)
        {
            public override long Evaluate(NumberToken token1, NumberToken token2)
                => token1.number + token2.number;
        }
        
        public record Minus() : Operation("-", priority: 0)
        {
            public override long Evaluate(NumberToken token1, NumberToken token2)
                => token1.number - token2.number;
        }

        public record Multiple() : Operation("*", priority: 1)
        {
            public override long Evaluate(NumberToken token1, NumberToken token2)
                => token1.number * token2.number;
        }
        
        public record Divide() : Operation("/", priority: 1)
        {
            public override long Evaluate(NumberToken token1, NumberToken token2)
                => token1.number / token2.number;
        }
        
        public record Xor() : Operation("^", priority: 2)
        {
            public override long Evaluate(NumberToken token1, NumberToken token2)
                => token1.number ^ token2.number;
        }
    }
}
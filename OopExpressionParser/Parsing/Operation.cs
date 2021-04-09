namespace OopExpressionParser.Parsing
{
    internal abstract record Operation(char Symbol, int Priority) : IToken
    {
        public abstract long Evaluate(NumberToken token1, NumberToken token2);

        public record Plus() : Operation('+', Priority: 0)
        {
            public override long Evaluate(NumberToken token1, NumberToken token2)
                => token1.Number + token2.Number;
        }
        
        public record Minus() : Operation('-', Priority: 0)
        {
            public override long Evaluate(NumberToken token1, NumberToken token2)
                => token1.Number - token2.Number;
        }

        public record Multiple() : Operation('*', Priority: 1)
        {
            public override long Evaluate(NumberToken token1, NumberToken token2)
                => token1.Number * token2.Number;
        }
        
        public record Divide() : Operation('/', Priority: 1)
        {
            public override long Evaluate(NumberToken token1, NumberToken token2)
                => token1.Number / token2.Number;
        }
        
        public record Xor() : Operation('^', Priority: 2)
        {
            public override long Evaluate(NumberToken token1, NumberToken token2)
                => token1.Number ^ token2.Number;
        }
    }
}
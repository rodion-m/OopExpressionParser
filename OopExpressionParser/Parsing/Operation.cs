using System;

namespace OopExpressionParser.Parsing
{
    internal abstract record Operation(char Symbol, int Priority) : IToken, IComparable<Operation>
    {
        public abstract long Evaluate(NumberToken token1, NumberToken token2);

        public static bool operator >(Operation a, Operation b) => a.Priority > b.Priority;
        public static bool operator <(Operation a, Operation b) => a.Priority < b.Priority;
        public int CompareTo(Operation? other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Priority.CompareTo(other.Priority);
        }

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
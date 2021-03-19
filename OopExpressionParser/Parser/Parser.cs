using System.Collections.Generic;
using System.Linq;

namespace OopExpressionParser.Parser
{
    public class Parser : IToken
    {
        private readonly List<IToken> _tokens;

        public Parser(List<IToken> tokens)
        {
            _tokens = tokens;
        }

        public long Parse()
        {
            var tokens = _tokens;
            var operationsByPriority = GetOperationsSorted(tokens);
            
            foreach (var operation in operationsByPriority)
            {
                var index = tokens.IndexOf(operation);
                var (leftIndex, rightIndex) = (index - 1, index + 1);
                var result = operation.Evaluate((NumberToken) tokens[leftIndex], (NumberToken) tokens[rightIndex]);
                tokens = ReplaceTokens(tokens, leftIndex, rightIndex, result);
            }

            return ((NumberToken) tokens[0]).number;
        }

        private static List<IToken> ReplaceTokens(List<IToken> tokens, int leftIndex, int rightIndex, long replacement)
        {
            return tokens
                .Take(leftIndex)
                .Append(new NumberToken(replacement))
                .Concat(tokens.Skip(rightIndex + 1))
                .ToList();
        }

        private static Operation[] GetOperationsSorted(List<IToken> tokens)
        {
            return tokens
                .Where(it => it is Operation)
                .Cast<Operation>()
                .OrderByDescending(it => it.priority)
                .ToArray();
        }
    }
}
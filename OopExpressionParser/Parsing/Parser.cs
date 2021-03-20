using System.Collections.Generic;
using System.Linq;

namespace OopExpressionParser.Parsing
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
            List<IToken> tokens = _tokens;
            var operationsByPriority = GetOperationsSorted(tokens);
            
            foreach (var operation in operationsByPriority)
            {
                var index = tokens.IndexOf(operation);
                var (leftIndex, rightIndex) = (index - 1, index + 1);
                var result = operation.Evaluate((NumberToken) tokens[leftIndex], (NumberToken) tokens[rightIndex]);
                ReplaceTokens(ref tokens, leftIndex, rightIndex, result);
            }

            return ((NumberToken) tokens[0]).number;
        }

        private static void ReplaceTokens(ref List<IToken> tokens, int leftIndex, int rightIndex, long replacement)
        {
            tokens.RemoveRange(leftIndex, rightIndex - leftIndex + 1);
            tokens.Insert(leftIndex, new NumberToken(replacement));
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
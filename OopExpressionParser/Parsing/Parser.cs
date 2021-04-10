using System;
using System.Collections.Generic;
using System.Linq;

namespace OopExpressionParser.Parsing
{
    public class Parser : IToken
    {
        private readonly LinkedList<IToken> _tokens;

        public Parser(LinkedList<IToken> tokens)
        {
            if (!tokens.Any()) throw new ArgumentException($"{nameof(tokens)} cannot be empty");
            _tokens = tokens;
        }

        public long Parse()
        {
            var operationsByPriority = GetOperationsSorted();
            foreach (var operationNode in operationsByPriority)
            {
                var operation = (Operation) operationNode.Value;
                var (prevNode, nextNode) = (operationNode.Previous, operationNode.Next);
                var (leftNumber, rightNumber) = ((NumberToken) prevNode!.Value, (NumberToken) nextNode!.Value);
                var result = operation.Evaluate(leftNumber, rightNumber);
                SimplifyOperation(operationNode, result);
            }

            return ((NumberToken) _tokens.First!.Value).Number;
        }

        private void SimplifyOperation(LinkedListNode<IToken> operationNode, long result)
        {
            _tokens.AddAfter(operationNode.Next!, new NumberToken(result));
            _tokens.Remove(operationNode.Previous!);
            _tokens.Remove(operationNode.Next!);
            _tokens.Remove(operationNode);
        }

        private List<LinkedListNode<IToken>> GetOperationsSorted()
        {
            var operationsNodes = new List<LinkedListNode<IToken>>();
            for (var node = _tokens.First!; node != null; node = node.Next)
            {
                if(node.Value is Operation)
                    operationsNodes.Add(node);
                
            }
            return operationsNodes.OrderByDescending(it => (Operation) it.Value).ToList();
        }
    }
}
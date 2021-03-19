using System;
using System.Collections.Generic;
using System.Linq;

namespace OopExpressionParser.Parser
{
    public interface ILexer<out T> where T: IToken
    {
        T? TokenizeOrNull(in string text, ref int index);
    }
    
        public class OperationLexer : ILexer<Operation>
    {
        private readonly Operation[] _operations;
        public OperationLexer()
        {
            _operations = typeof(Operation).Assembly.GetTypes()
                .Where(it => it.IsSubclassOf(typeof(Operation)))
                .Select(it => (Operation) Activator.CreateInstance(it)!)
                .ToArray();
        }

        public Operation? TokenizeOrNull(in string text, ref int index)
        {
            var c = text[index];
            var operation = _operations.SingleOrDefault(it => c == it.name[0]);
            if (operation != null) 
                ++index;
            return operation;
        }
    }

    public class NumberLexer : ILexer<NumberToken>
    {
        public NumberToken? TokenizeOrNull(in string text, ref int index)
        {
            if (!IsNumberChar(text[index]))
                return null;
            
            var chars = new List<char>() {text[index]};
            var startIndex = index;
            for (var i = startIndex + 1; i < text.Length && IsNumberChar(text[i]); i++)
            {
                chars.Add(text[i]);
            }

            index += chars.Count;

            return new NumberToken(int.Parse(new string(chars.ToArray())));
        }

        private bool IsNumberChar(char c) => c >= '0' && c <= '9';
    }
    
    public class ExpressionLexer
    {
        public string Text { get; }

        private static readonly ILexer<IToken>[] _parsers =
        {
            new NumberLexer(), 
            new OperationLexer()
        };

        public ExpressionLexer(string text)
        {
            Text = text;
        }
        
        public List<IToken> Tokenize()
        {
            var tokens = new List<IToken>();
            int i = 0;
            do
            {
                IToken? maybeToken = null;
                foreach (var parser in _parsers)
                {
                    maybeToken = parser.TokenizeOrNull(Text, ref i);
                    if (maybeToken != null) break;
                }

                if (maybeToken != null)
                    tokens.Add(maybeToken);
                else
                    throw new ApplicationException($"Unknown symbol at position: {i}");
            } while (i < Text.Length);

            return tokens;
        }
    }

}
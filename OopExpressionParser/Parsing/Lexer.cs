using System;
using System.Collections.Generic;
using System.Linq;

namespace OopExpressionParser.Parsing
{
    internal abstract class Lexer
    {
        public abstract IToken? TokenizeOrNull(in string text, ref int index);
    }

    internal class OperationLexer : Lexer
    {
        private static readonly Operation[] _operations = ReflectionEx.CreateAllSubclassesOf<Operation>();

        public override Operation? TokenizeOrNull(in string text, ref int index)
        {
            var c = text[index];
            var operation = _operations.SingleOrDefault(it => c == it.symbol);
            if (operation != null)
                ++index;
            return operation;
        }
    }

    internal class NumberLexer : Lexer
    {
        public override NumberToken? TokenizeOrNull(in string text, ref int index)
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
        
        public ExpressionLexer(string text)
        {
            Text = text;
        }
        
        private static readonly Lexer[] _lexers = ReflectionEx.CreateAllSubclassesOf<Lexer>();

        public List<IToken> Tokenize()
        {
            var tokens = new List<IToken>();
            int i = 0;
            do
            {
                IToken? maybeToken = null;
                foreach (var lexer in _lexers)
                {
                    maybeToken = lexer.TokenizeOrNull(Text, ref i);
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
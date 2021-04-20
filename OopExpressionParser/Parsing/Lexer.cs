using System;
using System.Collections.Generic;
using System.Linq;

namespace OopExpressionParser.Parsing
{
    internal abstract class Lexer
    {
        public abstract IToken? TokenizeOrNull(in ReadOnlySpan<char> text, ref int index);
    }

    internal class OperationLexer : Lexer
    {
        private static readonly Operation[] _operations = ReflectionEx.CreateAllSubclassesOf<Operation>();

        public override Operation? TokenizeOrNull(in ReadOnlySpan<char> text, ref int index)
        {
            var c = text[index];
            var operation = _operations.SingleOrDefault(it => c == it.Symbol);
            if (operation != null)
                ++index;
            return operation;
        }
    }

    internal class NumberLexer : Lexer
    {
        public override NumberToken? TokenizeOrNull(in ReadOnlySpan<char> text, ref int index)
        {
            if (!IsNumberChar(text[index]))
                return null;

            var seekIndex = index + 1;
            while (seekIndex < text.Length && IsNumberChar(text[seekIndex]))
                seekIndex++;

            var result = new NumberToken(int.Parse(text[index..seekIndex]));
            index = seekIndex;

            return result;
        }

        private static bool IsNumberChar(char c) => c is >= '0' and <= '9';
    }

    public class ExpressionLexer
    {
        public string Text { get; }
        
        public ExpressionLexer(in string text)
        {
            Text = text;
        }
        
        private static readonly Lexer[] _lexers = ReflectionEx.CreateAllSubclassesOf<Lexer>();

        public LinkedList<IToken> Tokenize()
        {
            var textSpan = Text.AsSpan();
            var tokens = new LinkedList<IToken>();
            var index = 0;
            do
            {
                IToken? maybeToken = null;
                foreach (var lexer in _lexers)
                {
                    maybeToken = lexer.TokenizeOrNull(textSpan, ref index);
                    if (maybeToken != null) break;
                }

                if (maybeToken != null)
                    tokens.AddLast(maybeToken);
                else
                    throw new ApplicationException($"Unknown symbol at position: {index}");
            } while (index < Text.Length);

            return tokens;
        }
    }
}
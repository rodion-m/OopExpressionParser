using System;

namespace OopExpressionParser.Parsing
{
    public class UnknownTokenException : Exception
    {
        public UnknownTokenException(int index) : base($"Unknown symbol at position: {index}")
        {
        }
    }
}
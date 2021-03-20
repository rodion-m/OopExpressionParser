using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using CodingSeb.ExpressionEvaluator;
using Flee.PublicTypes;
using NCalc;

namespace OopExpressionParser.Benchmark
{
    public class OopParserVsOthers
    {
        private const int N = 100;
        private readonly string data;

        public OopParserVsOthers()
        {
            const string exp = "2894032+194832-48391-40*1000-473-2000000-10000000/1000000-999990";
            var sb = new StringBuilder(exp);
            for (int i = 0; i < N; i++)
            {
                sb.Append($"+{exp}");
            }

            data = sb.ToString();
        }
        
        [Benchmark]
        public long OopParser() => data.ParseExpression();

        [Benchmark]
        public int Flee()
        {
            var context = new ExpressionContext();
            var e = context.CompileGeneric<int>(data);
            return e.Evaluate();
        }
        
        [Benchmark]
        public double NCalc()
        {
            var expr = new Expression(data);
            return (double) expr.Evaluate();
        }
        
        [Benchmark]
        public int ExpressionEvaluator()
        {
            var evaluator = new ExpressionEvaluator();
            return (int) evaluator.Evaluate(data);
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<OopParserVsOthers>();
        }
    }
}
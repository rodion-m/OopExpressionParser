using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace OopExpressionParser.Test
{
    public class Test
    {
        [Fact]
        public void Test1()
        {
            var vals = new[] { "z", "ban", "frut", "ded", "ban", "ded", "ban", "ded", "frut" };
            var res = vals.Aggregate(new SortedDictionary<string, int>(), (dict, s) =>
            {
                if (!dict.TryAdd(s, 0)) dict[s]++;
                return dict;
            });
            //res.Keys.Should().BeEquivalentTo("ban", "ded", "frut", "z");
            Sum(1)(2)(3);
        }

        IEnumerable<int> I(params int[] vals)
        {
            var sum = 1;
            return vals.Select((current) => sum *= current);
        }


        dynamic Sum(int v)
        {
            Console.WriteLine(v);
            return new Func<int, dynamic>(i => Sum(v + i));
        }
        
        public static Func<TResult> Apply<TResult, TArg> (Func<TArg, TResult> func, TArg arg)
        {
            return () => func(arg);
        }

        public static Func<TResult> Apply<TResult, TArg1, TArg2> (Func<TArg1, TArg2, TResult> func,
            TArg1 arg1, TArg2 arg2)
        {
            return () => func(arg1, arg2);
        }
    }
}
using System;
using OopExpressionParser;

string? input;
Console.WriteLine("Write expression (for example 2+2*2):");
while ((input = Console.ReadLine()) != null)
{
    try
    {
        Console.WriteLine(input.ParseExpression());
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
}

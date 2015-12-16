using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace Expressions
{
    class Program
    {
        static void Main(string[] args)
        {
            //PrintUsingCSharp();
            //PrintUsingExpressions();
            //AssigningAgainstRules();
            CSharpExample();
            LinqExample();
        }

        static void PrintUsingCSharp()
        {
            var s = Stopwatch.StartNew();
            Console.WriteLine("DLR using C#");
            s.Stop();
            Console.WriteLine("Time spent = {0} ticks", s.ElapsedTicks);
        }

        static void PrintUsingExpressions()
        {
            var s = Stopwatch.StartNew();
            var method = typeof(Console).GetMethod("WriteLine", new[] { typeof(String) });

            var callExpression = Expression.Call(null, method, Expression.Constant("DLR from Expression"));
            var callDelegate = Expression.Lambda<Action>(callExpression).Compile();
            callDelegate();
            s.Stop();
            Console.WriteLine("Time spent = {0} ticks", s.ElapsedTicks);
            var s2 = Stopwatch.StartNew();
            callDelegate();
            s2.Stop();
            Console.WriteLine("Time spent = {0} ticks", s2.ElapsedTicks);
        }

        static void AssigningAgainstRules()
        {
            MethodInfo method = typeof(String).GetMethod("ToLower", new Type[] { });
            ParameterExpression x = Expression.Variable(typeof(String), "x");
            ParameterExpression y = Expression.Variable(typeof(String), "y");
            Expression blockExpression = Expression.Block(new[] { x, y },
                Expression.Assign(x, Expression.Constant("Hello")),
                Expression.Assign(y,
                    Expression.Condition(Expression.Constant(true),
                        Expression.Call(x, method),
                        Expression.Default(typeof(String)),
                        typeof(String)))
                );

            Func<string> blockDelegate = Expression.Lambda<Func<String>>(blockExpression).Compile();
            String result = blockDelegate();
            Console.WriteLine("Assignment result={0}", result);
        }

        static void CSharpExample()
        {
            double result = (10d + 20d) / 3d;
            Console.WriteLine(result);
        }

        static void LinqExample()
        {
            Expression binaryExpression = Expression.Divide(
                Expression.Add(
                    Expression.Constant(10d),
                    Expression.Constant(20d)),
                Expression.Constant(3d));

            Func<double> binaryDelegate = Expression.Lambda<Func<double>>(binaryExpression).Compile();
            Console.WriteLine(binaryDelegate());
        }
    }
}
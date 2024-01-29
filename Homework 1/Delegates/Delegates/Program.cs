using System.Diagnostics;
using Delegates;

const string query = "Arr[0].Brr[2].Crr[3]";
A objectA = new A();

foreach (var tuple in QueryParser.ParseQuery(query))
{
    Console.WriteLine($"({tuple.Item1}, {tuple.Item2})");
}

var reflectionFunc = ReflectionFieldArrayAccess.FieldArrayAccess<A>(query);
Stopwatch watch = Stopwatch.StartNew();
var result = reflectionFunc.Invoke(objectA);
watch.Stop();
Console.WriteLine($"Reflection approach executed in {watch.ElapsedTicks} ns., " +
                  $"result is {result}");

watch.Reset();

var expressionFunc = ExpressionFieldArrayAccess.FieldArrayAccess<A>(query);
watch.Start();
result = expressionFunc.Invoke(objectA);
watch.Stop();
Console.WriteLine($"Expression tree approach executed in {watch.ElapsedTicks} ns., " +
                  $"result is {result}");

internal class A
{
    public B?[] Arr = {new(), null};
}

internal class B
{
    public C?[] Brr = {new(), null, new(), null};
}

internal class C
{
    public string?[] Crr = {"Dima", "Roma", null, "Egor"};
}
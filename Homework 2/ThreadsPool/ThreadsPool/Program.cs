using ThreadsPool.Task;

namespace ThreadsPool;

public static class Program
{
    private static void Main()
    {
       using ThreadPool threadPool = new(2);
        
        var taskOne = new MyTask<int>(() => 5);
        var taskTwo = new MyTask<int>(() => 10);
        var taskThree = taskOne.ContinueWith(result => result + 10);
        var taskFour = taskTwo.ContinueWith(_ =>
        {
            Thread.Sleep(1000);
            throw new InvalidOperationException("Exception is expected");
            return 0;
        });

        threadPool.Enqueue(taskOne);
        threadPool.Enqueue(taskTwo);

        Console.WriteLine(taskOne.Result + " " + taskOne.IsCompleted);
        Console.WriteLine(taskTwo.Result + " " + taskTwo.IsCompleted);
        Console.WriteLine(taskThree.Result + " " + taskThree.IsCompleted);
        try
        {
            Console.WriteLine(taskFour.Result + " " + taskFour.IsCompleted);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            Console.WriteLine(taskFour.IsCompleted);
        }
    }
}
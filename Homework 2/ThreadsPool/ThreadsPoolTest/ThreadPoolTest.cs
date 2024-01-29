using System;
using System.Collections.Generic;
using NUnit.Framework;
using ThreadsPool;
using ThreadsPool.Task;

namespace ThreadsPoolTest;

public class ThreadPoolTest
{
    private ThreadPool _threadPool = null!;
    private const int ThreadsNumber = 10;
    
    private readonly Func<int> _returnOne = () => 1;
    private readonly Func<int, int> _multiplyByTwo = input => input * 2;
    private readonly Func<int> _throwException = () =>
    {
        throw new InvalidOperationException("Exception is expected");
        return 1;
    };
    
    [SetUp]
    public void Setup()
    {
        _threadPool = new ThreadPool(ThreadsNumber);
    }
    
    [TearDown]
    public void TearDown()
    {
        _threadPool.Dispose();
    }
    
    [Test]
    public void VerifyInitializedThreadsNumber()
    {
        Assert.AreEqual(ThreadsNumber, _threadPool.ThreadsNumber);
    }

    [Test]
    public void EnqueueOneTask()
    {
        MyTask<int> task = new(_returnOne);
        _threadPool.Enqueue(task);
        
        Assert.AreEqual(1, task.Result);
    }
    
    [Test]
    public void EnqueueOneTaskWithContinuation()
    {
        MyTask<int> task = new(_returnOne);
        IMyTask<int> continuation = task.ContinueWith(_multiplyByTwo);
        _threadPool.Enqueue(task);
        
        Assert.AreEqual(1, task.Result);
        Assert.AreEqual(2, continuation.Result);
    }
    
    [Test]
    public void EnqueueOneTaskWithMultipleContinuations()
    {
        const int continuationsNumber = ThreadsNumber * 3;
        
        MyTask<int> task = new(_returnOne);
        List<IMyTask<int>> continuations = new();
        for (int i = 0; i < continuationsNumber; i++)
        {
            IMyTask<int> continuation = task.ContinueWith(_multiplyByTwo);
            continuations.Add(continuation);
        }
        _threadPool.Enqueue(task);
        
        Assert.AreEqual(1, task.Result);
        for (int i = 0; i < continuationsNumber; i++)
        {
            Assert.AreEqual(2, continuations[i].Result);
        }
    }
    
    [Test]
    public void EnqueueOneTaskWithSequentialContinuations()
    {
        const int continuationsNumber = ThreadsNumber * 3;
        
        MyTask<int> task = new(_returnOne);
        List<IMyTask<int>> continuations = new();
        IMyTask<int> currentContinuation = task;
        for (int i = 0; i < continuationsNumber; i++)
        {
            currentContinuation = currentContinuation.ContinueWith(_multiplyByTwo);
            continuations.Add(currentContinuation);
        }
        _threadPool.Enqueue(task);
        
        Assert.AreEqual(1, task.Result);
        int currentExpected = 2;
        for (int i = 0; i < continuationsNumber; i++)
        {
            Assert.AreEqual(currentExpected, continuations[i].Result);
            currentExpected *= 2;
        }
    }
    
    [Test]
    public void EnqueueMultipleTasks()
    {
        const int tasksNumber = ThreadsNumber * 3;
        
        List<MyTask<int>> tasks = new();
        for (int i = 0; i < tasksNumber; i++)
        {
            MyTask<int> task = new(_returnOne);
            tasks.Add(task);
            _threadPool.Enqueue(task);
        }
        
        for (int i = 0; i < tasksNumber; i++)
        {
            Assert.AreEqual(1, tasks[i].Result);
        }
    }
    
    [Test]
    public void EnqueueMultipleTasksWithContinuation()
    {
        const int tasksNumber = ThreadsNumber * 3;
        
        List<MyTask<int>> tasks = new();
        List<IMyTask<int>> continuations = new();
        for (int i = 0; i < tasksNumber; i++)
        {
            MyTask<int> task = new(_returnOne);
            tasks.Add(task);
            continuations.Add(task.ContinueWith(_multiplyByTwo));
            _threadPool.Enqueue(task);
        }
        
        for (int i = 0; i < tasksNumber; i++)
        {
            Assert.AreEqual(1, tasks[i].Result);
        }
        for (int i = 0; i < tasksNumber; i++)
        {
            Assert.AreEqual(2, continuations[i].Result);
        }
    }

    [Test]
    public void SpecifyIncorrectThreadsNumber()
    {
        Assert.Throws<ArgumentException>(() =>
        {
            ThreadPool _ = new ThreadPool(0);
        });
    }
    
    [Test]
    public void EnqueueTaskWithException()
    {
        MyTask<int> task = new(_throwException);
        
        _threadPool.Enqueue(task);
        Assert.Throws<AggregateException>(() =>
        {
            int _ = task.Result;
        });
    }
    
    [Test]
    public void EnqueueTaskAfterDispose()
    {
        ThreadPool threadPool = new ThreadPool(ThreadsNumber);
        MyTask<int> task = new(_returnOne);
        
        threadPool.Enqueue(task);
        Assert.AreEqual(1, task.Result);
        
        threadPool.Dispose();
        Assert.Throws<ObjectDisposedException>(() => threadPool.Enqueue(task));
    }
}
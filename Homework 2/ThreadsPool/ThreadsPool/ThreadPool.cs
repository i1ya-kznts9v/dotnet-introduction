using ThreadsPool.Task;

namespace ThreadsPool;

public class ThreadPool : IDisposable
{
    private readonly Queue<Thread> _threads = new();
    private readonly Queue<IMyTask> _tasks = new();
    
    private readonly object _lockObject = new();

    private readonly CancellationTokenSource _cancellationTokenSource = new();
    private readonly CancellationToken _cancellationToken;

    public ThreadPool(int threadsNumber)
    {
        if (threadsNumber < 1) throw new ArgumentException("Threads number must be >= 1");
        _cancellationToken = _cancellationTokenSource.Token;
        
        for (int i = 0; i < threadsNumber; i++)
        {
            Thread thread = new(WaitForTaskAndExecute);
            thread.Start();
            _threads.Enqueue(thread);
        }
    }
    
    public int ThreadsNumber => _threads.Count;
    
    private bool IsCanceled => _cancellationToken.IsCancellationRequested;
    
    private void WaitForTaskAndExecute()
    {
        while (true)
        {
            lock (_lockObject)
            {
                while (_tasks.Count == 0)
                {
                    if (IsCanceled) return;
                    Monitor.Wait(_lockObject);
                }
                
                IMyTask task = _tasks.Dequeue();
                task.Execute();
                
                task.Continuations?.ForEach(_tasks.Enqueue);
            }
        }
    }

    public void Enqueue(IMyTask task)
    {
        lock (_lockObject)
        {
            if (IsCanceled) throw new ObjectDisposedException("ThreadPool was disposed, " +
                                                              "new tasks are not accepted for execution");
            _tasks.Enqueue(task);
            Monitor.PulseAll(_lockObject);
        }
    }

    private void Dispose(bool isDisposing) {
        lock (_lockObject)
        {
            if (IsCanceled) return;
            _cancellationTokenSource.Cancel();
                
            if (isDisposing) {
                Monitor.PulseAll(_lockObject);
            }
        }
    }
    
    public void Dispose() {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    ~ThreadPool() {
        Dispose(false);
    }
}
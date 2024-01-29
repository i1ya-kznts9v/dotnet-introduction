namespace ThreadsPool.Task;

public class MyTask<TResult> : IMyTask<TResult>
{
    private readonly Func<TResult?> _func;
    private AggregateException? _aggregateException;
    private readonly AutoResetEvent _autoEvent = new(false);
    
    private List<IMyTask>? _continuations;
    
    private bool _isCompleted;
    private TResult? _result;

    public MyTask(Func<TResult?> func) => _func = func;
    
    public List<IMyTask>? Continuations => _continuations;
    
    public bool IsCompleted => _isCompleted;
    public TResult? Result => WaitForResultOrException();
    private TResult? WaitForResultOrException()
    {
        if (!_isCompleted) _autoEvent.WaitOne();
        return _aggregateException != null ? throw _aggregateException : _result;
    }

    public void Execute()
    {
        try
        {
            _result = _func.Invoke();
        }
        catch (Exception exception)
        {
            _aggregateException = new AggregateException(exception);
        }
        finally
        {
            _isCompleted = true;
            _autoEvent.Set();
        }
    }
    
    public IMyTask<TContinuationResult?> ContinueWith<TContinuationResult>(Func<TResult?, TContinuationResult?> func)
    {
        _continuations ??= new List<IMyTask>();
        
        MyTask<TContinuationResult?> continuation = new(() => func.Invoke(Result));
        _continuations.Add(continuation);
        return continuation;
    }
}
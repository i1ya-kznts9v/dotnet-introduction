namespace ThreadsPool.Task;

public interface IMyTask
{
    public void Execute();

    public List<IMyTask>? Continuations { get; }
    
    public bool IsCompleted { get; }
}

public interface IMyTask<out TResult> : IMyTask
{
    public IMyTask<TContinuationResult?> ContinueWith<TContinuationResult>
        (Func<TResult?, TContinuationResult?> task);

    public TResult? Result { get; }
}
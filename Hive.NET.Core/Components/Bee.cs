namespace Hive.NET.Core.Components;

internal class Bee
{
    public Guid Id { get;}
    public bool IsWorking { get; private set; }

    public Bee()
    {
        Id = Guid.NewGuid();
        IsWorking = false;
    }

    public async Task DoWork(BeeWork unitOfWork, 
        BeeFinishedWorkCallback beeCallback, 
        Action onSuccess = default, 
        Action<Exception> onFailure = default)
    {
        try
        {
            IsWorking = true;

            unitOfWork.task.Start();
            await unitOfWork.task;

            IsWorking = false;

            beeCallback(this);
            onSuccess?.Invoke();
        }
        catch (Exception ex)
        {
            onFailure?.Invoke(ex);
        }
    }

    public delegate void BeeFinishedWorkCallback(Bee bee);

    public Action BeeOnSuccessCallback;
    public Action BeeOnFailureCallback;
}
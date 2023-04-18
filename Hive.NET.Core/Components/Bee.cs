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

    public async Task DoWork(BeeWorkItem unitOfWork, 
        BeeFinishedWorkCallback beeCallback)
    {
        try
        {
            IsWorking = true;

            unitOfWork.task.Start();
            await unitOfWork.task;

            IsWorking = false;

            beeCallback(this);
            //todo refactor to use internal logging to enable log level filtering
            Console.WriteLine("{0:MM/dd/yyyy}: bzzzt, nothing happened", DateTime.UtcNow);
            unitOfWork.onSuccess?.Invoke();
        }
        catch (Exception ex)
        {
            unitOfWork.onFailure?.Invoke(ex);
        }
    }

    public delegate void BeeFinishedWorkCallback(Bee bee);
}
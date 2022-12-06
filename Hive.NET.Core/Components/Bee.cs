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

    public async Task DoWork(Task unitOfWork, BeeFinishedWorkCallback beeCallback)
    {
        IsWorking = true;
        
        unitOfWork.Start();
        await unitOfWork;
        
        IsWorking = false;

        beeCallback(this);
    }

    public delegate void BeeFinishedWorkCallback(Bee bee);
}
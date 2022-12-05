namespace Hive.NET.Core.Components;

public class Bee
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
        Console.WriteLine($"Bee {Id} is working");

        unitOfWork.Start();

        await unitOfWork;

        Console.WriteLine($"Bee {Id} finished working");
        IsWorking = false;

        beeCallback(this);
    }

    public delegate void BeeFinishedWorkCallback(Bee bee);
}
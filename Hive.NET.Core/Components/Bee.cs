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

    public void DoWork(Task unitOfWork)
    {
        IsWorking = true;
        Console.WriteLine($"Bee {Id} is working");

        unitOfWork.Start();

        Console.WriteLine($"Bee {Id} finished working");
        IsWorking = false;
    }
}
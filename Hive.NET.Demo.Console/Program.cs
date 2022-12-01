using Hive.NET.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Hive.NET.Demo.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            IConfiguration configuration = builder.Build();
            
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.ConfigureHive();
                }).Build();
            
            var service = ActivatorUtilities.CreateInstance<HiveDemoService>(host.Services);
            service.Run();
        }
    }
}




// // See https://aka.ms/new-console-template for more information
//
// using Hive.NET.Core.Components;
//
// Console.WriteLine("Hello, World!");
//
// service
//
//
//
//
//
// var tasks = new List<Task>();
//
// for (int i = 0; i < 20; i++)
// {
//     var i1 = i;
//     tasks.Add(new Task(() =>
//     {
//         var delay = 1000 + (i1 * 100);
//         Console.WriteLine($"Task {i1} with delay {delay}");
//         Thread.Sleep(delay);
//     }));
// }
//
// var swarm = new List<Bee>
// {
//     new Bee(),
//     new Bee()
// };
//
// Console.WriteLine("Let the hive begin");
//
// while (true)
// {
//     if (tasks.Count is 0 && swarm.TrueForAll(x => x.IsWorking == false))
//     {
//         Console.WriteLine("Done all tasks!");
//         break;
//     }
//
//     var bee = swarm.FirstOrDefault(x => x.IsWorking == false);
//
//     if (bee is null)
//     {
//         Console.WriteLine("Waiting for free bee");
//         Thread.Sleep(500);
//         continue;
//     }
//
//     var jobToDo = tasks.First();
//     tasks.Remove(jobToDo);
//     bee.DoWork(jobToDo);
// }

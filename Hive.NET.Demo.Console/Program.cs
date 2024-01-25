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
            BuildConfiguration(builder);
            IConfiguration configuration = builder.Build();

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((_, services) =>
                {
                    services.ConfigureHive(configuration);
                }).Build();

            host.Services.UseHive();

            var service = ActivatorUtilities.CreateInstance<HiveDemoService>(host.Services);
            service.Run();
        }

        static void BuildConfiguration(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();
        }
    }

    public delegate void BeeReturnedCallback(string result);

    public class CallBack
    {
        public void StartNewTask(BeeReturnedCallback beeReturned)
        {
            System.Console.WriteLine("I have started new Task.");

            if (beeReturned != null)
                beeReturned("I have completed Task.");
        }
    }

    public class CallBackTest
    {
        public void Test()
        {
            BeeReturnedCallback callback = TestCallBack;
            CallBack testCallBack = new CallBack();
            testCallBack.StartNewTask(callback);
        }
        public void TestCallBack(string result)
        {
            System.Console.WriteLine(result);
        }
    }
}
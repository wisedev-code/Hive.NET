using Hive.NET.Core.Manager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Hive.NET.Extensions.Api
{
    public class HiveApi : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            new Thread(
                delegate ()
                {
                    var builder = WebApplication.CreateBuilder();
                    builder.WebHost.UseUrls("https://*:7007");

                    var app = builder.Build();
                    app.MapGet("/hives", GetHives);
                    app.MapGet("/hives/{id:guid}", GetHive);
                    app.MapGet("/hives/{id:guid}/issues", GetHiveIssues);

                    app.Run();
                }).Start();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        
        private IResult GetHives()
        {
            var hiveManager = HiveManager.GetInstance();
            return Results.Ok(hiveManager.GetHives());
        }
        
        private IResult GetHive(Guid id)
        {
            var hiveManager = HiveManager.GetInstance();
            return Results.Ok(hiveManager.GetHiveDetails(id));
        }
        
        private IResult GetHiveIssues(Guid id)
        {
            var hiveManager = HiveManager.GetInstance();
            return Results.Ok(hiveManager.GetHiveRegisteredErrors(id));
        }
    }
}
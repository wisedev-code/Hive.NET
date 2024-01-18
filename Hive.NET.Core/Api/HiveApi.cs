using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Hive.NET.Core.Api
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
                    app.MapGet("/analytics", Get);
                    
                    app.UseSwagger();
                    app.Run();
                }).Start();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        
        private IResult Get()
        {
            return Results.Ok();
        }
    }
}
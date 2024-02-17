using Hive.NET.Core.Configuration;
using Hive.NET.Core.Factory;
using Hive.NET.Demo.Api;
using Hive.NET.Demo.Api.Repositories;
using Hive.NET.Demo.Api.Repositories.Interfaces;
using Hive.NET.Demo.Api.Services;
using Hive.NET.Demo.Api.Services.Interfaces;
using Hive.NET.Extensions;
using Hive.NET.Persistence;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c => { c.EnableAnnotations(); });
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
builder.Services.AddSingleton<ICustomerUpdateService, CustomerUpdateService>();
builder.Services.AddSingleton<IOrderUpdateService, OrderUpdateService>();

builder.Services.ConfigureHive(configuration: builder.Configuration);
builder.Services.AddHiveApi();
builder.Services.AddHiveSignalR();
builder.Services.IncludeHiveFileStorage("./hives.json");

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();
app.UseRouting();

app.UseHive();
app.MapHiveHub("/hubs/hive");

//Possible you would like to extract it into extension method.
HiveFactory.CreateHive(2, Keys.CustomerHiveName);
HiveFactory.CreateHive(5, Keys.OrderHiveName);


app.Run();


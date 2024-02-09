using Hive.NET.Core.Configuration;
using Hive.NET.Core.Factory;
using Hive.NET.Demo.Api.Repositories;
using Hive.NET.Demo.Api.Repositories.Interfaces;
using Hive.NET.Demo.Api.Services;
using Hive.NET.Demo.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c => { c.EnableAnnotations(); });
builder.Services.AddSingleton<ICustomerRepository, CustomerRepository>();
builder.Services.AddSingleton<IOrderRepository, OrderRepository>();
builder.Services.AddSingleton<ICustomerUpdateService, CustomerUpdateService>();
builder.Services.AddSingleton<IOrderUpdateService, OrderUpdateService>();

builder.Services.ConfigureHive(configuration: builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHive();

app.UseHttpsRedirection();
app.MapControllers();

//Possible you would like to extract it into extension method.
HiveFactory.CreateHive(5, "customerHive");
HiveFactory.CreateHive(2, "OrderHive");


app.Run();


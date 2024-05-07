using BinanceTradingMonitoring.core.Bussiness.Implemantions;
using BinanceTradingMonitoring.core.Bussiness.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Register the implementation of IApiConnector
builder.Services.AddSingleton<IApiConnector, ApiConnector>(); // Assuming ApiConnector implements IApiConnector

builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
using BinanceTradingMonitoring.core.Bussiness.Implemantions;
using BinanceTradingMonitoring.core.Bussiness.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowSpecificOrigin",
            builder =>
            {
                builder.WithOrigins("*") // Specify the allowed origins here
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
    });

    // Other service configurations...


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
// Enable CORS middleware
app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
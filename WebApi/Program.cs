using Autofac.Core;
using BinanceTradingMonitoring.core.Bussiness.Implemantions;
using BinanceTradingMonitoring.core.Bussiness.Interfaces;
using BinanceTradingMonitoring.core.Helpers;
using WebApi.Hubs;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("*") // Specify the allowed origins, allowe all sites
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
builder.Services.AddSingleton<IApiConnector, ApiConnector>(); // Assuming ApiConnector implements IApiConnector
builder.Services.AddSingleton<IJsonParser, JsonParser>();// Assuming JsonParser implements IJsonParser
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR(); // Add SignalR services

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowSpecificOrigin"); // Enable CORS middleware before authorization
app.UseAuthorization();
app.MapControllers();
app.MapHub<TradeHub>("/tradeHub"); // Map the SignalR hub endpoint
app.Run();
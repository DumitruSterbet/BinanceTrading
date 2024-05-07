using Autofac;
using BinanceTradingMonitoring.core.Bussiness;
using BinanceTradingMonitoring.core.Bussiness.Implemantions;
using BinanceTradingMonitoring.core.Bussiness.Interfaces;
using BinanceTradingMonitoring.core.Helpers;

namespace BinanceTradingMonitoring.core
{
    class Program
    {
        static void Main(string[] args)
        {
            // Disable Quick Edit Mode to prevent stopping when clicking on the console
            ConsoleHelper.DisableQuickEditMode();

            // Set up the dependency injection container
            var builder = new ContainerBuilder();

            // Register your dependencies
            builder.RegisterType<BinanceTrade>().As<IBinanceTrading>();
            builder.RegisterType<ApiConnector>().As<IApiConnector>();

            // Build the container
            var container = builder.Build();

            // Resolve the entry point of your application from the container
            using (var scope = container.BeginLifetimeScope())
            {
                var binanceTrade = scope.Resolve<IBinanceTrading>();
                    binanceTrade.RunTradeMonitor();
            }

           
           

        }
    }
}
using BinanceTradingMonitoring.core.Bussiness;
using BinanceTradingMonitoring.core.Helpers;

namespace BinanceTradingMonitoring.core
{
    class Program
    {
        static void Main(string[] args)
        {
            // Disable Quick Edit Mode to prevent stopping when clicking on the console
            ConsoleHelper.DisableQuickEditMode();
            // Run the Binance trade monitoring functionality
            new BinanceTrade().RunTradeMonitor();

        }
    }
}
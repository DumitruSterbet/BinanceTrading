using BinanceTradingMonitoring.Bussiness;
using BinanceTradingMonitoring.Helpers;

namespace MainClass
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
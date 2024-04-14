

namespace BinanceTradingMonitoring.Helpers
{
    /// <summary>
    /// Provides constant values for interacting with the Binance API.
    /// </summary>
    public class Constant
    {
        // Constants with URL to Binance API 
        public const string GetCurrenciesPairsURL = "https://api.binance.com/api/v3/exchangeInfo";
        public const string GetSubscriptionsURL = "wss://stream.binance.com:9443/ws/{pair}@trade";

        // Constant used for parse JSON
        public const string FirstCurrencieProperty = "baseAsset";
        public const string SecondCurrencieProperty = "quoteAsset";
        public const string Symbols = "symbols";
        public const string BuyingFlag = "m";
        public const string IsSpotTrading = "isSpotTradingAllowed";

        // Constants related to trade cleanup
        public const int MaxTradesToKeep = 10;
        public const int CleanupIntervalSeconds = 6;

        // Constants for console mode flags
        public const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
        public const uint ENABLE_EXTENDED_FLAGS = 0x0080;
        public const int STD_INPUT_HANDLE = -10;
    }
}

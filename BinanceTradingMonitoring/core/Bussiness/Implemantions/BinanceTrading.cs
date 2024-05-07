using BinanceTradingMonitoring.core.Bussiness.Interfaces;
using BinanceTradingMonitoring.core.Helpers;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace BinanceTradingMonitoring.core.Bussiness.Implemantions
{
    public class BinanceTrade : IBinanceTrading
    {
        public readonly List<string> _selectedPairs = new List<string>();
        // Is used for displaying quantity of trade for test
        // public static int _webSocketResponseCount = 0;
        // public static int _countOfDisplayedTrades = 0;
        public static readonly ConcurrentDictionary<string, List<string>> _tradesData = new ConcurrentDictionary<string, List<string>>();
        // Dictionary to store the trades to display
        public static Stack<string> _tradesToDisplay = new Stack<string>();
        public JsonParser _jsonParser = new JsonParser();
        // Helpers 
        public IApiConnector _apiHelper;
       
        

        public BinanceTrade(IApiConnector apiHelper)
        {
            _apiHelper = apiHelper;
        }
        public void RunTradeMonitor()
        {
            try
            {
                // Select currencies pairs to subscribe
                SelectCurrenciesPairs(DisplayAvailableCurrencies());
                // Subscribe to pairs trades
                SubscribeToTrades();
                // Remove old trades from trades stacks
                Thread cleanupOldTradesThread = new Thread(CleanupOldTrades);
                cleanupOldTradesThread.Start();
                // Display in console trade info from all trades
                Thread displayTradesThread = new Thread(DisplayTrades);
                displayTradesThread.Start();

                while (true)
                {
                    Thread.Sleep(1000);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred during trade monitoring: {e.Message}");
            }
        }



        /// <summary>
        /// Subscribes to WebSocket streams for each selected trading pair.
        /// </summary>
        public void SubscribeToTrades()
        {
            for (int i = 0; i < _selectedPairs.Count; i++)
            {
                string pair = _selectedPairs[i];
                Thread subscribeThread = new Thread(() => SendWebSocketRequest(pair));
                subscribeThread.Start();
            }
        }

        /// <summary>
        /// Retrieves a list of trade pairs from the API.
        /// </summary>
        /// <returns>A list of tuples containing trade pair information.</returns>
        public Dictionary<int, string> GetTradePairs()
        {
            // Get all trade pairs data
            var json = _apiHelper.SendHttpGetRequest("url");
            // Deserialize JSON without using reflection
            return _jsonParser.GetCurrencies(json.ToString());

        }

        /// <summary>
        /// Sends a WebSocket request to the specified URL.
        /// </summary>
        /// <param name="url">The URL to connect to via WebSocket.</param>
        /// <returns>The response from the WebSocket request.</returns>
        public object SendWebSocketRequest(string pair)
        {
            try
            {
   
                using (ClientWebSocket client = new ClientWebSocket())
                {
                    client.ConnectAsync(new Uri(Constant.GetSubscriptionsURL.Replace("{pair}", pair.ToLower())), CancellationToken.None).GetAwaiter().GetResult();
                    while (client.State == WebSocketState.Open)
                    {
                        byte[] buffer = new byte[1024];
                        var result = client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).GetAwaiter().GetResult();
                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            string tradeData = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            AddTrade(pair, tradeData);
                            //Is use for test quantity of displayed trades
                            // BinanceTrade._webSocketResponseCount++;
                        }
                    }
                }
                return new object();
            }
            catch (Exception e)
            {
                Console.WriteLine($"WebSocket request error: {e.Message}");
                throw;
            }
        }

        /// <summary>
        /// Adds trade data to the collection of trades for a specified pair.
        /// </summary>
        /// <param name="pair">The trading pair for which the trade data is being added.</param>
        /// <param name="tradeData">The trade data to be added.</param>
        public  void AddTrade(string pair, string tradeData)
        {
            if (!_tradesData.ContainsKey(pair))
            {
                _tradesData.TryAdd(pair, new List<string>());
            }
            // Add trades in collection of all trades
            lock (_tradesData[pair])
            {
                _tradesData[pair].Add(tradeData);
            }
            // Add trades for display on console
            lock (_tradesToDisplay)
            {
                _tradesToDisplay.Push(tradeData);
            }
        }

        /// <summary>
        /// Periodically removes old trades from the collection of trades for each selected trading pair.
        /// </summary>
        public void CleanupOldTrades()
        {
            while (true)
            {
                for (int i = 0; i < _selectedPairs.Count; i++)
                {
                    string pair = _selectedPairs[i];
                    if (_tradesData.TryGetValue(pair, out var trades))
                    {
                        // Remove old trades if the number of trades exceeds the maximum limit
                        if (trades.Count > Constant.MaxTradesToKeep)
                        {
                            lock (trades)
                            {
                                trades.RemoveRange(0, trades.Count - Constant.MaxTradesToKeep);
                            }
                        }
                    }
                }
                // Sleep for the specified interval before cleaning up again
                Thread.Sleep(Constant.CleanupIntervalSeconds * 1000);
            }
        }

        /// <summary>
        /// Displays the available currencies for trading.
        /// </summary>
        /// <returns>A dictionary representing currency pairs with integer keys and string values.</returns>
        public Dictionary<int, string> DisplayAvailableCurrencies()
        {
            // Get the first 20 currency pairs
            Dictionary<int, string> currencyPairs = GetTradePairs().Where(u => u.Value.Contains("BTC"))
                .Take(20)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            Console.WriteLine("List of trading currency pairs:");
            int[] keys = currencyPairs.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                int key = keys[i];
                string value = currencyPairs[key];
                Console.WriteLine($"Id: {key} {value}");
            }
            return currencyPairs;
        }

        /// <summary>
        /// Allows the user to select currency pairs from the provided list.
        /// </summary>
        /// <param name="currencyPairs">A dictionary representing available currency pairs with integer keys and string values.</param>
        public void SelectCurrenciesPairs(Dictionary<int, string> currencyPairs)
        {
            Console.WriteLine("Enter trading pairs from the previous list via Id (using comma as delimiter): ");
            string selectedPairsInput = Console.ReadLine();
            // Split the input by comma to get individual selected pair IDs
            string[] selectedPairIds = selectedPairsInput.Split(',');

            for (int i = 0; i < selectedPairIds.Length; i++)
            {
                string pairId = selectedPairIds[i];
                if (int.TryParse(pairId, out int id))
                {
                    if (currencyPairs.TryGetValue(id, out string value))
                    {
                        _selectedPairs.Add(value);
                    }
                    else
                    {
                        Console.WriteLine($"Invalid pair ID: {pairId}");
                        SelectCurrenciesPairs(currencyPairs);
                    }
                }
                else
                {
                    Console.WriteLine($"Invalid trading pairs IDs: {pairId}");
                    SelectCurrenciesPairs(currencyPairs);
                }
            }
        }
        /// <summary>
        /// Displays added trades in console.
        /// </summary>
        public void DisplayTrades()
        {
            while (true)
            {
                lock (_tradesToDisplay)
                {

                    if (_tradesToDisplay.Count != 0)
                    {
                        string trade = _tradesToDisplay.Pop();
                        // Console.WriteLine($"Counts of displayed trades: {++BinanceTrade._countOfDisplayedTrades} Counts of requested trades data: {BinanceTrade._webSocketResponseCount}");
                        Console.ForegroundColor = _jsonParser.IsBuyingTrade(trade) ? ConsoleColor.Green : ConsoleColor.Red;
                        // Remove trades from collection for dsiplaying trades

                        Console.WriteLine($" {trade}");
                    }
                }
            }
        }

       
    }
}
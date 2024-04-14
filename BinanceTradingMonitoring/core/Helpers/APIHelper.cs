using BinanceTradingMonitoring.core.Bussiness;
using System.Net.WebSockets;
using System.Text;

namespace BinanceTradingMonitoring.core.Helpers
{/// <summary>
 /// Helper class for interacting with the Binance API.
 /// </summary>
    public class ApiHelper
    {
        /// <summary>
        /// Retrieves the trade pairs from the Binance API.
        /// </summary>
        /// <returns>The JSON response containing trade pairs.</returns>
        public string GetTradePairsAPI()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(Constant.GetCurrenciesPairsURL).Result;
                    response.EnsureSuccessStatusCode();
                    return response.Content.ReadAsStringAsync().Result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"HTTP request error: {e.Message}");
                throw;
            }
        }

        /// <summary>
        /// Subscribes to the WebSocket stream for the specified trading pair.
        /// </summary>
        /// <param name="pair">The trading pair to subscribe to.</param>
        public void SubscribeToPair(string pair)
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
                            BinanceTrade.AddTrade(pair, tradeData);
                            //Is use for test quantity of displayed trades
                            // BinanceTrade._webSocketResponseCount++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"HTTP request error: {e.Message}");
                throw;
            }
        }
    }
}

using BinanceTradingMonitoring.core.Bussiness.Interfaces;
using BinanceTradingMonitoring.core.Helpers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace BinanceTradingMonitoring.core.Bussiness.Implemantions
{
    /// <summary>
    /// Class for interacting with the API.
    /// </summary>
    public class ApiConnector : IApiConnector
    {

        public ConcurrentDictionary<string, ConcurrentBag<string>> _dictionary = new ConcurrentDictionary<string, ConcurrentBag<string>>();
        /// <summary>
        /// Sends an HTTP GET request to the specified URL.
        /// </summary>
        /// <param name="url">The URL to send the request to.</param>
        /// <returns>The response from the HTTP GET request.</returns>

        public string SendHttpGetRequest(string url)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    HttpResponseMessage response = client.GetAsync(Constant.GetCurrenciesPairsURL).Result;
                    response.EnsureSuccessStatusCode();
                     string result =response.Content.ReadAsStringAsync().Result;
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"HTTP request error: {e.Message}");
                throw;
            }
        }


        /// <summary>
        /// Sends a WebSocket request to the specified URL.
        /// </summary>
        /// <param name="url">The URL to connect to via WebSocket.</param>
        /// <returns>The response from the WebSocket request.</returns>
        public void SendWebSocketRequest(string pair)
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
                            AddWebSocketResponse(pair, tradeData);
                            //Is use for test quantity of displayed trades
                            // BinanceTrade._webSocketResponseCount++;
                        }
                    }
                }               
            }
            catch (Exception e)
            {
                Console.WriteLine($"WebSocket request error: {e.Message}");
                throw;
            }
        }
        private void AddWebSocketResponse(string pair, string inputTradesData)
        {
            if (_dictionary.ContainsKey(pair))
            {
                _dictionary[pair].Add(inputTradesData);
            }
            else
            {
                _dictionary.TryAdd(pair,new ConcurrentBag<string> { inputTradesData } );
            }
            
        }
    }
}


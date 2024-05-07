using System.Text.Json;

namespace BinanceTradingMonitoring.core.Helpers
{
    /// <summary>
    /// Provides methods for parsing and manipulating JSON data without reflection.
    /// </summary>
    public class JsonParser
    {
        /// <summary>
        /// Checks if the trade JSON contains the Buying Flag property with the value "true", indicating it's a buying trade.
        /// </summary>
        /// <param name="jsonData">The JSON string representing the trade.</param>
        /// <returns>True if the trade is for buying, false otherwise.</returns>
        public bool IsBuyingTrade(string jsonData)
        {
            using (JsonDocument doc = JsonDocument.Parse(jsonData))
            {
                if (doc.RootElement.TryGetProperty(Constant.BuyingFlag, out JsonElement mElement))
                {
                    return mElement.ValueKind == JsonValueKind.True;
                }
                else
                {
                    throw new Exception($"The JSON does not contain the required 'm' property. {jsonData}");
                }
            }

        }
        /// <summary>
        /// Parses the provided JSON string to retrieve a list of currencies.
        /// </summary>
        /// <param name="json">The JSON string containing currency information.</param>
        /// <returns>A dictionary with integer keys and string values representing currencies.</returns>
        public Dictionary<int, string> GetCurrencies(string json)
        {
            using (JsonDocument doc = JsonDocument.Parse(json))
            {
                JsonElement root = doc.RootElement;
                if (root.TryGetProperty(Constant.Symbols, out JsonElement symbols))
                {
                    Dictionary<int, string> currenciesList = new Dictionary<int, string>();
                    for (int i = 0; i < symbols.GetArrayLength(); i++)
                    {
                        JsonElement symbolElement = symbols[i];
                        // Get currencies
                        string? firstCurrency = symbolElement.GetProperty(Constant.FirstCurrencieProperty).GetString();
                        string? secondCurrency = symbolElement.GetProperty(Constant.SecondCurrencieProperty).GetString();
                        bool isSpotTrading = symbolElement.GetProperty(Constant.IsSpotTrading).GetBoolean();
                        if (firstCurrency != null && secondCurrency != null && isSpotTrading)
                        {
                            currenciesList.Add(i + 1, $"{firstCurrency}{secondCurrency}");
                        }
                    }
                    return currenciesList;
                }
                else
                
                {
                    throw new Exception($"No properties {Constant.Symbols} element found in JSON. {json}");
                }
            }
        }
    }
}

namespace BinanceTradingMonitoring.core.Bussiness.Interfaces
{
    public interface IJsonParser
    {
        /// <summary>
        /// Checks if the trade JSON contains the Buying Flag property with the value "true", indicating it's a buying trade.
        /// </summary>
        /// <param name="jsonData">The JSON string representing the trade.</param>
        /// <returns>True if the trade is for buying, false otherwise.</returns>
        bool IsBuyingTrade(string jsonData);

        /// <summary>
        /// Deserializes the specified JSON string into an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="json">The JSON string to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        T Deserialize<T>(string json);

        /// <summary>
        /// Serializes the specified object into a JSON string.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The JSON string representing the serialized object.</returns>
        string Serialize<T>(T obj);

        /// <summary>
        /// Parses the provided JSON string to retrieve a list of currencies.
        /// </summary>
        /// <param name="json">The JSON string containing currency information.</param>
        /// <returns>A dictionary with integer keys and string values representing currencies.</returns>
        Dictionary<int, string> GetCurrencies(string json);
    }
}

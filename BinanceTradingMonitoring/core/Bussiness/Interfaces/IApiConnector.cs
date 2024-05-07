namespace BinanceTradingMonitoring.core.Bussiness.Interfaces
{
    public interface IApiConnector
    {
        public string SendHttpGetRequest(string url);
        object SendWebSocketRequest(string url);
    }
}

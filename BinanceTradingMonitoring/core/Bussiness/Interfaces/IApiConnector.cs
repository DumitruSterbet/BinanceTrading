namespace BinanceTradingMonitoring.core.Bussiness.Interfaces
{
    public interface IApiConnector
    {
        public string SendHttpGetRequest(string url);
        void SendWebSocketRequest(string url);
    }
}

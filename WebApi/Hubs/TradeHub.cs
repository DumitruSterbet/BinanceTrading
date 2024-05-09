using Microsoft.AspNetCore.SignalR;

namespace WebApi.Hubs
{
       public class TradeHub : Hub
      {
        public async Task SendTradeInfo(string selectedPair, string message)
          {
              // Broadcast the trade info to all connected clients
              await Clients.All.SendAsync("ReceiveTradeInfo", selectedPair, message);
          }
        
}
    
}

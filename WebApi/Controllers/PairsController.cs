using BinanceTradingMonitoring.core.Bussiness.Implemantions;
using BinanceTradingMonitoring.core.Bussiness.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class PairsController : ControllerBase
    {

        private readonly IApiConnector _apiConnector;
        private readonly ILogger<PairsController> _logger;
        private readonly IJsonParser _jsonParser;

        public PairsController(ILogger<PairsController> logger, IApiConnector apiConnector, IJsonParser jsonParser)
        {
            _logger = logger;
            _apiConnector = apiConnector;
            _jsonParser = jsonParser;
        }

        [HttpGet(Name = "GetCurrencyList")]
        public IActionResult GetCurrencyList()
        {
            // Send HTTP GET request to the API endpoint
            string jsonResponse = _apiConnector.SendHttpGetRequest("");

            // Parse the JSON response to extract currency pairs
            Dictionary<int, string> currencyPairs = _jsonParser.GetCurrencies(jsonResponse);

            // Extract currency values from the dictionary
            List<string> currencyValues = new List<string>(currencyPairs.Values);

            return Ok(currencyValues);
        }

        [HttpPost(Name = "SubscribeToTrades")]
        public IActionResult SubsribeToTrades([FromBody] List<string> selectedPairs)
        {
            selectedPairs = new List<string> {  "ETHBTC",
  "LTCBTC",
  "BNBBTC"};
            foreach (var pair in selectedPairs)
            {
                Thread subscribeThread = new Thread(() => _apiConnector.SendWebSocketRequest(pair));
                subscribeThread.Start();
            }

            return Ok();
        }


        [HttpGet(Name = "GetTradesInfo")] // Changed action name to "GetTradesInfo"
        public IActionResult GetTradesInfo()
        {

            return Ok();
        }
    }
}

using BinanceTradingMonitoring.core.Bussiness.Interfaces;
using BinanceTradingMonitoring.core.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PairsController : ControllerBase
    {
       
        private readonly IApiConnector _apiConnector;

        private readonly ILogger<PairsController> _logger;
        private readonly JsonParser _jsonParser= new JsonParser();

        public PairsController(ILogger<PairsController> logger, IApiConnector apiConnector)
        {
            _logger = logger;
            _apiConnector = apiConnector;
        }

        [HttpGet(Name = "GetPairs")]
        public List<string> GetCurrencyList()
        {
            // Send HTTP GET request to the API endpoint
            string jsonResponse = _apiConnector.SendHttpGetRequest("");

            // Parse the JSON response to extract currency pairs
            Dictionary<int, string> currencyPairs = _jsonParser.GetCurrencies(jsonResponse);

            // Extract currency values from the dictionary
            List<string> currencyValues = new List<string>(currencyPairs.Values);

            return currencyValues;
        }
    }
}

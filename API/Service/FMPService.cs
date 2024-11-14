using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApplication1.DTOs.Stock;
using WebApplication1.Interfaces;
using WebApplication1.Mappers;
using WebApplication1.Models;

namespace WebApplication1.Service
{
    public class FMPService : IFMPService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        public FMPService(HttpClient httpClient, IConfiguration config)
        {
            _config = config;
            _httpClient = httpClient;
        }
        public async Task<Stock> FindStockBySymbolAsync(string symbol)
        {
            try {
                var result = await _httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_config["FMPKey"]}");
                if (result.IsSuccessStatusCode) {
                    var content = await result.Content.ReadAsStringAsync();
                    var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
                    var stock = tasks[0];
                    if (stock != null) {
                        return stock.ToStockFromFMP();
                    }
                    return null;
                }
                return null;
            } catch (Exception e) {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}
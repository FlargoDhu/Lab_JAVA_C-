using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StockApp
{
    class StockHistoryConnection
    {
        static string apiKey = "1b6714e500f0cdd864a8b49ec6ac5e45";
        static string apiBaseUrl = "https://api.iextrading.com/1.0/stock/";

        public static async Task<string> LoadDataAsync(string symbol)
        {
            string apiCall = apiBaseUrl + symbol + "/chart";
            Task<string> result;
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(apiCall))
            using (HttpContent content = response.Content)
            {
                result = content.ReadAsStringAsync();
            }
            return await result;
        }
    }
}

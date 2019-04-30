using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace StockAppTests
{
    [TestClass]
    public class StockAppTest
    {
        private Task<string> results;

        class StockTest
        {
            public string Symbol { get; set; }
            public string CompanyName { get; set; }
            public float priceToSales { get; set; }
        }
    



        class StockConnectionTest
        {

            static string apiKey = "1b6714e500f0cdd864a8b49ec6ac5e45";
            static string apiBaseUrl = "https://api.iextrading.com/1.0/stock/";

            public static async Task<string> LoadDataAsync(string symbol)
            {
                string apiCall = apiBaseUrl + symbol + "/stats";
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

        class StockReaderTest
        {
            public static StockTest Read(string data)
            {

                StockTest stock = JsonConvert.DeserializeObject<StockTest>(data);
                return stock;
            }

        }
                



        [TestMethod]
        public async Task test1()
        {
            results = StockConnectionTest.LoadDataAsync("F");
            Assert.IsNotNull(results);

                     
        }

        [TestMethod]
        public async Task test2()
        {
            string data = await StockConnectionTest.LoadDataAsync("F");
            StockTest stock = StockReaderTest.Read(data);
            Assert.IsNotNull(stock);

        }
                          
    }
}

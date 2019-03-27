using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Lab01
{
    class WeatherConnection
    {
        static string apiKey = "1b6714e500f0cdd864a8b49ec6ac5e45";
        static string apiBaseUrl = "https://api.openweathermap.org/data/2.5/weather";

        public static async Task<string> LoadDataAsync(string cityName)
        {
            string apiCall = apiBaseUrl + "?q=" + cityName + "&apikey=" + apiKey + "&mode=xml";
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

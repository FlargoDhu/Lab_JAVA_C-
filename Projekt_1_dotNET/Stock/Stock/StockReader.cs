
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp
{
    class StockReader
    {
        public static Stock Read(string data)
        {

            Stock stock = JsonConvert.DeserializeObject<Stock>(data);
            return stock;
        }
        
    }
}

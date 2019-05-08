using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp
{
    class StockHistoryReader
    {
        public static List<StockHistory> Read(string data)
        {

            List<StockHistory> history = JsonConvert.DeserializeObject<List<StockHistory>>(data);
            return history;
        }
    }
}

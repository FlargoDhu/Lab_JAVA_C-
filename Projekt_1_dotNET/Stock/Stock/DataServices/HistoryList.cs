using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.DataServices
{
    class HistoryList
    {
        private DatabaseEntities1 conn;
        private List<History> history;
        public HistoryList(string symbol)
        {
            conn = new DatabaseEntities1();
            var id = conn.Symbol.Where(x => x.Name == symbol).FirstOrDefault().Id;
            history = conn.History.Where(x => x.SymbolId == id).ToList();
        }
        public List<History> GetList()
        {

            return history;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.DataServices
{
    class SymbolList
    {
        private DatabaseEntities1 conn;
        private List<Symbol> symbols;
        public SymbolList()
        {
            conn = new DatabaseEntities1();
            symbols = conn.Symbol.ToList<Symbol>();
        }
        public List<Symbol> GetList()
        {

            return symbols;
        }
    }
}

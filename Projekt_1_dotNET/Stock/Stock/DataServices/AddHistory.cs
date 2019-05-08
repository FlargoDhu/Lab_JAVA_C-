using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace StockApp
{
    class AddHistory
    {
        public string Symbol { get; set; }

        public AddHistory(string symbol)
        {
            var conn = new DatabaseEntities1();
            Symbol = symbol;
            if (conn.Symbol.Where(x => x.Name == Symbol).Count() <= 0)
                MessageBox.Show("First add Symbol to database");

        }
        public void Add(List<StockHistory> history)
        {
            var conn = new DatabaseEntities1();
            var id = conn.Symbol.Where(x => x.Name == Symbol).FirstOrDefault().Id;
            foreach (var item in history)
            {
               
                    if (conn.History.Where(x => x.SymbolId == id && x.Date == item.Date).Count() > 0)
                        continue;

                    conn.History.Add(new History()
                    {
                        SymbolId = id,
                        Price = item.Close,
                        Date = item.Date
                    });
               
            }
            conn.SaveChanges();

        }
    }
}

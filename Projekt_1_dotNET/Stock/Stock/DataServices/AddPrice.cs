using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.DataServices
{
    class AddPrice
    {
        bool wasNew = false;
        public Symbol item;
        public AddPrice(Stock stock)
        {
            var conn = new DatabaseEntities1();
            
            var c = conn.Symbol.Where(x => x.Name == stock.Symbol).Count();
            if (c == 0)
            {

                conn.Symbol.Add(new Symbol() {
                    Name = stock.Symbol,
                    Price = stock.priceToSales,
                    Company = stock.CompanyName
                });
                conn.SaveChanges();

                item = conn.Symbol.Where(x => x.Name == stock.Symbol).FirstOrDefault();
                wasNew = true;
            }

        }
        public bool New()
        {
            return wasNew;
        }
    }
}

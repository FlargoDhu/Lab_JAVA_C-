using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab01
{
    static class ParseWeather_LINQ
    {
        public static WeatherDataEntry Parse(System.IO.Stream stream)
        {
            XElement xml = XElement.Load(stream);
            var nameQuery = (from element in xml.Elements()
                         let elementName = element.Name
                         where(elementName == "city")
                         select new 
                         {
                             City = element.Attributes("name").FirstOrDefault(), 
                         });
            var temperatureQuery = (from element in xml.Elements()
                             let elementName = element.Name
                             where (elementName == "temperature")
                             select new
                             {
                                 Temperature = element.Attributes("value").FirstOrDefault(),
                             });
            return new WeatherDataEntry()
            {
                City = nameQuery.FirstOrDefault().City.Value,
                Temperature = float.Parse(
                    temperatureQuery.FirstOrDefault().Temperature.Value, 
                    System.Globalization.CultureInfo.InvariantCulture)
            };
        }
    }
}

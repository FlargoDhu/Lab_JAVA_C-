using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lab01
{
    public class ParseWeather_XmlReader
    {
        public static WeatherDataEntry Parse(System.IO.Stream stream)
        {
            XmlTextReader reader = new XmlTextReader(stream);
            WeatherDataEntry result = new WeatherDataEntry()
            {
                City = string.Empty,
                Temperature = float.NaN
            };

            while (reader.Read())
            { 
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        switch (reader.Name)
                        {
                            case "city":
                                result.City = reader.GetAttribute("name");
                                break;
                            case "temperature":
                                result.Temperature = 
                                    float.Parse(
                                        reader.GetAttribute("value"),
                                        System.Globalization.CultureInfo.InvariantCulture);
                                break;
                        }
                        break;
                }
            }

            return result;
        }
    }
}

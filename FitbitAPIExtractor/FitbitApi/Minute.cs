using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FitbitAPIExtractor.FitbitApi
{
    class Minute
    {
        public string When { get; set; }
        public int Steps { get; set; }
        public int Heartrate { get; set; }
        public int Distance { get; set; }
        public int Elevation { get; set; }
        public int Calories { get; set; }
        public int Floors { get; set; }

        public Minute()
        {
            When = "00:00:00";
            Steps = 0;
            Heartrate = 0;
            Calories = 0;
        }

        public XElement SerializeToXml()
        {
            XElement x = new XElement("minute");

            x.Add(new XAttribute("time", When));
            x.Add(new XAttribute("steps", Steps));
            x.Add(new XAttribute("heartrate", Heartrate));
            x.Add(new XAttribute("distance", Distance));
            x.Add(new XAttribute("elevation", Elevation));
            x.Add(new XAttribute("calories", Calories));
            x.Add(new XAttribute("floors", Floors));

            return x;
        }
    }
}

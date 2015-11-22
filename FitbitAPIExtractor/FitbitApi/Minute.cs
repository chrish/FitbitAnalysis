using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitbitAPIExtractor.FitbitApi
{
    class Minute
    {
        public string When { get; set; }
        public int Steps { get; set; }
        public int Heartrate { get; set; }
        public int Calories { get; set; }

        public Minute()
        {
            When = "00:00:00";
            Steps = 0;
            Heartrate = 0;
            Calories = 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace FitbitAPIExtractor.FitbitApi
{
    /// <summary>
    /// Will accept a dataset for one day per activity, and can generate a list of minutes 
    /// containing all activity. 
    /// </summary>
    class FitbitDataAggregator
    {
        public JObject StepsJson { get; set; }
        public JObject CaloriesJson { get; set; }
        public JObject FloorsJson { get; set; }
        public JObject PulseJson { get; set; }
        public JObject DistanceJson { get; set; }

        public List<Minute> ParseData()
        {
            Dictionary<string, Minute> minutes = new Dictionary<string, Minute>();

            for (int hour = 0; hour < 24; hour++)
            {
                for (int min = 0; min < 60; min++)
                {
                    string timestamp = hour.ToString("D2") + ":" + min.ToString("D2") + ":00";
                    Console.WriteLine("Parsing " + timestamp);
                    Minute m = ParseMinute(timestamp);

                    minutes.Add(timestamp, m);
                }
            }

            return minutes.Values.ToList();
        }
            
        /// <summary>
        /// Parses all the data into minute objects containing all stats for a given 
        /// minute. Will loop over steps using that timestamp to extract data from the 
        /// other datasets to avoid multiple loops.
        /// </summary>
        /// <param name="timestamp"></param>
        /// <returns></returns>
        protected Minute ParseMinute(string timestamp)
        {
            Minute m = new Minute();

            for (int i = 0; i < StepsJson["activities-steps-intraday"]["dataset"].Count(); i++)
            {
                string jsonTimestamp = StepsJson["activities-steps-intraday"]["dataset"][i]["time"].Value<string>();
                if (jsonTimestamp.Equals(timestamp))
                {
                    m.When = timestamp;

                    int steps = Convert.ToInt32(StepsJson["activities-steps-intraday"]["dataset"][i]["value"]);
                    m.Steps = steps;

                    // Steps will always exist, and be 0 if no steps have been registered. 
                    // Other data such as HR can be nonexistant if nothing has been registered
                    int floors = 0;

                    if (FloorsJson["activities-floors-intraday"]["dataset"].Count() > i)
                    {
                        floors = Convert.ToInt32(FloorsJson["activities-floors-intraday"]["dataset"][i]["value"]);
                    }
                    m.Floors = floors;

                    int distance = 0;
                    if (DistanceJson["activities-distance-intraday"]["dataset"].Count() > i)
                    {
                        distance = Convert.ToInt32(DistanceJson["activities-distance-intraday"]["dataset"][i]["value"]);
                    }
                    m.Distance = distance;

                    int calories = 0;
                    if (CaloriesJson["activities-calories-intraday"]["dataset"].Count() > i)
                    {
                        calories = Convert.ToInt32(CaloriesJson["activities-calories-intraday"]["dataset"][i]["value"]);
                    }
                    m.Calories = calories;
                    

                    int heart = 0;
                    if (PulseJson["activities-heart-intraday"]["dataset"].Count() > i)
                    {
                        heart = Convert.ToInt32(PulseJson["activities-heart-intraday"]["dataset"][i]["value"]);
                    }    
                    m.Heartrate = heart;
                        
                    
                    break;
                }    
            }
            
            return m;
        }

    }
}

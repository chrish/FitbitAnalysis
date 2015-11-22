using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace FitbitAPIExtractor.FitbitApi
{
    class Fitbit
    {
        public static readonly string ACT_CAL = "activities/calories";
        public static readonly string ACT_STEPS = "activities/steps";
        public static readonly string ACT_DIST = "activities/distance";
        public static readonly string ACT_FLRS = "activities/floors";
        public static readonly string ACT_HR = "activities/heart";

        public static readonly string INT_1MIN = "1min";
        public static readonly string INT_15MIN = "15min";

        public static readonly string FITBIT_ENDPOINT = "https://api.fitbit.com/1/user/";

        public static readonly string DEFAULT_RESPONSEFORMAT = ".json";

        public Dictionary<DateTime, List<Minute>> RetrievedData;

        protected string Token;

        public Fitbit(string token)
        {
            RetrievedData = new Dictionary<DateTime, List<Minute>>();
            Token = token;
        }
        

        public void GetData()
        {
            GetData(Program.StartDate, Program.EndDate);
        }

        public void GetData(DateTime from, DateTime to)
        {
            DateTime iteratorDate = from;
            while (iteratorDate <= to)
            {
                FitbitDataRequest stepsRequest = new FitbitDataRequest(Token);
                KeyValuePair<string, string> step = stepsRequest.GetData(iteratorDate, Fitbit.ACT_STEPS);

                FitbitDataRequest caloriesRequest = new FitbitDataRequest(Token);
                KeyValuePair<string, string> cal = caloriesRequest.GetData(iteratorDate, Fitbit.ACT_CAL);

                FitbitDataRequest distRequest = new FitbitDataRequest(Token);
                KeyValuePair<string, string> dist = distRequest.GetData(iteratorDate, Fitbit.ACT_DIST);

                FitbitDataRequest pulseRequest = new FitbitDataRequest(Token);
                KeyValuePair<string, string> hr = pulseRequest.GetData(iteratorDate, Fitbit.ACT_HR);

                FitbitDataRequest floorsRequest = new FitbitDataRequest(Token);
                KeyValuePair<string, string> floor = floorsRequest.GetData(iteratorDate, Fitbit.ACT_FLRS);
                
                FitbitDataAggregator fbda = new FitbitDataAggregator();
                
                fbda.StepsJson = JObject.Parse(step.Value);
                fbda.PulseJson = JObject.Parse(hr.Value);
                fbda.CaloriesJson = JObject.Parse(cal.Value);
                fbda.DistanceJson = JObject.Parse(dist.Value);
                fbda.FloorsJson = JObject.Parse(floor.Value);

                List<Minute> parsedData = fbda.ParseData();
                RetrievedData.Add(iteratorDate, parsedData);

                iteratorDate  = iteratorDate.AddDays(1);
            }
        }

        public void SerializeDataToXml(string file)
        {
            XElement root = new XElement("fitbitSerializedData");

            foreach (KeyValuePair<DateTime, List<Minute>> p in RetrievedData)
            {
                XElement day = new XElement("day");
                day.Add(new XAttribute("date", p.Key.ToString("yyyy-MM-dd")));

                foreach (Minute m in p.Value)
                {
                    day.Add(m.SerializeToXml());
                }

                root.Add(day);
            }

            XDocument x = new XDocument(root);

            x.Save(file);
        }
    }
}

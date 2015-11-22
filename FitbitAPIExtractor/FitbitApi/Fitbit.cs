using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace FitbitAPIExtractor.FitbitApi
{
    class Fitbit
    {
        public static readonly string ACT_CAL = "activities/calories";
        public static readonly string ACT_STEPS = "activities/steps";
        public static readonly string ACT_DIST = "activities/distance";
        public static readonly string ACT_FLRS = "activities/floors";
        public static readonly string ACT_ELEV = "activities/elevation";

        public static readonly string INT_1MIN = "1min";
        public static readonly string INT_15MIN = "15min";

        public static readonly string FITBIT_ENDPOINT = "https://api.fitbit.com/1/user/";

        public static readonly string DEFAULT_RESPONSEFORMAT = ".json";

        protected string Token;

        public Fitbit(string token)
        {
            Token = token;
        }


        public List<Minute> GetData()
        {
            return GetData(Program.StartDate, Program.EndDate);
        }

        public List<Minute> GetData(DateTime from, DateTime to)
        {
            DateTime iteratorDate = from;
            while (iteratorDate <= to)
            {
                FitbitDataRequest stepsRequest = new FitbitDataRequest(Token);
                KeyValuePair<string, string> step = stepsRequest.GetData(iteratorDate, Fitbit.ACT_STEPS);

                /*FitbitDataRequest caloriesRequest = new FitbitDataRequest(Token);
                KeyValuePair<string, string> cal = caloriesRequest.GetData(iteratorDate, Fitbit.ACT_CAL);

                FitbitDataRequest distRequest = new FitbitDataRequest(Token);
                KeyValuePair<string, string> dist = distRequest.GetData(iteratorDate, Fitbit.ACT_DIST);

                FitbitDataRequest elevRequest = new FitbitDataRequest(Token);
                KeyValuePair<string, string> elev = elevRequest.GetData(iteratorDate, Fitbit.ACT_ELEV);

                FitbitDataRequest floorsRequest = new FitbitDataRequest(Token);
                KeyValuePair<string, string> floor = floorsRequest.GetData(iteratorDate, Fitbit.ACT_FLRS);
                */
                FitbitDataAggregator fbda = new FitbitDataAggregator();
                
                fbda.StepsJson = JObject.Parse(step.Value);
                fbda.ParseData();

                iteratorDate  = iteratorDate.AddDays(1);
            }

            return new List<Minute>();
        } 
    }
}

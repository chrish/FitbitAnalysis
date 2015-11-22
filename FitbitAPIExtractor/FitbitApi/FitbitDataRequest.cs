using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using FitbitAPIExtractor.Auth;

namespace FitbitAPIExtractor.FitbitApi
{
    /// <summary>
    /// Get stats for one day only. Call again for next day, etc. 
    /// 
    /// https://api.fitbit.com/1/user/-/activities/steps/date/2015-11-04/2015-11-04/1min.xml
    /// 
    /// @todo: Errorhandling
    /// </summary>
    class FitbitDataRequest
    {
        public string Format { get; protected set; }
        public string User { get; set; }
        public string Interval { get; protected set; }

        protected string ResourcePath;
        protected DateTime Date;

        protected string AuthToken;
        
        /// <summary>
        /// Configures the request. 
        /// </summary>
        /// <param name="resourcePath"></param>
        /// <param name="date"></param>
        /// <param name="authToken"></param>
        public FitbitDataRequest(string authToken)
        {
            Format = Fitbit.DEFAULT_RESPONSEFORMAT;
            User = "-";
            Interval = Fitbit.INT_1MIN;
            AuthToken = authToken;
        }

        /// <summary>
        /// Builds the URI used for the request. 
        /// </summary>
        /// <returns></returns>
        protected Uri GetRequestUri()
        {
            string date = Date.ToString("yyyy-MM-dd");
            string uri = Fitbit.FITBIT_ENDPOINT + User + "/" + ResourcePath 
                + "/date/" + date + "/" + date + "/" + Interval + Format;

            return new Uri(uri);
        }


        /// <summary>
        /// Gets all the various datas for a given day, creating a list of minutes 
        /// adding the data into each.
        /// </summary>
        /// <param name="When"></param>
        /// <returns></returns>
        public KeyValuePair<string, string> GetData(DateTime when, string resourcePath)
        {
            Date = when;
            ResourcePath = resourcePath;
            Uri uri = GetRequestUri();

            KeyValuePair<string, string> header = new KeyValuePair<string, string>("Authorization", "Bearer " + AuthToken);
            List<KeyValuePair<string, string>> headerList = new List<KeyValuePair<string, string>>();
            headerList.Add(header);

            SimpleWebRequest rq = new SimpleWebRequest(uri, headerList);

            KeyValuePair<string, string> requestData = rq.MakeRequest();

            return requestData;
        }  

    }
}

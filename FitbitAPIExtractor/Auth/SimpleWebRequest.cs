using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace FitbitAPIExtractor.Auth
{
    public class SimpleWebRequest
    {
        Uri RequestUri;
        List<KeyValuePair<string, string>> Headers;
        string Method;
        string Data;
        string ContentType = "application/x-www-form-urlencoded";

        public SimpleWebRequest(Uri uri, List<KeyValuePair<string, string>> headers)
        {
            Headers = headers;
            RequestUri = uri;
            Method = "GET";
            Data = "";
        }

        public SimpleWebRequest(Uri uri, List<KeyValuePair<string, string>> headers, string data)
        {
            Headers = headers;
            RequestUri = uri;
            Method = "POST";
            Data = data;
        }

        /// <summary>
        /// Executes the request, passing the response back to the caller for handling. 
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public KeyValuePair<string, string> MakeRequest()
        {
            Console.WriteLine("Requesting " + RequestUri);
            HttpWebRequest Request = (HttpWebRequest) WebRequest.Create(RequestUri);

            Request.Method = Method;
            //Request.ContentType = ContentType;

            foreach (KeyValuePair<string, string> kv in Headers)
            {
                Request.Headers.Add(kv.Key, kv.Value);
            }

            // Send data when POSTing.
            if (Method.Equals("POST"))
            {
                Request.ContentType = ContentType;
                Request.ContentLength = Encoding.UTF8.GetBytes(Data).Length;

                Stream requestStream = Request.GetRequestStream();
                requestStream.Write(Encoding.UTF8.GetBytes(Data), 0, Encoding.UTF8.GetBytes(Data).Length);
                requestStream.Close();
            }

            HttpWebResponse response = (HttpWebResponse) Request.GetResponse();

            return GetDataFromResponse(response);
        }

        /// <summary>
        /// Will return a key/value containing mimetype and response data as plaintext.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        protected KeyValuePair<string, string> GetDataFromResponse(WebResponse response)
        {
            long contentLength = response.ContentLength;
            string contentType = response.ContentType;
            string responseText = "";

            if (contentType.Contains("application/json"))
            {
                using (var s = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseText = s.ReadToEnd();
                }
            }

            return new KeyValuePair<string, string>(contentType, responseText);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using FitbitAPIExtractor.Auth;
using FitbitAPIExtractor.FitbitApi;

namespace FitbitAPIExtractor
{
    class Program
    {
        public static DateTime StartDate = new DateTime(2015, 11, 04);
        public static DateTime EndDate = DateTime.Today;
        public static readonly string ClientId = "";
        public static readonly string Secret = "";

        static void Main(string[] args)
        {
            new Program();
        }

        


        public Program()
        {
            string[] s = new string[1];

            Authentication auth = new Authentication("http://localhost:8989/Fitbit/");
            string token = auth.InitializeAuthenticationRequest();

            Console.WriteLine("Authentication completed; token is: " + token);
            

            Fitbit fb = new Fitbit(token);
            fb.GetData(Program.StartDate, Program.EndDate);

            // Now we're authenticated. Let's get some data...



            // Then make one request per day for the profile. Skip days with no activity


        }


    }
}

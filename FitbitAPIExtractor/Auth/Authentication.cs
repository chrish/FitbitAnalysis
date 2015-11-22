using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace FitbitAPIExtractor.Auth
{
    /// <summary>
    /// Class handling all authentication against Fitbit.com. Will open a browser 
    /// asking the user to log in and authorize API access; SimpleWebServer will 
    /// listen for a callback where we can grab the auth token. 
    /// 
    /// Callback example:
    /// http://localhost:8989/Fitbit/#scope=nutrition+weight+location+social+heartrate+settings+sleep+activity+profile
    /// &user_id=3TMF35
    /// &token_type=Bearer
    /// &expires_in=86400
    /// &access_token=eyJhbGciOiJIUzI1NiJ9.eyJleHAiOjE0NDgwMzgyMTcsInNjb3BlcyI6InJ3ZWkgcnBybyByaHIgcmxvYyBybnV0IHJzbGUgcnNldCByYWN0IHJzb2MiLCJzdWIiOiIzVE1GMzUiLCJhdWQiOiIyMkIyU1ciLCJpc3MiOiJGaXRiaXQiLCJ0eXAiOiJhY2Nlc3NfdG9rZW4iLCJpYXQiOjE0NDc5NTE4MTd9.neNUdj2GRlKfZkxAA2D8JzfUblcQokB4UER7888bnD4
    /// </summary>
    class Authentication
    {
        public string FitbitToken { get; protected set; }
        protected string FitbitOAuthUrlBase = "https://www.fitbit.com/oauth2/";

        protected string ClientId;
        protected string ResponseType;
        protected string RedirectUri;

        public string CompleteRedirectUri { get; protected set; }

        List<string> Scopes;

        /// <summary>
        /// Set up the authentication
        /// </summary>
        /// <param name="redirectUri"></param>
        public Authentication(string redirectUri)
        {
            Scopes = new List<string>(){"activity", "heartrate", "location", "nutrition", "profile", "settings", "sleep", "social", "weight"};
            ClientId = Program.ClientId;
            ResponseType = "token";
            RedirectUri = HttpUtility.UrlEncode(redirectUri);

            CompleteRedirectUri = GetFullOAuthUri();
        }

        /// <summary>
        /// Generate the full URI to authorize against
        /// </summary>
        /// <returns></returns>
        protected string GetFullOAuthUri()
        {
            string FitbitOAuthUrl = FitbitOAuthUrlBase + "authorize";
            if (!FitbitOAuthUrl.EndsWith("?"))
            {
                FitbitOAuthUrl += "?";
            }
            
            
            FitbitOAuthUrl += "response_type=" + ResponseType;
            FitbitOAuthUrl += "&redirect_uri=" + RedirectUri;
            FitbitOAuthUrl += "&client_id=" + ClientId;
            FitbitOAuthUrl += "&scope=" + HttpUtility.UrlEncode(string.Join(" ", Scopes));



            return FitbitOAuthUrl;
        }

        /// <summary>
        /// Fire up a browser, pointing towards the auth URI. Then start the 
        /// HTTPListener to check for a response from Fitbit. 
        /// 
        /// We don't care about the contents of the response, only the URI returned
        /// as this will give us our token. 
        /// </summary>
        public string InitializeAuthenticationRequest()
        {
            System.Diagnostics.Process.Start(CompleteRedirectUri);

            SimpleWebServer swebServer = new SimpleWebServer();
            Uri fitBitCallbackUri = swebServer.StartListener();

            string queryString = fitBitCallbackUri.ToString().Split('?')[1];

            var queryParams = HttpUtility.ParseQueryString(HttpUtility.UrlDecode(queryString));

            string authCode = queryParams["access_token"];

            return authCode;
        }

    }
}

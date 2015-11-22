using System;
using System.Net;

namespace FitbitAPIExtractor.Auth
{
    /// <summary>
    /// Very basic httplistener that waits until a request containing a questionmark
    /// appear. 
    /// 
    /// Fitbit uses a callback with a hashmark as a separator for url parameters, 
    /// so a javascript snippet is injected into the callback response in order to 
    /// replace the hash with a questionmark, and then redirect the page to the 
    /// modified callback url. 
    /// </summary>
    public class SimpleWebServer
    {
        protected HttpListener _listener;

        protected string ListenerPrefix = "http://localhost:8989/Fitbit/";

        public SimpleWebServer()
        {
            _listener = new HttpListener();

            _listener.Prefixes.Add(ListenerPrefix);
        }

        /// <summary>
        /// Opens the HTTP-listener, waiting for something to be requested.
        /// </summary>
        /// <returns>Requested URL</returns>
        public Uri StartListener()
        {
            _listener.Start();

            string requestUri = "";

            while (!requestUri.Contains("?"))
            {
                HttpListenerContext context = _listener.GetContext();
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                string url = request.Url.AbsoluteUri;

                string redirectScript = "";
                string onloadFunc = "";

                if (url.Contains("?"))
                {
                    requestUri = url;

                }
                else
                {
                    onloadFunc = "onload=\"redir();\"";
                    redirectScript = @"<script type='text/javascript' language='javascript'>
                        function redir(){ 
                        var loc = window.location.href;

                        if (loc.indexOf('#') > 0){
                            window.location = loc.replace('#', '?');
                        } 
                    }
                    </script>";
                }

                string responseString = "<!DOCTYPE html>" + Environment.NewLine +
                                            "<html><title>Fitbitredirector</title><body "+ onloadFunc +">" +
                                            redirectScript.Replace("'", "\"") + "Hello world!</body></html>";
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                // Get a response stream and write the response to it.
                response.ContentLength64 = buffer.Length;
                System.IO.Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                // You must close the output stream.
                output.Close();
            }

            _listener.Stop();

            Uri uri = new Uri(requestUri);

            return uri;
        }
    }

}

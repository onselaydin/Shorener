using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Shorener
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri webAddress = new Uri("https://developers.google.com/url-shortener/v1/getting_started");
            var credential = new NetworkCredential("", "", ""); //Your credential
            Console.WriteLine("Source\n{0}\nShorten\n{1}"
                , webAddress.ToString()
                , webAddress.GetShorten(proxtCredential: credential));
        }
    }

    public static class UriExtensions
    {
        public static string GetShorten(this Uri source
            ,NetworkCredential proxtCredential=null)
        {
            string shortenUrl = String.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest
                .Create("https://www.googleapis.com/urlshortener/v1/url");
            if (proxtCredential != null)
                request.Proxy.Credentials = proxtCredential;
            request.ContentType = "application/json";
            request.Method = "POST";

            using (var writer = new StreamWriter(request.GetRequestStream()))
            {
                writer.Write("{\"longUrl\":\""+source.AbsoluteUri+"\"}");
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                dynamic jo = JObject.Parse(reader.ReadToEnd());
                shortenUrl = jo.id;
            }
            response.Close();
            return shortenUrl;
        }
    }
}

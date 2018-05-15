
using System.IO;
using System;
using System.Net;

namespace IceCreamRatingsAPI
{
    public class Connect
    {
        public string id;
        public string type;

        protected string api = "https://serverlessohlondonproduct.azurewebsites.net/api/GetProduct/";
        protected string options = "?productid=4c25613a-a3c2-4ef3-8e02-9c335eb23204";

        public string request()
        {
            string totalUrl = this.join(id);

            return this.HttpGet(totalUrl);
        }

        protected string join(string s)
        {
            return api + type + "/" + s + options;
        }

        protected string get(string url)
        {
            try
            {
                string rt;

                WebRequest request = WebRequest.Create(url);

                WebResponse response = request.GetResponse();

                Stream dataStream = response.GetResponseStream();

                StreamReader reader = new StreamReader(dataStream);

                rt = reader.ReadToEnd();

                Console.WriteLine(rt);

                reader.Close();
                response.Close();

                return rt;
            }

            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
        public string HttpGet(string URI)
        {
            WebClient client = new WebClient();

            // Add a user agent header in case the 
            // requested URI contains a query.

            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");

            Stream data = client.OpenRead(URI);
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            data.Close();
            reader.Close();

            return s;
        }
    }
}

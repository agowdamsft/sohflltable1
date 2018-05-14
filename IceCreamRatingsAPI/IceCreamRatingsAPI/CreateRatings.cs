
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Net;

namespace IceCreamRatingsAPI
{
    public static class CreateRatings
    {
        [FunctionName("CreateRatings")]
        public static async System.Threading.Tasks.Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            var x  = new Connect();

            var rating = new Rating();
            rating.userid = data?.userid;
            rating.productid = data?.productid;
            rating.timestamp = data?.timestamp;
            rating.locationname = data?.locationname;
            rating.rating = data?.rating;
            rating.usernotes = data?.usernotes;
            rating.id = Guid.NewGuid().ToString();

            var uri =  $"https://serverlessohlondonproduct.azurewebsites.net/api/GetProduct/?productid=" + rating.productid;

            try
            {
                dynamic productresult = JsonConvert.DeserializeObject(x.HttpGet(uri));
            }
            catch (Exception)
            {

                return new BadRequestObjectResult("Please pass a valid productid on the query string or in the request body");
            }

            uri = $"https://serverlessohlondonuser.azurewebsites.net/api/GetUser/?userid=" + rating.userid;

            try
            {
                dynamic productresult = JsonConvert.DeserializeObject(x.HttpGet(uri));
            }
            catch (Exception)
            {

                return new BadRequestObjectResult("Please pass a valid userid on the query string or in the request body");
            }

            var dbRepo = new DocumentDBRepository<Rating>();

            await dbRepo.CreateItemAsync(rating);


            return rating.userid != null
                ? (ActionResult)new OkObjectResult(rating)
                : new BadRequestObjectResult("Please pass a userID on the query string or in the request body");
        }
    }

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

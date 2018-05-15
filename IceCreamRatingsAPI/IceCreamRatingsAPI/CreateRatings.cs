
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Configuration;
using Microsoft.Extensions.Configuration;

namespace IceCreamRatingsAPI
{
    public static class CreateRatings
    {
        [FunctionName("CreateRatings")]
        public static async System.Threading.Tasks.Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info($"C# HTTP trigger function CreateRatings Started: {DateTime.Now}");

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            var x  = new Connect();

            var rating = new Rating
            {
                userid = data?.userid,
                productid = data?.productid,
                timestamp = data?.timestamp,
                locationname = data?.locationname,
                rating = data?.rating,
                usernotes = data?.usernotes,
                id = Guid.NewGuid().ToString(),
            };

            log.Info($"C# HTTP trigger function CreateRatings Check Product ID: {DateTime.Now}");

            var uri =  $"https://serverlessohlondonproduct.azurewebsites.net/api/GetProduct/?productid=" + rating.productid;

            try
            {
                dynamic productresult = JsonConvert.DeserializeObject(x.HttpGet(uri));
            }
            catch (Exception)
            {

                return new BadRequestObjectResult("Please pass a valid productid on the query string or in the request body");
            }

            log.Info($"C# HTTP trigger function CreateRatings Check Product ID Ended: {DateTime.Now}");

            log.Info($"C# HTTP trigger function CreateRatings Check User ID Started: {DateTime.Now}");

            uri = $"https://serverlessohlondonuser.azurewebsites.net/api/GetUser/?userid=" + rating.userid;

            try
            {
                dynamic productresult = JsonConvert.DeserializeObject(x.HttpGet(uri));
            }
            catch (Exception)
            {

                return new BadRequestObjectResult("Please pass a valid userid on the query string or in the request body");
            }

            log.Info($"C# HTTP trigger function CreateRatings Check User ID Completed: {DateTime.Now}");

            log.Info($"C# HTTP trigger function CreateRatings Save Called: {DateTime.Now}");
            var dbRepo = new DocumentDBRepository<Rating>();

            await dbRepo.CreateItemAsync(rating);

            log.Info($"C# HTTP trigger function CreateRatings Save Completed: {DateTime.Now}");

            return rating.userid != null
                ? (ActionResult)new OkObjectResult(rating)
                : new BadRequestObjectResult("Please pass a userID on the query string or in the request body");
        }
    }
}

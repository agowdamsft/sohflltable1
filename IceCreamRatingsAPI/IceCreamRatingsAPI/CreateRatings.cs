
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace IceCreamRatingsAPI
{
    public static class CreateRatings
    {
        [FunctionName("CreateRatings")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            var rating = new Rating();
            rating.userid = data?.userid;
            rating.productid = data?.productid;
            rating.timestamp = data?.timestamp;
            rating.locationname = data?.locationname;
            rating.rating = data?.rating;
            rating.usernotes = data?.usernotes;

            var dbRepo = new DocumentDBRepository<Rating>();

            var result = dbRepo.CreateItemAsync(rating);


            return rating.userid != null
                ? (ActionResult)new OkObjectResult($"Hello, {result}")
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}

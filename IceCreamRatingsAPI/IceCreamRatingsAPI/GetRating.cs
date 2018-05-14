
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;

namespace IceCreamRatingsAPI
{
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public async static Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
        
            string ratingid = req.Query["ratingid"];

            string requestBody = new StreamReader(req.Body).ReadToEnd();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //ratingid = ratingid ?? data?.name;

            var dbRepo = new DocumentDBRepository<Rating>();

           // iRating rating = new iRating();

           //var rating = await dbRepo.GetItemAsync(ratingid);
            
           var result = await dbRepo.GetItemsAsync(d => d.id==ratingid);



            return ratingid != null
                ? (ActionResult)new OkObjectResult(result.FirstOrDefault())
                : new BadRequestObjectResult("Please pass a name on the query string or in the request body");
        }
    }
}

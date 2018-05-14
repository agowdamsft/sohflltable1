
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace IceCreamRatingsAPI
{
    
    public static class GetRatingsByUser
    {
        [FunctionName("GetRatingsByUser")]
        public async static Task<IEnumerable<Rating>> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/{userId}")]HttpRequest req, string userId, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");

            var ratingsRepo = new DocumentDBRepository<Rating>();

            var ratingsByUser = await ratingsRepo.GetItemsAsync((r) => r.userid == userId);

            return ratingsByUser;
        }
    }
}

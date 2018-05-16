
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
using RestSharp;
using System.Linq;
using System.Collections.Generic;

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

            var connect = new Connect();

            var rating = new Rating
            {
                userid = data?.userid,
                productid = data?.productid,
                timestamp = data?.timestamp,
                locationname = data?.locationname,
                rating = data?.rating,
                usernotes = data?.usernotes,
                id = Guid.NewGuid().ToString()
            };

            log.Info($"C# HTTP trigger function CreateRatings Check Product ID: {DateTime.Now}");

            var uri = $"https://serverlessohlondonproduct.azurewebsites.net/api/GetProduct/?productid=" + rating.productid;

            try
            {
                dynamic productresult = JsonConvert.DeserializeObject(connect.HttpGet(uri));
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
                dynamic productresult = JsonConvert.DeserializeObject(connect.HttpGet(uri));
            }
            catch (Exception)
            {
                return new BadRequestObjectResult("Please pass a valid userid on the query string or in the request body");
            }

            log.Info($"C# HTTP trigger function CreateRatings Check User ID Completed: {DateTime.Now}");

            // Challenge 8 - Adding sentiment analysis to user analysis
            log.Info($"C# HTTP trigger function CreateRatings Check Sentiment Analysis: {DateTime.Now}");

            var client = new RestClient(Environment.GetEnvironmentVariable("TextAnalyticsEndpoint", EnvironmentVariableTarget.Process));

            var request = new RestRequest("", Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Ocp-Apim-Subscription-Key", Environment.GetEnvironmentVariable("TextAnalyticsKey", EnvironmentVariableTarget.Process));

            DocumentSentiment sentimentDocPayload = new DocumentSentiment
            {
                Language = "en",
                Id = rating.id,
                Text = rating.usernotes
            };

            List<DocumentSentiment> sentiments = new List<DocumentSentiment>();
            sentiments.Add(sentimentDocPayload);

            Sentiment sentimentPayload = new Sentiment();
            sentimentPayload.Documents = sentiments.ToArray();
            request.AddJsonBody(sentimentPayload);
            request.RequestFormat = DataFormat.Json;

            IRestResponse response = client.Execute(request);
            var contentReturned = response.Content;

            log.Info($"C# HTTP trigger function CreateRatings Check Sentiment Analysis has Ended: {DateTime.Now}");

            if (response.IsSuccessful)
            {
                var sentimentDeserialized = JsonConvert.DeserializeObject<SentimentReturn>(contentReturned);
                rating.Sentiment = sentimentDeserialized.Documents[0].Score;
            }

            log.Info($"C# HTTP trigger function CreateRatings Save Called: {DateTime.Now}");

            var dbRepo = new DocumentDBRepository<Rating>("SOHFLLTable1", "Ratings");
            await dbRepo.CreateItemAsync(rating);

            log.Info($"C# HTTP trigger function CreateRatings Save Completed: {DateTime.Now}");
            
            return rating.userid != null
                ? (ActionResult)new OkObjectResult(rating)
                : new BadRequestObjectResult("Please pass a userID on the query string or in the request body");
        }
    }
}

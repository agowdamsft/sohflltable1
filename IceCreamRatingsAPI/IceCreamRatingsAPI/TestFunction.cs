
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace IceCreamRatingsAPI
{
    public static class TestFunction
    {
        [FunctionName("TestFunction")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, TraceWriter log)
        {

            string[] header = new string[] { "PBA461, 5 / 4 / 2018 6:15:07 PM,BBB222,Northwind Traders,456 Foodcenter Lane,98101,94.36,9.436"};

            string[] orderItems = new string[] { "PBA461,0f5a0fe8-4506-4332-969e-699a693334a8,2,15.99,31.98,3.198",
                                                 "PBA461,e4e7068e-500e-4a00-8be4-630d4594735b,1,3.99,3.99,0.399",
                                                 "PBA461,65ab124a-9b2c-4294-a52d-18839364ef15,2,8.99,17.98,1.798",
                                                 "PBA461,76065ecd-8a14-426d-a4cd-abbde2acbb10,9,4.49,40.41,4.041"};

            string[] products = new string[] {"76065ecd-8a14-426d-a4cd-abbde2acbb10,Gone Bananas,I'm not sure how appealing banana ice cream really is.",
                                              "65ab124a-9b2c-4294-a52d-18839364ef15,Durian Durian,Smells suspect but tastes... also suspect.",
                                              "e4e7068e-500e-4a00-8be4-630d4594735b,It's Grape!,Unraisinably good ice cream.",
                                              "0f5a0fe8-4506-4332-969e-699a693334a8,Beer,Hey this isn't ice cream! };",
                                              "BatchSavePO.ProcessPOInformation(header, orderItems, products);" };

            BatchSavePO.ProcessPOInformation(header, orderItems, products);

            return (ActionResult)new OkObjectResult($"Processed");

        }
    }
}

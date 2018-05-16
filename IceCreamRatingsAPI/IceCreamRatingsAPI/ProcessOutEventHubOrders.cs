using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace IceCreamRatingsAPI
{
    public static class ProcessOutEventHubOrders
    {
        [FunctionName("ProcessOutEventHubOrders")]
        public static async Task Run([EventHubTrigger("eventhubpos", Connection = "EVentHubConn")]string[] myEventHubMessage, TraceWriter log)
        {
            log.Info($"C# Event Hub trigger function processed a message Length: {myEventHubMessage.Length}");
            POS newpositem = new POS();

            foreach (string item in myEventHubMessage)
            {
                newpositem = JsonConvert.DeserializeObject<POS>(item);

                bool success = await BatchSavePOS.SavePOSAsync(newpositem);

                if (success)
                {
                    log.Info($"{item} \r\n");
                }
                else
                {
                    log.Info($"Failed to process 1 item");
                }
            }
        }
    }
}

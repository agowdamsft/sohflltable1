using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;

namespace IceCreamRatingsAPI
{
    public static class ProcessOutEventHubOrders
    {
        [FunctionName("ProcessOutEventHubOrders")]
        public static void Run([EventHubTrigger("challenge7post1", Connection = "EVentHubConn")]string myEventHubMessage, TraceWriter log)
        {
            log.Info($"C# Event Hub trigger function processed a message: {myEventHubMessage}");
        }
    }
}

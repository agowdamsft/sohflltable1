using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using System;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using System.Linq;

namespace IceCreamRatingsAPI
{
    public static class BatchOrch
    {
        [FunctionName("BatchOrch")]
        [Singleton]
        public static async void Run([BlobTrigger("batchch6/{name}", Connection = "AzureBlobFlat")]Stream myBlob, string name, TraceWriter log)
        {
            
            CloudStorageAccount storageAccount = null;
            CloudBlobContainer cloudBlobContainer = null;
          
            var prefix =  name.Substring(0, name.IndexOf("-"));
            string filenameorderheader=$"{prefix}-OrderHeaderDetails.csv";
            string filenameorderlineitems = $"{prefix}-OrderLineItems.csv";
            string filenameorderproductInfo = $"{prefix}-ProductInformation.csv";

                        

                    
            log.Info($"File Name That Came In:{name} \r\n");
            // Check whether the connection string can be parsed.
            if (CloudStorageAccount.TryParse(Environment.GetEnvironmentVariable("AzureBlobFlat",EnvironmentVariableTarget.Process), out storageAccount))
            {
                // If the connection string is valid, proceed with operations against Blob storage here.
                // Create a container called 'quickstartblobs' and append a GUID value to it to make the name unique. 
                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                cloudBlobContainer = cloudBlobClient.GetContainerReference("batchch6");
                BlobContinuationToken blobContinuationToken = null;



                var results = await cloudBlobContainer.ListBlobsSegmentedAsync(prefix, blobContinuationToken);
                // Get the value of the continuation token returned by the listing call.
                blobContinuationToken = results.ContinuationToken;
                int count = 0;
                foreach (IListBlobItem item in results.Results)
                {
                    //  log.Info($"\r\n File Found:" + item.Uri);
                    // Console.WriteLine(item.Uri);
                    count++;
                }

                if (count==3)
                {
                    log.Info($"All 3 files found - {name} , {filenameorderheader}, {filenameorderlineitems}, {filenameorderproductInfo}");


                    //TODO:
                    //Read the CSV files, skip first file
                    string[] orderHeaderContent = await ReadBlobFileAsync(cloudBlobContainer, filenameorderheader);
                    string[] orderlineContent = await ReadBlobFileAsync(cloudBlobContainer, filenameorderlineitems);
                    string[] productInfoContent = await ReadBlobFileAsync(cloudBlobContainer, filenameorderproductInfo);


                    //TODO:
                    // Call content processor


                }

                //do
                //{


                //    results = null;
                //}

                //while (blobContinuationToken != null); // Loop while the continuation token is not null.



            }
            else
            {
                // Otherwise, let the user know that they need to define the environment variable.
                Console.WriteLine(
                    "A connection string has not been defined in the system environment variables. " +
                    "Add a environment variable named 'storageconnectionstring' with your storage " +
                    "connection string as a value.");
                Console.WriteLine("Press any key to exit the sample application.");
                Console.ReadLine();
            }


    
        }

        private async static Task<string[]> ReadBlobFileAsync(CloudBlobContainer cloudBlobContainer, string blobFile)
        {
            string[] contents = null;

            if (cloudBlobContainer != null)
            {
                var blobReference = cloudBlobContainer.GetBlobReference(blobFile);
                var fileExists = await blobReference.ExistsAsync();
                if (fileExists)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await blobReference.DownloadToStreamAsync(memoryStream);
                        var text = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());

                        contents = text.Split('\n');

                    }
                }
            }
            return contents;
        }
    }
}

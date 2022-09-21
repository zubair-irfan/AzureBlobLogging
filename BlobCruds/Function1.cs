using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage.Blob;
using Azure.Storage.Blobs;
using Microsoft.WindowsAzure.Storage;
using System.Configuration;

namespace BlobCruds
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.User, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {

                var str = ConfigurationManager.AppSettings["AzureStorageConnectionString"];
                log.LogInformation("C# HTTP trigger function processed a request.");
                string connectionString = "AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;DefaultEndpointsProtocol=http;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;QueueEndpoint=http://127.0.0.1:10001/devstoreaccount1;TableEndpoint=http://127.0.0.1:10002/devstoreaccount1;";
                string conString = "DefaultEndpointsProtocol=https;AccountName=wsneptune;AccountKey=F4ESy610xDeUQ4ApNxwZc0OtV4SPr7JTC9dy/8UczPxK0UxpY/PI0o6RFnnjpuM+Ng7hV0x5m4zIVlDzGN43Fw==;BlobEndpoint=https://wsneptune.blob.core.windows.net/;QueueEndpoint=https://wsneptune.queue.core.windows.net/;TableEndpoint=https://wsneptune.table.core.windows.net/;FileEndpoint=https://wsneptune.file.core.windows.net/;";
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(conString);
                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference("test");
                // Create the container if it doesn't already exist
                await container.CreateIfNotExistsAsync();
                var blob = container.GetAppendBlobReference("applog.txt");
                if (!await blob.ExistsAsync())
                {
                    await blob.CreateOrReplaceAsync();
                }
                var st = $"User:User, Message:Message at - {DateTime.UtcNow}";
                await blob.AppendTextAsync($"[User:Test, Message:Message at - {DateTime.UtcNow}{Environment.NewLine}]");

                return new OkObjectResult(st);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}

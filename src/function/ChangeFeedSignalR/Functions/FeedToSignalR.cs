using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace ChangeFeedSignalR
{
    public static class FeedToSignalR
    {
        private static AzureSignalR signalR = new AzureSignalR(Environment.GetEnvironmentVariable("AzureSignalRConnectionString"));

        /// <summary>
        ///  This function Triggers upon new documents in the Cosmos DB database and broadcasts them to SignalR connected clients.
        /// </summary>
        [FunctionName("FeedToSignalR")]
        public static async Task Run([CosmosDBTrigger(
            databaseName: "chat",
            collectionName: "lines",
            ConnectionStringSetting = "AzureCosmosDBConnectionString",
            LeaseConnectionStringSetting = "AzureCosmosDBConnectionString",
            CreateLeaseCollectionIfNotExists = true,
            LeaseCollectionName = "leases")]IReadOnlyList<Document> documents, TraceWriter log)
        {
            if (documents != null && documents.Count > 0)
            {
                var broadcast = documents.Select((d) => new
                {
                    message = d.GetPropertyValue<string>("message"),
                    user = d.GetPropertyValue<string>("user")
                });

                await signalR.SendAsync("cosmicServerlessHub", "NewMessages", JsonConvert.SerializeObject(broadcast));
            }
        }
    }
}

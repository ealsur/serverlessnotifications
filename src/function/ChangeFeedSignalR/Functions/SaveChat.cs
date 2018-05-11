using System;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ChangeFeedSignalR
{
    public static class SaveChat
    {
        /// <summary>
        /// This HttpTriggered function is called from the web client to save a message into Azure Cosmos DB
        /// </summary>
        [FunctionName("SaveChat")]
        public static HttpResponseMessage Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]ChatMessage chatMessage,
            [DocumentDB("chat", "lines", Id = "id", ConnectionStringSetting = "AzureCosmosDBConnectionString")] out dynamic document,
            TraceWriter log)
        {
            document = new { id = Guid.NewGuid(), user = chatMessage.user, message = chatMessage.message };

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}

using System;
using System.Net;
using System.Net.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace ChangeFeedSignalR
{
    public static class SignalRConfiguration
    {
        private static AzureSignalR signalR = new AzureSignalR(Environment.GetEnvironmentVariable("AzureSignalRConnectionString"));

        /// <summary>
        /// This HttpTriggered function returns the SignalR configuration to the web client.
        /// </summary>
        [FunctionName("SignalRConfiguration")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous)]HttpRequestMessage req, TraceWriter log)
        {
            return req.CreateResponse(HttpStatusCode.OK, 
                new {
                    hubUrl = signalR.GetClientHubUrl("cosmicServerlessHub"),
                    accessToken = signalR.GenerateAccessToken("cosmicServerlessHub")
                });
        }
    }
}

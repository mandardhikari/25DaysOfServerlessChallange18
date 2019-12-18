// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using System.Threading.Tasks;
using SantasPerfectGift.Helpers;
using System.IO;

namespace SantasPerfectGift.Functions
{
    public class EvaluateGiftWithEventGrid
    {
        public readonly IComputerVisionHelper _computerVisionHelper;


        public EvaluateGiftWithEventGrid(IComputerVisionHelper computerVisionHelper)
        {
            _computerVisionHelper = computerVisionHelper ?? throw new ArgumentNullException(nameof(computerVisionHelper));
        }

        /// <summary>
        /// Azure Function To Evaluate Gift using Event Grid Trigger
        /// </summary>
        /// <param name="eventGridEvent"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        [FunctionName(nameof(EvaluateGiftWithEventGrid))]
        public async Task Run(
            [EventGridTrigger] EventGridEvent eventGridEvent, ILogger logger)
        {
            logger.LogInformation($"Started the evaluation of the gift");

            StorageBlobCreatedEventData storageBlobCreatedEventData =
                ((JObject)eventGridEvent.Data).ToObject<StorageBlobCreatedEventData>();

            string blobUrl = storageBlobCreatedEventData.Url;

            string fileName = Path.GetFileName(blobUrl);

            logger.LogInformation($"Evaluating Gift {fileName}");

            var isGiftProperlyWrapped = await _computerVisionHelper.IsPresentPerfectlyWrapped(blobUrl);

            if (!isGiftProperlyWrapped)
                logger.LogError($"Gift Not Correctly wrapped. SANTA NOT HAPPY!!!!!!!!!!!!!!");
            else
                logger.LogInformation($"Gift is correctly wrapped. SANTA HAPPY!!!!!!!");



        }

    }

}

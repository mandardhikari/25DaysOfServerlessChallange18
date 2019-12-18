using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SantasPerfectGift.Helpers;

namespace SantasPerfectGift.Functions
{

    public class EvaluateGiftWithBlobTrigger
    {
        private readonly IComputerVisionHelper _computerVisionHelper;

        public EvaluateGiftWithBlobTrigger(IComputerVisionHelper computerVisionHelper)
        {
            _computerVisionHelper = computerVisionHelper ?? throw new ArgumentNullException(nameof(computerVisionHelper));

        }

        /// <summary>
        /// Azure Function To Analyze the Gift with Blob Trigger
        /// </summary>
        /// <param name="blob"></param>
        /// <param name="name"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        [FunctionName(nameof(EvaluateGiftWithBlobTrigger))]
        public async Task Run(
            [BlobTrigger("analyzegiftwithblobtrigger/{name}", Connection = "SantasGiftBlobTriggerContainer")]
            Stream blob, string name, ILogger logger)
        {
            logger.LogInformation($"Analyzing Gift:{name}");

            var isGiftProperlyWrapped = await _computerVisionHelper.IsPresentPerfectlyWrapped(blob);

            if (!isGiftProperlyWrapped)
                logger.LogError($"Gift Not Correctly wrapped. SANTA NOT HAPPY!!!!!!!!!!!!!!");
            else
                logger.LogInformation($"Gift is correctly wrapped. SANTA HAPPY!!!!!!!");
        }
        
    }
    
    
}

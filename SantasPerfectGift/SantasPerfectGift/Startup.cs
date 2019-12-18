using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SantasPerfectGift.Helpers;

[assembly: FunctionsStartup(typeof(SantasPerfectGift.Startup))]
namespace SantasPerfectGift
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            //Register ComputerVisionClient
            builder.Services.AddSingleton<IComputerVisionClient>
                (new ComputerVisionClient(new ApiKeyServiceClientCredentials(Environment.GetEnvironmentVariable("ComputerVisionApiKey")))
                {
                    Endpoint = Environment.GetEnvironmentVariable("ComputerVisionEndPoint")
                }
                );

            //Register the ComputerVisionHelper
            builder.Services.AddSingleton<IComputerVisionHelper, ComputerVisionHelper>();
        }
    }
}

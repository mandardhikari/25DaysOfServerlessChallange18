using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SantasPerfectGift.Helpers
{
    /// <summary>
    /// Wrapper class on ComputerVision Client
    /// </summary>
    public class ComputerVisionHelper : IComputerVisionHelper
    {
        private readonly IComputerVisionClient _computerVisionClient;

        private readonly List<string> _neatlyWrapped;

        public ComputerVisionHelper(IComputerVisionClient computerVisionClient)
        {
            _computerVisionClient = computerVisionClient ?? throw new ArgumentNullException(nameof(computerVisionClient));
            _neatlyWrapped = new List<string>()
                { "box", "present", "gift wrapping", "ribbon" };
        }

        /// <summary>
        /// Determine if the gift is correctly wrapped using image url.
        /// </summary>
        /// <param name="imageUrl">Url of the image of the gift</param>
        /// <returns>Result if the image represents a correctly wrapped gift or not</returns>
        public async Task<bool> IsPresentPerfectlyWrapped(string imageUrl)
        {
            
            var result = await _computerVisionClient.TagImageAsync(imageUrl);

            return AnalyzeTags(result);
        }

        /// <summary>
        /// Determine if the gift is correctly wrapped using image stream
        /// </summary>
        /// <param name="blob">Image in the stream format</param>
        /// <returns>Result if the image represents a correctly wrapped gift or not</returns>
        public async Task<bool> IsPresentPerfectlyWrapped(Stream blob)
        {
            var result = await _computerVisionClient.TagImageInStreamAsync(blob);
            
            return AnalyzeTags(result);

        }

        /// <summary>
        /// Helper function
        /// </summary>
        /// <param name="tagResult"></param>
        /// <returns></returns>
        private bool AnalyzeTags(TagResult tagResult)
        {
            var extractedTags = new List<string>();

            foreach (var item in tagResult.Tags)
            {
                extractedTags.Add(item.Name);

            }

            bool isNeatlyWrapped = _neatlyWrapped.Intersect(extractedTags).Count() == _neatlyWrapped.Count();

            return isNeatlyWrapped;

        }
      

    }
}

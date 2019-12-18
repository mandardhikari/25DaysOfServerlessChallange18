using System.IO;
using System.Threading.Tasks;

namespace SantasPerfectGift.Helpers
{
    public interface IComputerVisionHelper
    {
        Task<bool> IsPresentPerfectlyWrapped(string imageUrl);

        Task<bool> IsPresentPerfectlyWrapped(Stream blob);
    }
}
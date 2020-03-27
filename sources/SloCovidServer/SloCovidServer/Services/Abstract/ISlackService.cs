using System.Threading;
using System.Threading.Tasks;

namespace SloCovidServer.Services.Abstract
{
    public interface ISlackService
    {
        Task SendNotificationAsync(string text, CancellationToken ct);
    }
}

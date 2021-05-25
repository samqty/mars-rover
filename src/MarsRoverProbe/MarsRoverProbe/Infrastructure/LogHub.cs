using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace MarsRoverProbe.Infrastructure
{
    public class LogHub : Hub
    {
        public async Task SendLog(string log)
        {
            await Clients.Caller.SendAsync("LogAdded", log);
        }
    }
}

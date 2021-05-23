using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverProbe.Infrastructure
{
    public interface IProgressLogger
    {
        public Task LogProgress(string message);
    }

    public class SignalRProgressLogger : IProgressLogger
    {
        private readonly IHubContext<LogHub> _hubContext;

        public SignalRProgressLogger(IHubContext<LogHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task LogProgress(string message)
        {
            await _hubContext.Clients.All.SendAsync("LogAdded", message);
        }
    }
}

using Microsoft.AspNetCore.SignalR;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverProbe.Infrastructure
{
    public class LogHub : Hub
    {
        public async Task SendLog(string log)
        {
            await Clients.All.SendAsync("LogAdded", log);
        }
    }
}

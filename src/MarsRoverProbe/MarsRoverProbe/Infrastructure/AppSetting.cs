using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverProbe.Infrastructure
{
    public class AppSetting
    {
        public string DatesFilesDirectory { get; set; }
        public string DownloadDestinationDirectory { get; set; }
        public string NasaApiKey { get; set; }
    }
}

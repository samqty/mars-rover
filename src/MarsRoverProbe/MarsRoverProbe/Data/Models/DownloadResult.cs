using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverProbe.Data.Models
{
    public class DownloadResult
    {
        public string ImageUrl { get; set; }
        public string FileName { get; set; }
        public bool IsSuccessfullyDownloaded { get; set; }
        public string Message { get; set; }
    }
}

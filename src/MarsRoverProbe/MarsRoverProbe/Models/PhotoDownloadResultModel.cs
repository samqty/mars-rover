using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverProbe.Models
{
    public class PhotoDownloadResultModel
    {
        public string ImageUrl { get; set; }
        public string FileName { get; set; }
        public bool IsSuccessfullyDownloaded { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverProbe.Models
{
    public class PhotoDownloadResponseModel
    {
        public string FileName { get; set; }
        public List<PhotoDownloadBatchModel> Batches { get; set; }
    }
}

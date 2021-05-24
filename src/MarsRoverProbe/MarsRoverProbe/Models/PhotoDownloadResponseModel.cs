using System.Collections.Generic;

namespace MarsRoverProbe.Models
{
    public class PhotoDownloadResponseModel
    {
        public string FileName { get; set; }
        public List<PhotoDownloadBatchModel> Batches { get; set; }
    }
}

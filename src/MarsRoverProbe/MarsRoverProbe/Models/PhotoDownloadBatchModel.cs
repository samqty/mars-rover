using System;
using System.Collections.Generic;

namespace MarsRoverProbe.Models
{
    public class PhotoDownloadBatchModel
    {
        public string RawDate { get; set; }
        public DateTime? Date { get; set; }

        public bool IsValid => Date != null;

        public List<PhotoDownloadResultModel> DownloadResults { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

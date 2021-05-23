using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverProbe.Data.Models
{
    public class Photo
    {
        public string earth_data { get; set; }
        public int id { get; set; }
        public int name { get; set; }
        public int rover_id { get; set; }
        public string img_src { get; set; }
        public int sole { get; set; }
    }
}

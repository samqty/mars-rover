using MarsRoverProbe.Data.Models;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverProbe.Data
{
    public interface INasaApi
    {
        [Get("/rovers/curiosity/photos")]
        Task<PhotosResponse> GetPhotos(string earth_date, string api_key);
    }
}

using MarsRoverProbe.Data;
using MarsRoverProbe.Data.Models;
using MarsRoverProbe.Infrastructure;
using MarsRoverProbe.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverProbe.Services
{
    public interface IMarsRoverPhotosService
    {
        Task<PhotoDownloadResponseModel> DownloadPhotos(string fileName);

        Task<byte[]> GetLocalPhoto(string name);
    }
    public class MarsRoverPhotosService : IMarsRoverPhotosService
    {
        private readonly INasaApi _nasaApi;
        private readonly ILogger<IMarsRoverPhotosService> _logger;
        private readonly IStorage _storage;
        private readonly AppSetting _appSetting;
        private readonly IProgressLogger _progressLogger;

        public MarsRoverPhotosService(INasaApi nasaApi,
            ILogger<IMarsRoverPhotosService> logger,
            IStorage storage,
            IOptions<AppSetting> setting,
            IProgressLogger progressLogger)
        {
            _nasaApi = nasaApi;
            _logger = logger;
            _storage = storage;
            _appSetting = setting.Value;
            _progressLogger = progressLogger;
        }

        public async Task<PhotoDownloadResponseModel> DownloadPhotos(string fileName)
        {
            var result = new PhotoDownloadResponseModel
            {
                FileName = fileName,
                Batches = new List<PhotoDownloadBatchModel>()
            };

            await _progressLogger.LogProgress($"Reading dates from : [{fileName}]");

            var photodates = await _storage.ReadDates(result.FileName);
            foreach (var photodate in photodates)
            {
                await _progressLogger.LogProgress($"Fetching photo metadata for : [{photodate}]");
                var batch = new PhotoDownloadBatchModel
                {
                    RawDate = photodate,
                    DownloadResults = new List<PhotoDownloadResultModel>()
                };
                result.Batches.Add(batch);

                if (!DateTime.TryParse(photodate, out var dt))
                {
                    await _progressLogger.LogProgress($"Invalid date format : [{photodate}]");
                    continue;
                }
                batch.Date = dt;

                var photos = await _nasaApi.GetPhotos(dt.ToString("yyyy-MM-dd"), _appSetting.NasaApiKey);
                var tasks = new List<Task<DownloadResult>>();
                photos.photos.ForEach(x =>
                {
                    tasks.Add(_storage.Save(x.img_src));
                });

                var all = await Task.WhenAll<DownloadResult>(tasks);

                all.ToList().ForEach(x => batch.DownloadResults.Add(new PhotoDownloadResultModel
                {
                    FileName = x.FileName,
                    ImageUrl = x.ImageUrl,
                    IsSuccessfullyDownloaded = x.IsSuccessfullyDownloaded
                }));

                await _progressLogger.LogProgress($"Completed Processing : [{photodate}]");
            }

            await _progressLogger.LogProgress($"Completed All Dates  in : [{fileName}]");
            return result;
        }

        public async Task<byte[]> GetLocalPhoto(string name)
        {
            return await _storage.GetPhotoContent(name);
        }
    }
}

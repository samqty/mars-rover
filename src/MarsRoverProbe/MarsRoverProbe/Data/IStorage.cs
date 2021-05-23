using MarsRoverProbe.Data.Models;
using MarsRoverProbe.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MarsRoverProbe.Data
{
    public interface IStorage
    {
        Task<DownloadResult> Save(string url);
        Task<List<string>> ReadDates(string listName);

        Task<byte[]> GetPhotoContent(string name);
    }

    public class FileStorage : IStorage
    {
        private readonly AppSetting _appSetting;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProgressLogger _progressLogger;

        public FileStorage(IOptions<AppSetting> settings, IWebHostEnvironment webHostEnvironment,IProgressLogger progressLogger)
        {
            _appSetting = settings.Value;
            _webHostEnvironment = webHostEnvironment;
            _progressLogger = progressLogger;
        }

        public async Task<byte[]> GetPhotoContent(string name)
        {
            byte[] buffer;

            var filepath = Path.Combine(_appSetting.DownloadDestinationDirectory, name);
            using (var fs = File.OpenRead(filepath))
            {
                buffer = new byte[fs.Length];
                await fs.ReadAsync(buffer, 0, (int)fs.Length);
            }

            return buffer;
        }

        public async Task<List<string>> ReadDates(string listName)
        {
            var path = Path.Combine(_webHostEnvironment.ContentRootPath,_appSetting.DatesFilesDirectory, listName);
            return await Task.FromResult(File.ReadAllLines(path).ToList());
        }

        public async Task<DownloadResult> Save(string url)
        {
            await _progressLogger.LogProgress($"Downloading [{url}] ..."); 
            var result = new DownloadResult {
                ImageUrl = url
            };

            try
            {
                var httpClient = new HttpClient();
                var filename = Path.GetFileName(url);
                var downloadFileName = Path.Combine(_webHostEnvironment.ContentRootPath, _appSetting.DownloadDestinationDirectory, filename);
                var buffer = await httpClient.GetByteArrayAsync(url);
                await File.WriteAllBytesAsync(downloadFileName, buffer);
                result.IsSuccessfullyDownloaded = true;
                result.FileName = filename;
                await _progressLogger.LogProgress($"Completed [{url}] ...");
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.IsSuccessfullyDownloaded = false;
                await _progressLogger.LogProgress($"Failed to download [{url}] : {ex.Message}");
            }

            return result;
        }
    }
}

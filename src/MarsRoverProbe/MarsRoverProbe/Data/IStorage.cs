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
    }

    public class FileStorage : IStorage
    {
        private readonly AppSetting _appSetting;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileStorage(IOptions<AppSetting> settings, IWebHostEnvironment webHostEnvironment)
        {
            _appSetting = settings.Value;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<List<string>> ReadDates(string listName)
        {
            var path = Path.Combine(_webHostEnvironment.ContentRootPath,_appSetting.DatesFilesDirectory, listName);
            return await Task.FromResult(File.ReadAllLines(path).ToList());
        }

        public async Task<DownloadResult> Save(string url)
        {
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
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.IsSuccessfullyDownloaded = false;
            }

            return result;
        }
    }
}

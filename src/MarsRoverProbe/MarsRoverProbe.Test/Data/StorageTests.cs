using MarsRoverProbe.Data;
using MarsRoverProbe.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MarsRoverProbe.Test.Data
{
    public class StorageTests
    {
        [Fact]
        public async Task TestGetPhotoContent()
        {
            //given a storage repository instance and a file saved on a local disk
            var baseDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Directory.CreateDirectory(Path.Combine(baseDir, "DownloadedPhotos"));
            var appSetting = new AppSetting { DownloadDestinationDirectory = "DownloadedPhotos"};
            var appSettingMock = new Mock<IOptions<AppSetting>>();
            appSettingMock.Setup(x => x.Value).Returns(appSetting);

            var webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            webHostEnvironmentMock.Setup(x => x.ContentRootPath).Returns(baseDir);

            var testFile = "testfile.txt";
            var content = System.Text.ASCIIEncoding.UTF8.GetBytes("jljlk");
            File.WriteAllBytes(Path.Combine(baseDir,appSetting.DownloadDestinationDirectory,testFile),content);

            var storage = new FileStorage(appSettingMock.Object, webHostEnvironmentMock.Object, new Mock<IProgressLogger>().Object);

            //when a get photo is invoked with a file name
            var result = await storage.GetPhotoContent(testFile);

            //then the content of the file is pulled from a local disc
            Assert.NotNull(result);
            Assert.Equal(result, content);
        }
    }
}

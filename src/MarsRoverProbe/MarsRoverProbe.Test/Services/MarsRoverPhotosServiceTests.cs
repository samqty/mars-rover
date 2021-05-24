using MarsRoverProbe.Data;
using MarsRoverProbe.Data.Models;
using MarsRoverProbe.Infrastructure;
using MarsRoverProbe.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace MarsRoverProbe.Test.Services
{
    public class MarsRoverPhotosServiceTests
    {
        [Fact]
        public async Task DownloadPhotosTest()
        {
            //given a file containing list of dates
            var datesFileName = "test.txt";
            var dates = new List<string> { "2020-02-01" };
            var appSetting = new AppSetting
            {
                NasaApiKey = "TestKey"
            };

            var testNasaPhotos = new PhotosResponse
            {
                photos = new List<Photo>
                {
                    new Photo
                    {
                        img_src = "http://test.com/testurl.png"
                    }
                }
            };

            var photoDownloadResult = new DownloadResult
            {
                ImageUrl = testNasaPhotos.photos[0].img_src
            };

            var nasaApiMock = new Mock<INasaApi>();
            var storageMock = new Mock<IStorage>();
            var settingMock = new Mock<IOptions<AppSetting>>();

            storageMock.Setup(x => x.ReadDates(datesFileName)).Returns(Task.FromResult(dates));
            storageMock.Setup(x => x.Save(testNasaPhotos.photos[0].img_src)).Returns(Task.FromResult(photoDownloadResult));
            nasaApiMock.Setup(x => x.GetPhotos(dates[0], appSetting.NasaApiKey)).Returns(Task.FromResult(testNasaPhotos));
            settingMock.Setup(x => x.Value).Returns(appSetting);

            var service = new MarsRoverPhotosService(nasaApiMock.Object,
                new Mock<ILogger<MarsRoverPhotosService>>().Object,
                storageMock.Object,
                settingMock.Object,
                new Mock<IProgressLogger>().Object);

            //when a download photos is invoked
            var result = await service.DownloadPhotos(datesFileName);

            //then downloads data to storage
            Assert.NotNull(result);
            storageMock.Verify(x => x.ReadDates(datesFileName), Times.Once);
            storageMock.Verify(x => x.Save(testNasaPhotos.photos[0].img_src), Times.Once);
            nasaApiMock.Verify(x => x.GetPhotos(dates[0], appSetting.NasaApiKey));
        }

        [Fact]
        public async Task GetLocalPhotoTest()
        {
            //given a service instance and a photo
            var fileName = "testphoto.jpg";
            var nasaApiMock = new Mock<INasaApi>();
            var storageMock = new Mock<IStorage>();
            var settingMock = new Mock<IOptions<AppSetting>>();

            storageMock.Setup(x => x.GetPhotoContent(fileName)).Returns(Task.FromResult(System.Text.ASCIIEncoding.UTF8.GetBytes("Test Photo")));

            var service = new MarsRoverPhotosService(nasaApiMock.Object,
                new Mock<ILogger<MarsRoverPhotosService>>().Object,
                storageMock.Object,
                settingMock.Object,
                 new Mock<IProgressLogger>().Object);

            //when getphoto is called
            var result = await service.GetLocalPhoto(fileName);

            //then it invokes the underlying storage service with the correct parameter
            Assert.NotNull(result);
            storageMock.Verify(x => x.GetPhotoContent(fileName), Times.Once);
        }
    }
}

using MarsRoverProbe.Controllers;
using MarsRoverProbe.Models;
using MarsRoverProbe.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MarsRoverProbe.Test.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public async Task TestDownloadPhotos()
        {
            //given controller instance
            var datesFileName = "dates.txt";
            var marsRoverPhotosServiceMock = new Mock<IMarsRoverPhotosService>();
            var response = new PhotoDownloadResponseModel
            {
                Batches = new List<PhotoDownloadBatchModel> { },
                FileName = datesFileName
            };

            marsRoverPhotosServiceMock.Setup(x => x.DownloadPhotos(datesFileName)).Returns(Task.FromResult(response));
            var controller = new HomeController(new Mock<ILogger<HomeController>>().Object, marsRoverPhotosServiceMock.Object);

            //when downloadphotos is invoked
            var result = await controller.DownloadPhotos();

            //then underlying service is invoked with the correct parameter
            Assert.NotNull(result);
            marsRoverPhotosServiceMock.Verify(x => x.DownloadPhotos(datesFileName), Times.Once);
        }

        [Fact]
        public async Task TestGetPhoto()
        {
            //given controller instance
            var filename = "testfile.jpg";
            var marsRoverPhotosServiceMock = new Mock<IMarsRoverPhotosService>();
            marsRoverPhotosServiceMock.Setup(x => x.GetLocalPhoto(filename)).Returns(Task.FromResult(ASCIIEncoding.UTF8.GetBytes("This is an image")));

            var controller = new HomeController(new Mock<ILogger<HomeController>>().Object, marsRoverPhotosServiceMock.Object);

            //when getphoto is invoked with a given filename
            var result = await controller.GetPhoto(filename);

            //then the underlying service is invoked with the correct parameter
            Assert.NotNull(result);
            marsRoverPhotosServiceMock.Verify(x => x.GetLocalPhoto(filename), Times.Once);
        }
    }
}

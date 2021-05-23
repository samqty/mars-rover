using MarsRoverProbe.Controllers;
using MarsRoverProbe.Models;
using MarsRoverProbe.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
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
            var datesFileName = "Dates.txt";
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
    }
}

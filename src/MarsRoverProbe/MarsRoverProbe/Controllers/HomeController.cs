using MarsRoverProbe.Models;
using MarsRoverProbe.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MarsRoverProbe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMarsRoverPhotosService _marsRoverPhotoService;

        public HomeController(ILogger<HomeController> logger, IMarsRoverPhotosService marsRoverPhotoService)
        {
            _logger = logger;
            _marsRoverPhotoService = marsRoverPhotoService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DownloadPhotos()
        {
            var result = await _marsRoverPhotoService.DownloadPhotos("dates.txt");
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPhoto(string filename)
        {
            return File(await _marsRoverPhotoService.GetLocalPhoto(filename), "image/jpg");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}

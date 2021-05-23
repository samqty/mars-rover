using MarsRoverProbe.Infrastructure;
using MarsRoverProbe.Models;
using MarsRoverProbe.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRoverProbe.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMarsRoverPhotosService _marsRoverPhotoService;
        private readonly IHubContext<LogHub> _hubContext;

        public HomeController(ILogger<HomeController> logger, IMarsRoverPhotosService marsRoverPhotoService, IHubContext<LogHub> hubContext)
        {
            _logger = logger;
            _marsRoverPhotoService = marsRoverPhotoService;
            _hubContext = hubContext;
        }



        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DownloadPhotos()
        {
            var result = await _marsRoverPhotoService.DownloadPhotos("Dates.txt");
            return Ok(result);
        }

        [HttpGet]
        public async Task Log()
        {
            await _hubContext.Clients.All.SendAsync("LogAdded", "this is my message");
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

        [HttpGet]
        public async Task<IActionResult> GetPhoto(string filename)
        {
            return File(await _marsRoverPhotoService.GetLocalPhoto(filename), "image/jpg");
        }
    }
}

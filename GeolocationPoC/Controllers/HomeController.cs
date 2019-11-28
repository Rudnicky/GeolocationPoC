using GeolocationPoC.Core.Interfaces.Web;
using GeolocationPoC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace GeolocationPoC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGeolocationRepository _geolocationRepository;

        public HomeController(ILogger<HomeController> logger, IGeolocationRepository geolocationRepository)
        {
            _logger = logger;
            _geolocationRepository = geolocationRepository;
        }

        public async Task<IActionResult> Index()
        {
            //try
            //{
            //    var result = await _geolocationRepository.Get("37.30.19.137");
            //    if (result != null)
            //    {

            //    }
            //}
            //catch (Exception)
            //{

            //}

            return View();
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

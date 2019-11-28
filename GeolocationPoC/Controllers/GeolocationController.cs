using GeolocationPoC.Core.Domain;
using GeolocationPoC.Core.Interfaces.Db;
using GeolocationPoC.Core.Interfaces.Web;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GeolocationPoC.Controllers
{
    public class GeolocationController : Controller
    {
        private readonly IGeolocationRepository _geolocationRepository;
        private readonly IGeolocationDbRepository _geolocationDbRepository;

        public GeolocationController(
            IGeolocationRepository geolocationRepository,
            IGeolocationDbRepository geolocationDbRepository)
        {
            _geolocationRepository = geolocationRepository;
            _geolocationDbRepository = geolocationDbRepository;
        }

        public async Task<IActionResult> Index()
        {
            var geolocations = await _geolocationDbRepository.GetAll();
            if (geolocations != null)
            {
                return View("List", geolocations);
            }
            return NotFound();
        }

        [HttpGet("/Create")]
        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost("/Create")]
        public async Task<IActionResult> Create(IpAddress ipAddress)
        {
            // get list of geolocations from DB
            var geolocations = await _geolocationDbRepository.GetAll();

            // get data through web service call
            var result = await _geolocationRepository.Get(ipAddress.Ip);

            if (geolocations != null && result != null)
            {
                var geolocation = new Geolocation()
                {
                    Ip = result.Ip,
                    CountryName = result.CountryName,
                    Latitude = result.Latitude,
                    Longitude = result.Longitude,
                    Location = result.Location
                };

                // add obtained geolocation to the DB
                _geolocationDbRepository.Add(geolocation);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("/Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var goelocation = _geolocationDbRepository.Get(id);
            if (goelocation != null)
            {
                _geolocationDbRepository.Delete(goelocation);
            }
            return RedirectToAction("Index");
        }

        [HttpGet("/Details/{id}")]
        public IActionResult Details(int id)
        {
            var goelocation = _geolocationDbRepository.Get(id);
            if (goelocation != null)
            {
                return View("Details", goelocation);
            }
            return NotFound();
        }

        [HttpGet("/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            var goelocation = _geolocationDbRepository.Get(id);
            if (goelocation != null)
            {
                return View("Edit", goelocation);
            }
            return NotFound();
        }

        [HttpPost("/Edit/{id}")]
        public IActionResult Edit(Geolocation geolocation)
        {
            _geolocationDbRepository.Update(geolocation);

            return RedirectToAction("Index");
        }
    }
}
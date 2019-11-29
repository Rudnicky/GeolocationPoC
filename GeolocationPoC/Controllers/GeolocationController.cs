using GeolocationPoC.Core.Domain;
using GeolocationPoC.Core.Interfaces.WebRequestAccessLayer;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace GeolocationPoC.Controllers
{
    public class GeolocationController : Controller
    {
        private readonly IGeolocationApi _geolocationApi;

        public GeolocationController(IGeolocationApi geolocationApi)
        {
            _geolocationApi = geolocationApi;
        }

        public async Task<IActionResult> Index()
        {
            var geolocations = await _geolocationApi.GetAll();
            if (geolocations != null)
            {
                return View("List", geolocations.ToList());
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
            //// get list of geolocations from DB
            //var geolocations = await _geolocationApi.GetAll();

            //// get data through web service call
            //var result = await _geolocationApi.Get(ipAddress.Ip);

            //if (geolocations != null && result != null)
            //{
            //    var geolocation = new Geolocation()
            //    {
            //        Ip = result.Ip,
            //        CountryName = result.CountryName,
            //        Latitude = result.Latitude,
            //        Longitude = result.Longitude,
            //        Location = result.Location
            //    };

            //    // add obtained geolocation to the DB
            //    _geolocationDbRepository.Add(geolocation);
            //}

            return RedirectToAction("Index");
        }

        [HttpGet("/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var goelocation = _geolocationApi.Get(id.ToString());
            if (goelocation != null)
            {
                await _geolocationApi.Delete(id.ToString());
            }
            return RedirectToAction("Index");
        }

        [HttpGet("/Details/{id}")]
        public IActionResult Details(int id)
        {
            var goelocation = _geolocationApi.Get(id.ToString());
            if (goelocation != null)
            {
                return View("Details", goelocation);
            }
            return NotFound();
        }

        [HttpGet("/Edit/{id}")]
        public IActionResult Edit(int id)
        {
            //var goelocation = _geolocationDbRepository.Get(id);
            //if (goelocation != null)
            //{
            //    return View("Edit", goelocation);
            //}
            return NotFound();
        }

        [HttpPost("/Edit/{id}")]
        public IActionResult Edit(Geolocation geolocation)
        {
            //_geolocationDbRepository.Update(geolocation);

            return RedirectToAction("Index");
        }
    }
}
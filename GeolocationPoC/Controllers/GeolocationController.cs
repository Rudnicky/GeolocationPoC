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
            await _geolocationApi.Post(ipAddress.Ip);

            return RedirectToAction("Index");
        }

        [HttpGet("/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var goelocation = await _geolocationApi.Get(id.ToString());
            if (goelocation != null)
            {
                await _geolocationApi.Delete(id.ToString());
            }
            return RedirectToAction("Index");
        }

        [HttpGet("/Details/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var goelocation = await _geolocationApi.Get(id.ToString());
            if (goelocation != null)
            {
                return View("Details", goelocation);
            }
            return NotFound();
        }

        [HttpGet("/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var goelocation = await _geolocationApi.Get(id.ToString());
            if (goelocation != null)
            {
                return View("Edit", goelocation);
            }
            return NotFound();
        }

        [HttpPost("/Edit/{id}")]
        public async Task<IActionResult> Edit(Geolocation geolocation)
        {
            await _geolocationApi.Put(geolocation);

            return RedirectToAction("Index");
        }
    }
}
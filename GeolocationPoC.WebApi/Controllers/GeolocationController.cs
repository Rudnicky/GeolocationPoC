using GeolocationPoC.Core.Domain;
using GeolocationPoC.Core.Exceptions;
using GeolocationPoC.Core.Interfaces.DatabaseAccessLayer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace GeolocationPoC.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GeolocationController : Controller
    {
        private readonly IGeolocationDbRepository _geolocationDbRepository;
        private readonly IGeolocationManager _geolocationManager;

        public GeolocationController(IGeolocationDbRepository geolocationDbRepository, IGeolocationManager geolocationManager)
        {
            _geolocationDbRepository = geolocationDbRepository;
            _geolocationManager = geolocationManager;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Geolocation>>> GetAll()
        {
            try
            {
                var geolocations = await _geolocationDbRepository.GetAll();
                if (geolocations != null)
                {
                    return Ok(geolocations);
                }
            }
            catch (Exception ex)
            {
                return Json(new GeolocationException(ex.Message, "Geolocation", "GetAll"));
            }
            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Geolocation>> Get(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest("Id must be specified and not equal zero");
                }

                var geolocation = await _geolocationDbRepository.Get(id);
                if (geolocation != null)
                {
                    return NotFound($"Object with specified id={id} not found");
                }
                return Ok(geolocation);
            }
            catch (Exception ex)
            {
                return Json(new GeolocationException(ex.Message, "Geolocation", "Get"));
            }
        }

        [HttpPost("create/{ip}")]
        public async Task<IActionResult> Post(string ip)
        {
            var result = await _geolocationManager.Save(ip);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return Json(new GeolocationException(result.Message, "Geolocation", "Post", result.StatusCode));
            }
            return Ok(result.Message);
        }

        [HttpPut("update")]
        public async Task<IActionResult> Put(Geolocation geolocation)
        {
            var result = await _geolocationManager.Update(geolocation);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return Json(new GeolocationException(result.Message, "Geolocation", "Put", result.StatusCode));
            }
            return Ok(result.Message);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _geolocationManager.Delete(id);
            if (result.StatusCode != HttpStatusCode.OK)
            {
                return Json(new GeolocationException(result.Message, "Geolocation", "Delete", result.StatusCode));
            }
            return Ok(result.Message);
        }
    }
}

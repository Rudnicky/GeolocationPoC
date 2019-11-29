using GeolocationPoC.Core.Domain;
using GeolocationPoC.Core.Exceptions;
using GeolocationPoC.Core.Interfaces.DatabaseAccessLayer;
using GeolocationPoC.Core.Interfaces.WebRequestAccessLayer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GeolocationPoC.WebApi.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public class GeolocationController : Controller
    {
        private readonly IGeolocationRepository _geolocationRepository;
        private readonly IGeolocationDbRepository _geolocationDbRepository;

        public GeolocationController(IGeolocationRepository geolocationRepository, IGeolocationDbRepository geolocationDbRepository)
        {
            _geolocationRepository = geolocationRepository;
            _geolocationDbRepository = geolocationDbRepository;
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

        /// <summary>
        /// TODO: all of this validation *might* be moved to
        /// some kind of geolocation manager dao
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpPost("create/{ip}")]
        public async Task<IActionResult> Post(string ip)
        {
            try
            {
                if (string.IsNullOrEmpty(ip))
                {
                    return BadRequest("Ip cannot be empty");
                }

                var pattern = @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
                var match = Regex.Match(ip, pattern);
                if (!match.Success)
                {
                    return BadRequest("Ip has a wrong pattern. Example: [15.37.112.196]");
                }

                var exists = await _geolocationDbRepository.FindByIp(ip);
                if (exists != null)
                {
                    return BadRequest("Object already exists");
                }

                var result = await _geolocationRepository.Get(ip);
                if (result == null)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, "Something went wrong during getting data through https://ipstack.com/ api");
                }

                // special case for retrieving object through external api
                // it returns 200 although object is empty
                if (string.IsNullOrEmpty(result.CountryName))
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, $"There's no data availabe under {ip} according to https://ipstack.com/ api");
                }

                var geolocation = new Geolocation()
                {
                    Ip = result.Ip,
                    CountryName = result.CountryName,
                    Latitude = result.Latitude,
                    Longitude = result.Longitude,
                    Location = result.Location
                };

                // store retrieved object into our DB
                _geolocationDbRepository.Add(geolocation);

                var geolocationCreated = await _geolocationDbRepository.FindByIp(ip);
                if (geolocationCreated != null)
                {
                    return Created($"api/geolocation/{ip}", "Object Created Successfully!");
                }

                return StatusCode((int)HttpStatusCode.FailedDependency, "Propbably something went wrong during adding object to the database");
            }
            catch (Exception ex)
            {
                return Json(new GeolocationException(ex.Message, "Geolocation", "Post"));
            }
        }

        [HttpPut("update")]
        public async Task<IActionResult> Put(Geolocation geolocation)
        {
            try
            {
                if (geolocation.Id == 0)
                {
                    return BadRequest("Id must be specified and not equal zero");
                }

                var exists = await _geolocationDbRepository.Get(geolocation.Id);
                if (exists == null)
                {
                    return BadRequest("Object deos not exists. Please verify your id");
                }

                var pattern = @"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";
                var match = Regex.Match(geolocation.Ip, pattern);
                if (!match.Success)
                {
                    return BadRequest("Ip has a wrong pattern. Example: [15.37.112.196]");
                }

                exists.Ip = !string.IsNullOrEmpty(geolocation.Ip) ? geolocation.Ip : exists.Ip;
                exists.CountryName = !string.IsNullOrEmpty(geolocation.CountryName) ? geolocation.CountryName : exists.CountryName;
                exists.Latitude = geolocation.Latitude > 0 ? geolocation.Latitude : exists.Latitude;
                exists.Longitude = geolocation.Longitude > 0 ? geolocation.Longitude : exists.Longitude;
                exists.Location = geolocation.Location != null ? geolocation.Location : exists.Location;

                _geolocationDbRepository.Update(exists);

                return Ok($"Object with id={geolocation.Id} updated Successfully!");
            }
            catch (Exception ex)
            {
                return Json(new GeolocationException(ex.Message, "Geolocation", "Put"));
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                if (!int.TryParse(id, out int value))
                {
                    return BadRequest("Id has wrong format");
                }

                var exists = await _geolocationDbRepository.Get(int.Parse(id));
                if (exists == null)
                {
                    return BadRequest($"Object with given id={id} does not exists");
                }

                _geolocationDbRepository.Delete(exists);

                var deletedObject = await _geolocationDbRepository.Get(int.Parse(id));
                if (deletedObject != null)
                {
                    return BadRequest($"Something went wrong during deleting object from the database");
                }

                return Ok($"Object with given id={id} deleted Successfully");
            }
            catch (Exception ex)
            {
                return Json(new GeolocationException(ex.Message, "Geolocation", "Delete"));
            }
        }
    }
}

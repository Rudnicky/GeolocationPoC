using GeolocationPoC.Core.Domain;
using GeolocationPoC.Core.Domain.Db;
using GeolocationPoC.Core.Interfaces.DatabaseAccessLayer;
using GeolocationPoC.Core.Interfaces.WebRequestAccessLayer;
using GeolocationPoC.Core.Utils;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GeolocationPoC.Persistence.Repositories.DatabaseAccessLayer
{
    public class GeolocationManager : IGeolocationManager
    {
        private readonly IGeolocationRepository _geolocationRepository;
        private readonly IGeolocationDbRepository _geolocationDbRepository;

        public GeolocationManager(IGeolocationRepository geolocationRepository, IGeolocationDbRepository geolocationDbRepository)
        {
            _geolocationRepository = geolocationRepository;
            _geolocationDbRepository = geolocationDbRepository;
        }

        public async Task<GeolocationResult> Delete(string id)
        {
            try
            {
                if (!int.TryParse(id, out int value))
                {
                    return new GeolocationResult() { StatusCode = HttpStatusCode.BadRequest, Message = ErrorMessages.WRONG_ID_FORMAT };
                }

                var exists = await _geolocationDbRepository.Get(int.Parse(id));
                if (exists == null)
                {
                    return new GeolocationResult() { StatusCode = HttpStatusCode.BadRequest, Message = ErrorMessages.NOT_EXISTS };
                }

                _geolocationDbRepository.Delete(exists);

                var deletedObject = await _geolocationDbRepository.Get(int.Parse(id));
                if (deletedObject != null)
                {
                    return new GeolocationResult() { StatusCode = HttpStatusCode.BadRequest, Message = ErrorMessages.DB_ERROR };
                }

                return new GeolocationResult() { StatusCode = HttpStatusCode.OK, Message = ErrorMessages.DELETED };
            }
            catch (Exception ex)
            {
                return new GeolocationResult() { StatusCode = HttpStatusCode.InternalServerError, Message = ex.Message };
            }
        }

        public async Task<GeolocationResult> Save(string ip)
        {
            try
            {
                if (string.IsNullOrEmpty(ip))
                {
                    return new GeolocationResult() { StatusCode = HttpStatusCode.BadRequest, Message = ErrorMessages.WRONG_IP };
                }
                
                var match = Regex.Match(ip, Constants.IP_REGEX_PATTERN);
                if (!match.Success)
                {
                    return new GeolocationResult() { StatusCode = HttpStatusCode.BadRequest, Message = ErrorMessages.WRONG_IP_PATTERN };
                }

                var exists = await _geolocationDbRepository.FindByIp(ip);
                if (exists != null)
                {
                    return new GeolocationResult() { StatusCode = HttpStatusCode.BadRequest, Message = ErrorMessages.EXISTS };
                }

                var result = await _geolocationRepository.Get(ip);
                if (result == null)
                {
                    return new GeolocationResult() { StatusCode = HttpStatusCode.InternalServerError, Message = ErrorMessages.IP_STACK_ERROR };
                }

                // special case for retrieving object through external api
                // it returns 200 although object is empty
                if (string.IsNullOrEmpty(result.CountryName))
                {
                    return new GeolocationResult() { StatusCode = HttpStatusCode.InternalServerError, Message = $"There's no data availabe under {ip} according to https://ipstack.com/ api" };
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
                    return new GeolocationResult() { StatusCode = HttpStatusCode.OK, Message = ErrorMessages.CREATED };
                }

                return new GeolocationResult() { StatusCode = HttpStatusCode.FailedDependency, Message = ErrorMessages.DB_ERROR };
            }
            catch (Exception ex)
            {
                return new GeolocationResult() { StatusCode = HttpStatusCode.InternalServerError, Message = ex.Message };
            }
        }

        public async Task<GeolocationResult> Update(Geolocation geolocation)
        {
            try
            {
                if (geolocation.Id == 0)
                {
                    return new GeolocationResult() { StatusCode = HttpStatusCode.BadRequest, Message = ErrorMessages.WRONG_ID };
                }

                var exists = await _geolocationDbRepository.Get(geolocation.Id);
                if (exists == null)
                {
                    return new GeolocationResult() { StatusCode = HttpStatusCode.BadRequest, Message = ErrorMessages.NOT_EXISTS };
                }

                var match = Regex.Match(geolocation.Ip, Constants.IP_REGEX_PATTERN);
                if (!match.Success)
                {
                    return new GeolocationResult() { StatusCode = HttpStatusCode.BadRequest, Message = ErrorMessages.WRONG_IP_PATTERN };
                }

                exists.Ip = !string.IsNullOrEmpty(geolocation.Ip) ? geolocation.Ip : exists.Ip;
                exists.CountryName = !string.IsNullOrEmpty(geolocation.CountryName) ? geolocation.CountryName : exists.CountryName;
                exists.Latitude = geolocation.Latitude > 0 ? geolocation.Latitude : exists.Latitude;
                exists.Longitude = geolocation.Longitude > 0 ? geolocation.Longitude : exists.Longitude;
                exists.Location = geolocation.Location != null ? geolocation.Location : exists.Location;

                _geolocationDbRepository.Update(exists);

                return new GeolocationResult() { StatusCode = HttpStatusCode.OK, Message = ErrorMessages.UPDATED };
            }
            catch (Exception ex)
            {
                return new GeolocationResult() { StatusCode = HttpStatusCode.InternalServerError, Message = ex.Message };
            }
        }
    }
}

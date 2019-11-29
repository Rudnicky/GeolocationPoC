using GeolocationPoC.Core.Domain;
using GeolocationPoC.Core.Domain.Web;
using GeolocationPoC.Core.Interfaces.DatabaseAccessLayer;
using GeolocationPoC.Core.Interfaces.WebRequestAccessLayer;
using GeolocationPoC.Core.Utils;
using GeolocationPoC.Persistence.Repositories.DatabaseAccessLayer;
using GeolocationPoC.Tests.Helpers;
using NSubstitute;
using System.Net;
using Xunit;

namespace GeolocationPoC.Tests
{
    /// <summary>
    /// Unit Tests for the GeolocationManager class
    /// Naming convention: MethodName_ExpectedBehavior_StateUnderTest
    /// </summary>
    public class GeolocationManagerTests
    {
        [Fact]
        public async void Save_ShouldNotCreateGeolocation_EmptyIp()
        {
            // Arrange
            var geolocationRepository = Substitute.For<IGeolocationRepository>();
            var geolocationDbRepository = Substitute.For<IGeolocationDbRepository>();
            var geolocationManager = new GeolocationManager(geolocationRepository, geolocationDbRepository);

            // Act
            var result = await geolocationManager.Save(string.Empty);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal(ErrorMessages.WRONG_IP, result.Message);

            await geolocationDbRepository.DidNotReceiveWithAnyArgs().FindByIp(Arg.Any<string>());
            await geolocationRepository.DidNotReceiveWithAnyArgs().Get(Arg.Any<string>());
            geolocationDbRepository.DidNotReceiveWithAnyArgs().Add(Arg.Any<Geolocation>());
        }

        [Fact]
        public async void Save_ShouldNotCreateGeolocation_WrongIpFormat()
        {
            // Arrange
            var ip = "1232.22323.@@@@.3333..333..";
            var geolocationRepository = Substitute.For<IGeolocationRepository>();
            var geolocationDbRepository = Substitute.For<IGeolocationDbRepository>();
            var geolocationManager = new GeolocationManager(geolocationRepository, geolocationDbRepository);

            // Act
            var result = await geolocationManager.Save(ip);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal(ErrorMessages.WRONG_IP_PATTERN, result.Message);

            await geolocationDbRepository.DidNotReceiveWithAnyArgs().FindByIp(Arg.Any<string>());
            await geolocationRepository.DidNotReceiveWithAnyArgs().Get(Arg.Any<string>());
            geolocationDbRepository.DidNotReceiveWithAnyArgs().Add(Arg.Any<Geolocation>());
        }

        [Fact]
        public async void Save_ShouldNotCreateGeolocation_DuplicatedObject()
        {
            // Arrange
            var ip = "35.23.168.196";
            var geolocationRepository = Substitute.For<IGeolocationRepository>();
            var geolocationDbRepository = Substitute.For<IGeolocationDbRepository>();

            geolocationDbRepository.FindByIp(ip).Returns(new Geolocation());

            var geolocationManager = new GeolocationManager(geolocationRepository, geolocationDbRepository);

            // Act
            var result = await geolocationManager.Save(ip);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal(ErrorMessages.EXISTS, result.Message);

            await geolocationDbRepository.Received().FindByIp(ip);
            await geolocationRepository.DidNotReceiveWithAnyArgs().Get(Arg.Any<string>());
            geolocationDbRepository.DidNotReceiveWithAnyArgs().Add(Arg.Any<Geolocation>());
        }

        [Fact]
        public async void Save_ShouldNotCreateGeolocation_IpStackApiError()
        {
            // Arrange
            var ip = "35.23.168.196";
            var geolocationRepository = Substitute.For<IGeolocationRepository>();
            var geolocationDbRepository = Substitute.For<IGeolocationDbRepository>();

            Geolocation geolocation = null;
            IpStack ipStack = null;

            geolocationDbRepository.FindByIp(ip).Returns(geolocation);
            geolocationRepository.Get(ip).Returns(ipStack);

            var geolocationManager = new GeolocationManager(geolocationRepository, geolocationDbRepository);

            // Act
            var result = await geolocationManager.Save(ip);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Equal(ErrorMessages.IP_STACK_ERROR, result.Message);

            await geolocationDbRepository.Received().FindByIp(ip);
            await geolocationRepository.Received().Get(ip);
            geolocationDbRepository.DidNotReceiveWithAnyArgs().Add(Arg.Any<Geolocation>());
        }

        [Fact]
        public async void Save_ShouldNotCreateGeolocation_IpStackApiSpecialCaseError()
        {
            // Arrange
            var ip = "35.23.168.196";
            var geolocationRepository = Substitute.For<IGeolocationRepository>();
            var geolocationDbRepository = Substitute.For<IGeolocationDbRepository>();

            Geolocation geolocation = null;
            IpStack ipStack = new IpStack();

            geolocationDbRepository.FindByIp(ip).Returns(geolocation);
            geolocationRepository.Get(ip).Returns(ipStack);

            var geolocationManager = new GeolocationManager(geolocationRepository, geolocationDbRepository);

            // Act
            var result = await geolocationManager.Save(ip);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.Contains("https://ipstack.com/", result.Message);

            await geolocationDbRepository.Received().FindByIp(ip);
            await geolocationRepository.Received().Get(ip);
            geolocationDbRepository.DidNotReceiveWithAnyArgs().Add(Arg.Any<Geolocation>());
        }

        [Fact]
        public async void Save_ShouldCreateObject_ValidData()
        {
            // Arrange
            var ip = "35.23.168.196";
            var geolocationRepository = Substitute.For<IGeolocationRepository>();
            var geolocationDbRepository = Substitute.For<IGeolocationDbRepository>();

            Geolocation geolocation = null;
            IpStack ipStack = new IpStack()
            {
                CountryName = "Poland"
            };

            var results = new Results<Geolocation>(geolocation).Then(new Geolocation());
            geolocationDbRepository.FindByIp(ip).Returns(x => results.Next());
            geolocationRepository.Get(ip).Returns(ipStack);

            var geolocationManager = new GeolocationManager(geolocationRepository, geolocationDbRepository);

            // Act
            var result = await geolocationManager.Save(ip);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);

            // TODO: change error messages static class name.
            Assert.Equal(ErrorMessages.CREATED, result.Message);

            await geolocationDbRepository.Received(2).FindByIp(ip);
            await geolocationRepository.Received().Get(ip);
            geolocationDbRepository.Received().Add(Arg.Any<Geolocation>());
        }

        [Fact]
        public async void Delete_ShouldNotDeleteObject_WrongIdFormat()
        {
            // Arrange
            var id = "aaaaa22222";
            var geolocationRepository = Substitute.For<IGeolocationRepository>();
            var geolocationDbRepository = Substitute.For<IGeolocationDbRepository>();

            var geolocationManager = new GeolocationManager(geolocationRepository, geolocationDbRepository);

            // Act
            var result = await geolocationManager.Delete(id);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal(ErrorMessages.WRONG_ID_FORMAT, result.Message);

            await geolocationDbRepository.DidNotReceiveWithAnyArgs().Get(Arg.Any<int>());
            geolocationDbRepository.DidNotReceiveWithAnyArgs().Delete(Arg.Any<Geolocation>());
        }

        [Fact]
        public async void Delete_ShouldNotDeleteObject_NotExistingValue()
        {
            // Arrange
            var id = "666";
            var geolocationRepository = Substitute.For<IGeolocationRepository>();
            var geolocationDbRepository = Substitute.For<IGeolocationDbRepository>();
            Geolocation geolocation = null;

            geolocationDbRepository.Get(Arg.Any<int>()).Returns(geolocation);

            var geolocationManager = new GeolocationManager(geolocationRepository, geolocationDbRepository);

            // Act
            var result = await geolocationManager.Delete(id);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal(ErrorMessages.NOT_EXISTS, result.Message);

            await geolocationDbRepository.Received().Get(Arg.Any<int>());
            geolocationDbRepository.DidNotReceiveWithAnyArgs().Delete(Arg.Any<Geolocation>());
        }

        [Fact]
        public async void Delete_ShouldNotDeleteObject_DbError()
        {
            // Arrange
            var id = "666";
            var geolocationRepository = Substitute.For<IGeolocationRepository>();
            var geolocationDbRepository = Substitute.For<IGeolocationDbRepository>();

            var results = new Results<Geolocation>(new Geolocation()).Then(new Geolocation());
            geolocationDbRepository.Get(Arg.Any<int>()).Returns(x => results.Next());

            var geolocationManager = new GeolocationManager(geolocationRepository, geolocationDbRepository);

            // Act
            var result = await geolocationManager.Delete(id);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal(ErrorMessages.DB_ERROR, result.Message);

            await geolocationDbRepository.Received(2).Get(Arg.Any<int>());
            geolocationDbRepository.Received().Delete(Arg.Any<Geolocation>());
        }

        [Fact]
        public async void Delete_ShouldDeleteObject_ValidData()
        {
            // Arrange
            var id = "666";
            var geolocationRepository = Substitute.For<IGeolocationRepository>();
            var geolocationDbRepository = Substitute.For<IGeolocationDbRepository>();
            Geolocation geolocation = null;

            var results = new Results<Geolocation>(new Geolocation()).Then(geolocation);
            geolocationDbRepository.Get(Arg.Any<int>()).Returns(x => results.Next());

            var geolocationManager = new GeolocationManager(geolocationRepository, geolocationDbRepository);

            // Act
            var result = await geolocationManager.Delete(id);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(ErrorMessages.DELETED, result.Message);

            await geolocationDbRepository.Received(2).Get(Arg.Any<int>());
            geolocationDbRepository.Received().Delete(Arg.Any<Geolocation>());
        }
    }
}

using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BikeStore.Core.Services;
using BikeStoreWebApi.Controllers;
using BikeStoreWebApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Unit.Controllers
{
    [TestFixture]
    public class BikeControllerTests
    {
        private IFixture _fixture;
        private Mock<IBikeService> _bikeServiceMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void Setup()
        {
            _fixture = CreateFixture();
            _bikeServiceMock = _fixture.Freeze<Mock<IBikeService>>();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
        }

        [Test]
        public async Task GetAllBikes_ReturnsOk()
        {
            //Arrange
            var returnedCollection = _fixture.Create<IEnumerable<BikeDto>>();

            _bikeServiceMock.Setup(service => service.GetAllBikesWithCategoryAndBrand()).ReturnsAsync(returnedCollection);

            var controller = new BikesController(_bikeServiceMock.Object, _mapperMock.Object);

            //Act
            var result = (await controller.GetAllBikes()).Result as OkObjectResult;

            //Assert
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(200));

            _bikeServiceMock.Verify(service => service.GetAllBikesWithCategoryAndBrand(), Times.Once);
        }

        [Test]
        public async Task GetBikeById_ReturnsOk()
        {
            //Arrange
            var testId = _fixture.Create<int>();
            _fixture.Customize<BikeDto>(bike => bike.With(b => b.BikeId, testId));
            var returnedBikeDto = _fixture.Create<BikeDto>();

            _bikeServiceMock.Setup(service => service.GetBikeWithCategoryAndBrand(testId)).ReturnsAsync(returnedBikeDto); ;

            var controller = new BikesController(_bikeServiceMock.Object, _mapperMock.Object);

            //Act
            var result = (await controller.GetBikeById(testId)).Result as OkObjectResult;

            //Assert
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(((BikeDto)result.Value).BikeId, Is.EqualTo(testId));
            Assert.That(result.StatusCode, Is.EqualTo(200));

            _bikeServiceMock.Verify(service => service.GetBikeWithCategoryAndBrand(testId), Times.Once);
        }

        [Test]
        public async Task GetBikeById_ReturnsBadRequest()
        {
            //Arrange
            var testId = 0;
            var controller = new BikesController(_bikeServiceMock.Object, _mapperMock.Object);

            //Act
            var result = (await controller.GetBikeById(testId)).Result as BadRequestResult;

            //Assert
            Assert.That(result.StatusCode, Is.EqualTo(400));
        }

        [Test]
        public async Task GetBikeById_ReturnsNotFound()
        {
            //Arrange
            var testId = _fixture.Create<int>();

            _bikeServiceMock.Setup(service => service.GetBikeWithCategoryAndBrand(testId)).ReturnsAsync(It.IsAny<BikeDto>);

            var controller = new BikesController(_bikeServiceMock.Object, _mapperMock.Object);

            //Act
            var result = (await controller.GetBikeById(testId)).Result as NotFoundResult;

            //Assert
            Assert.That(result.StatusCode, Is.EqualTo(404));

            _bikeServiceMock.Verify(service => service.GetBikeWithCategoryAndBrand(testId), Times.Once);
        }

        [Test]
        public async Task CreateBike_ReturnsCreatedBike()
        {
            //Arrange
            var testSaveBikeDto = _fixture.Create<SaveBikeDto>();
            var testBikeDto = _fixture.Create<BikeDto>();

            _bikeServiceMock.Setup(service => service.CreateBike(testSaveBikeDto)).ReturnsAsync(testBikeDto);

            var controller = new BikesController(_bikeServiceMock.Object, _mapperMock.Object);

            //Act
            var result = (await controller.CreateBike(testSaveBikeDto)).Result as CreatedAtActionResult;

            //Assert
            Assert.That(result.ActionName, Is.EqualTo(nameof(BikesController.CreateBike)));
            Assert.That(((BikeDto)result.Value).BikeId, Is.EqualTo(testBikeDto.BikeId));
            Assert.That(result.StatusCode, Is.EqualTo(201));

            _bikeServiceMock.Verify(service => service.CreateBike(testSaveBikeDto), Times.Once);
        }

        private static IFixture CreateFixture()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            fixture.Behaviors.Remove(fixture.Behaviors.FirstOrDefault(x => x.GetType() == typeof(ThrowingRecursionBehavior)));
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            return fixture;
        }
    }
}

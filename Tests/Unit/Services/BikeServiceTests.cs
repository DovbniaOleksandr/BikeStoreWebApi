using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BikeStore.Core.Models;
using BikeStore.Core.Services;
using BikeStore.Services;
using BikeStoreEF;
using BikeStoreWebApi.DTOs;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Unit.Services
{
    [TestFixture]
    public class BikeServiceTests
    {
        private IFixture _fixture;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        private Mock<IMapper> _mapperMock;

        [SetUp]
        public void Setup()
        {
            _fixture = CreateFixture();
            _mapperMock = _fixture.Freeze<Mock<IMapper>>();
            _unitOfWorkMock = _fixture.Freeze<Mock<IUnitOfWork>>();
        }

        [Test]
        public async Task DeleteBike_MapBikeDtoAndDelete()
        {
            //Arrange
            var bikeToDelete = _fixture.Create<Bike>();
            _unitOfWorkMock.Setup(map => map.Bikes.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(bikeToDelete);
            _unitOfWorkMock.Setup(map => map.Bikes.Remove(bikeToDelete));
            _unitOfWorkMock.Setup(map => map.SaveAsync());

            var service = _fixture.Create<BikeService>();

            //Act
            await service.DeleteBike(bikeToDelete);

            //Assert
            _unitOfWorkMock.Verify(map => map.Bikes.GetByIdAsync(It.IsAny<Guid>()), Times.Once);
            _unitOfWorkMock.Verify(map => map.Bikes.Remove(bikeToDelete), Times.Once);
            _unitOfWorkMock.Verify(map => map.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task CreateBike_ReturnsCreatedBike()
        {
            //Arrange
            var saveBikeDto = _fixture.Create<SaveBikeDto>();
            var bike = _fixture.Create<Bike>();
            var bikeDto = _fixture.Create<BikeDto>();

            _mapperMock.Setup(map => map.Map<SaveBikeDto, Bike>(saveBikeDto)).Returns(bike);
            _unitOfWorkMock.Setup(map => map.Bikes.AddAsync(bike));
            _unitOfWorkMock.Setup(map => map.SaveAsync());
            _mapperMock.Setup(map => map.Map<Bike, BikeDto>(bike)).Returns(bikeDto);

            var service = _fixture.Create<BikeService>();

            //Act
            var result = await service.CreateBike(saveBikeDto);

            //Assert
            Assert.That(result, Is.EqualTo(bikeDto));

            _mapperMock.Verify(map => map.Map<SaveBikeDto, Bike>(saveBikeDto), Times.Once);
            _mapperMock.Verify(map => map.Map<Bike, BikeDto>(bike), Times.Once);
            _unitOfWorkMock.Verify(map => map.Bikes.AddAsync(bike), Times.Once);
            _unitOfWorkMock.Verify(map => map.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateBike_ReturnsTrue()
        {
            //Arrange
            var bike = _fixture.Create<SaveBikeDto>();
            var bikeToBeUpdated = _fixture.Create<Bike>();
            var id = _fixture.Create<Guid>();

            _unitOfWorkMock.Setup(map => map.Bikes.GetWithBrandAndCategoryByIdAsync(id)).ReturnsAsync(bikeToBeUpdated);
            _unitOfWorkMock.Setup(map => map.SaveAsync());

            var service = _fixture.Create<BikeService>();

            //Act
            var result = await service.UpdateBike(id, bike);

            //Assert (Check equality of all properties)
            Assert.That(bikeToBeUpdated.BrandId, Is.EqualTo(bike.BrandId));

            _unitOfWorkMock.Verify(map => map.Bikes.GetWithBrandAndCategoryByIdAsync(id), Times.Once);
            _unitOfWorkMock.Verify(map => map.SaveAsync(), Times.Once);
        }

        [Test]
        public async Task UpdateBike_ArgumentException()
        {
            //Arrange
            var id = _fixture.Create<Guid>();
            var service = _fixture.Create<BikeService>();

            //Act
            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await service.UpdateBike(id, null));
        }

        [Test]
        public async Task UpdateBike_ReturnsFalse()
        {
            //Arrange
            var bike = _fixture.Create<SaveBikeDto>();
            var id = _fixture.Create<Guid>();

            _unitOfWorkMock.Setup(map => map.Bikes.GetWithBrandAndCategoryByIdAsync(id)).ReturnsAsync((Bike) null);

            var service = _fixture.Create<BikeService>();

            //Act
            var result = await service.UpdateBike(id, bike);

            //Assert
            Assert.That(result, Is.False);

            _unitOfWorkMock.Verify(map => map.Bikes.GetWithBrandAndCategoryByIdAsync(id), Times.Once);
        }

        [Test]
        public async Task GetAllBikes_ReturnsBikes()
        {
            //Arrange
            var returnedBikesDto = _fixture.Create<List<BikeDto>>();

            _mapperMock
                .Setup(map => map.Map<IEnumerable<Bike>, IEnumerable<BikeDto>>(It.IsAny<IEnumerable<Bike>>()))
                .Returns(returnedBikesDto);

            var service = _fixture.Create<BikeService>();

            //Act
            var result = await service.GetAllBikes();

            //Assert
            Assert.That(result, Is.EqualTo(returnedBikesDto));

            _unitOfWorkMock.Verify(map => map.Bikes.GetAllAsync(), Times.Once);
            _mapperMock.Verify(map => map.Map<IEnumerable<Bike>, IEnumerable<BikeDto>>(It.IsAny<IEnumerable<Bike>>()), Times.Once);
        }

        [Test]
        public async Task GetAllBikesWithCategoryAndBrand_ReturnsBikesWithCategoryAndBrand()
        {
            //Arrange
            var returnedBikesDto = _fixture.Create<List<BikeDto>>();

            _mapperMock
                .Setup(map => map.Map<IEnumerable<Bike>, IEnumerable<BikeDto>>(It.IsAny<IEnumerable<Bike>>()))
                .Returns(returnedBikesDto);

            var service = _fixture.Create<BikeService>();

            //Act
            var result = await service.GetAllBikesWithCategoryAndBrand();

            //Assert
            Assert.That(result, Is.EqualTo(returnedBikesDto));

            _unitOfWorkMock.Verify(map => map.Bikes.GetAllWithBrandAndCategoryAsync(), Times.Once);
            _mapperMock.Verify(map => map.Map<IEnumerable<Bike>, IEnumerable<BikeDto>>(It.IsAny<IEnumerable<Bike>>()), Times.Once);
        }

        [Test]
        public void GetBikesByCategory_ReturnsBikeWithThatCategory()
        {
            //Arrange
            var category = _fixture.Create<string>();

            var returnedBikesDto = _fixture.Create<List<BikeDto>>();

            _mapperMock
                .Setup(map => map.Map<IEnumerable<Bike>, IEnumerable<BikeDto>>(It.IsAny<IEnumerable<Bike>>()))
                .Returns(returnedBikesDto);

            var service = _fixture.Create<BikeService>();

            //Act
            var result = service.GetBikesByCategory(category);

            //Assert
            Assert.That(result, Is.EqualTo(returnedBikesDto));

            _unitOfWorkMock.Verify(map => map.Bikes.Find(x => x.Category.Name == category), Times.Once);
            _mapperMock.Verify(map => map.Map<IEnumerable<Bike>, IEnumerable<BikeDto>>(It.IsAny<IEnumerable<Bike>>()), Times.Once);
        }

        [Test]
        public void GetBikesByBrand_ReturnsBikeWithThatBrand()
        {
            //Arrange
            var brand = _fixture.Create<string>();

            var returnedBikesDto = _fixture.Create<List<BikeDto>>();

            _mapperMock
                .Setup(map => map.Map<IEnumerable<Bike>, IEnumerable<BikeDto>>(It.IsAny<IEnumerable<Bike>>()))
                .Returns(returnedBikesDto);

            var service = _fixture.Create<BikeService>();

            //Act
            var result = service.GetBikesByBrand(brand);

            //Assert
            Assert.That(result, Is.EqualTo(returnedBikesDto));

            _unitOfWorkMock.Verify(map => map.Bikes.Find(x => x.Brand.BrandName == brand), Times.Once);
            _mapperMock.Verify(map => map.Map<IEnumerable<Bike>, IEnumerable<BikeDto>>(It.IsAny<IEnumerable<Bike>>()), Times.Once);
        }

        [Test]
        public async Task FilterBikes__ReturnsBikeByFilter()
        {
            //Arrange
            var filters = _fixture.Create<BikeFilters>();

            var returnedBikesDto = _fixture.Create<List<BikeDto>>();

            _mapperMock
                .Setup(map => map.Map<IEnumerable<Bike>, IEnumerable<BikeDto>>(It.IsAny<IEnumerable<Bike>>()))
                .Returns(returnedBikesDto);

            var service = _fixture.Create<BikeService>();

            //Act
            var result = await service.FilterBikes(filters);

            //Assert
            Assert.That(result, Is.EqualTo(returnedBikesDto));

            _unitOfWorkMock.Verify(map => map.Bikes.FindAllWithBrandAndCategoryAsync(b =>
                (filters.Categories.Contains(b.CategoryId) || !filters.Categories.Any()) &&
                (filters.Brands.Contains(b.BrandId) || !filters.Brands.Any()) &&
                (b.Name.Contains(filters.Name) || filters.Name == null) &&
                (b.ModelYear == filters.ModelYear || filters.ModelYear == null) &&
                (b.Price >= filters.MinPrice || filters.MinPrice == null) &&
                (b.Price <= filters.MaxPrice || filters.MaxPrice == null)), Times.Once);

            _mapperMock.Verify(map => map.Map<IEnumerable<Bike>, IEnumerable<BikeDto>>(It.IsAny<IEnumerable<Bike>>()), Times.Once);
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

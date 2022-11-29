using BikeStore.Core.Models;
using BikeStore.Core.Repositories;
using BikeStore.DAL;
using BikeStore.DAL.Repositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Helpers;

namespace Tests.Unit.Repositories
{
    [TestFixture]
    public class BikeRepositoryTests
    {
        private BikeStoreDBContext _context;
        private IBikeRepository _bikeRepository;

        [SetUp]
        public void Setup()
        {
            _context = Utilities.GetInMemoryDBContext();
            _bikeRepository = new BikeRepository(_context);
        }

        [Test]
        public async Task GetAllWithBrandAndCategoryAsync()
        {
            //Act
            var result = await _bikeRepository.GetAllWithBrandAndCategoryAsync();

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(result.Count(), 3);
            Assert.That(result.All(b => b.Category != null));
            Assert.That(result.All(b => b.Brand != null));
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public async Task GetWithBrandAndCategoryByIdAsync_ReturnsBikeWithId(int id)
        {
            //Act
            var result = await _bikeRepository.GetWithBrandAndCategoryByIdAsync(id);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.AreEqual(result.BikeId, id);
            Assert.That(result.Brand, Is.Not.Null);
            Assert.That(result.Category, Is.Not.Null);
        }

        [Test]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        public async Task GetWithBrandAndCategoryByIdAsync_ReturnsNull(int id)
        {
            //Act
            var result = await _bikeRepository.GetWithBrandAndCategoryByIdAsync(id);

            //Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task Remove_DeleteBike()
        {
            //Arrange
            var bikeToDelete = _context.Bikes.FirstOrDefault();

            //Act
            _bikeRepository.Remove(bikeToDelete);
            await _context.SaveChangesAsync();

            //Assert
            Assert.That(_context.Bikes, Is.All.Matches<Bike>(b => b.BikeId != bikeToDelete.BikeId));
            Assert.AreEqual(_context.Bikes.Count(), 2);
        }

        [Test]
        [TestCase(1)]
        [TestCase(2)]
        public async Task FindAllWithBrandAndCategoryAsync_FindByCategory(int categoryId)
        {
            //Act
            var result = await _bikeRepository.FindAllWithBrandAndCategoryAsync(b => b.CategoryId == categoryId);

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.All.Matches<Bike>(b => b.CategoryId == categoryId));
        }

        [Test]
        [TestCase(5)]
        [TestCase(6)]
        public async Task FindAllWithBrandAndCategoryAsync_DoNotFindByCategory(int categoryId)
        {
            //Act
            var result = await _bikeRepository.FindAllWithBrandAndCategoryAsync(b => b.CategoryId == categoryId);

            //Assert
            Assert.That(result, Is.Empty);
        }
    }
}

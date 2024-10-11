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
            Assert.Equals(result.Count(), 3);
            Assert.That(result.All(b => b.Category != null));
            Assert.That(result.All(b => b.Brand != null));
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
            Assert.That(_context.Bikes, Is.All.Matches<Bike>(b => b.Id != bikeToDelete.Id));
            Assert.Equals(_context.Bikes.Count(), 2);
        }
    }
}

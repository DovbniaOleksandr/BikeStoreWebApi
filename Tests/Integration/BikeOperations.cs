using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using BikeStore.Core.Models;
using BikeStore.Core.Services;
using BikeStore.Services;
using BikeStoreEF;
using BikeStoreWebApi.DTOs;
using Microsoft.Extensions.Hosting;
using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using BikeStoreWebApi;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using Tests.Helpers;
using System.Transactions;
using Microsoft.Extensions.DependencyInjection;
using BikeStore.DAL;

namespace Tests.Integration
{
    [TestFixture]
    public class BikeOperations
    {
        private WebApplicationFactory<Program> _applicationFactory;

        [OneTimeSetUp]
        public void Setup()
        {
            _applicationFactory = new BikeApplication();
        }


        [OneTimeTearDown]
        public void TearDown()
        {
            _applicationFactory.Dispose();
        }

        [Test]
        public async Task Post_CreateBike_Created()
        {
            //Arrange
            var bikeDto = new SaveBikeDto()
            {
                Name = "Best Bike",
                BrandId = 1,
                CategoryId = 2,
                ModelYear = 2000,
                Price = 1000,
                Description = "Post_CreateBike_Response_OK"
            };

            var json = JsonConvert.SerializeObject(bikeDto);
            using var message = new StringContent(json);

            message.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            using var client = _applicationFactory.CreateClient();

            //Act
            using var response = await client.PostAsync("/api/Bikes", message);

            //Assert
            var responseContent = await response.Content.ReadAsStringAsync();
            var bikeResponse = JsonConvert.DeserializeObject<BikeDto>(responseContent);

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(bikeResponse.BikeId, Is.Not.EqualTo(null));
        }

        [Test]
        public async Task Post_CreateBike_BadRequest()
        {
            //Arrange
            var bikeDto = new SaveBikeDto()
            {
                Name = "Best Bike",
                ModelYear = 2000,
                Price = 1000,
                Description = "Post_CreateBike_Response_OK"
            };

            var json = JsonConvert.SerializeObject(bikeDto);
            using var message = new StringContent(json);

            message.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");

            using var client = _applicationFactory.CreateClient();

            //Act
            using var response = await client.PostAsync("/api/Bikes", message);

            //Assert
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(responseContent, Does.Contain("Category Id' must not be 0."));
            Assert.That(responseContent, Does.Contain("Brand Id' must not be 0."));
        }

        [Test]
        [TestCase(1)]
        public async Task Get_GetBikeById_Ok(int id)
        {
            //Arrange
            using var client = _applicationFactory.CreateClient();

            //Act
            using var response = await client.GetAsync($"/api/Bikes/{id}");

            //Assert
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var bikeResponse = JsonConvert.DeserializeObject<BikeDto>(responseContent);

            Assert.That(bikeResponse.BikeId, Is.EqualTo(id));
        }

        [Test]
        [TestCase(0)]
        public async Task Get_GetBikeById_BadRequest(int id)
        {
            //Arrange
            using var client = _applicationFactory.CreateClient();

            //Act
            using var response = await client.GetAsync($"/api/Bikes/{id}");

            //Assert
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        public async Task Get_GetBikeById_NotFound(int id)
        {
            //Arrange
            using var client = _applicationFactory.CreateClient();

            //Act
            using var response = await client.GetAsync($"/api/Bikes/{id}");

            //Assert
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        [TestCase(5)]
        [TestCase(6)]
        [TestCase(7)]
        public async Task Delete_DeleteBike_NotFound(int id)
        {
            //Arrange
            using var client = _applicationFactory.CreateClient();

            //Act
            using var response = await client.DeleteAsync($"/api/Bikes/{id}");

            //Assert
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        [TestCase(0)]
        public async Task Delete_DeleteBike_BadRequest(int id)
        {
            //Arrange
            using var client = _applicationFactory.CreateClient();

            //Act
            using var response = await client.DeleteAsync($"/api/Bikes/{id}");

            //Assert
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Test]
        [TestCase(2)]
        [TestCase(3)]
        public async Task Delete_DeleteBike_NoContent(int id)
        {
            //Arrange
            using var client = _applicationFactory.CreateClient();

            //Act
            using var response = await client.DeleteAsync($"/api/Bikes/{id}");

            //Assert
            var responseContent = await response.Content.ReadAsStringAsync();

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
        }
    }
}

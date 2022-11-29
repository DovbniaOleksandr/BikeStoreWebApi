using BikeStore.Core.Models;
using BikeStore.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Helpers
{
    public static class Utilities
    {
        public static void InitDBForTest(BikeStoreDBContext dbContext)
        {
            dbContext.AddRange(GetTestBikes());
            dbContext.AddRange(GetTestBrands());
            dbContext.AddRange(GetTestCategories());

            dbContext.SaveChanges();
        }

        public static void ReinitializeDBForTest(BikeStoreDBContext dbContext)
        {
            dbContext.Bikes.RemoveRange(dbContext.Bikes);
            dbContext.Brands.RemoveRange(dbContext.Brands);
            dbContext.Categories.RemoveRange(dbContext.Categories);

            InitDBForTest(dbContext);
        }

        private static List<Category> GetTestCategories()
        {
            return new List<Category>()
            {
                new Category()
                {
                    Name = "Children Bicycles"
                },
                new Category()
                {
                    Name = "Comfort Bicycles"
                }
            };
        }

        private static List<Brand> GetTestBrands()
        {
            return new List<Brand>()
            {
                new Brand()
                {
                    BrandName = "Electra"
                },
                new Brand()
                {
                    BrandName = "Haro"
                }
            };
        }

        private static List<Bike> GetTestBikes()
        {
            return new List<Bike>()
            {
                new Bike()
                {
                    Name = "Best Bike In The World",
                    BrandId = 1,
                    CategoryId = 2,
                    ModelYear = 2001,
                    Price = 2001,
                    Description = "Test 1"
                },
                new Bike()
                {
                    Name = "Best Bike In The World 2",
                    BrandId = 1,
                    CategoryId = 1,
                    ModelYear = 2002,
                    Price = 2002,
                    Description = "Test 2"
                },
                new Bike()
                {
                    Name = "Best Bike In The World 3",
                    BrandId = 2,
                    CategoryId = 2,
                    ModelYear = 2002,
                    Price = 2003,
                    Description = "Test 3"
                }
            };
        }

        public static BikeStoreDBContext GetInMemoryDBContext()
        {
            var options = new DbContextOptionsBuilder<BikeStoreDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;

            var context = new BikeStoreDBContext(options);

            InitDBForTest(context);

            return context;
        }
    }
}

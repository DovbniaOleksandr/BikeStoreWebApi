using BikeStore.Core.Models;
using BikeStore.DAL.Configurations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BikeStore.DAL
{
    public class BikeStoreDBContext: DbContext
    {
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }

        public BikeStoreDBContext(DbContextOptions<BikeStoreDBContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfiguration(new BikeConfigurations());

            modelBuilder
                .ApplyConfiguration(new CategoryConfiguration());

            modelBuilder
                .ApplyConfiguration(new BrandConfigurations());

            modelBuilder
                .Seed();
        }
    }

    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>().HasData(new List<Brand>()
            {
                new Brand()
                {
                    BrandId = 1,
                    BrandName = "Electra"
                },
                new Brand()
                {
                    BrandId = 2,
                    BrandName = "Haro"
                },
                new Brand()
                {
                    BrandId = 3,
                    BrandName = "Heller"
                },
                new Brand()
                {
                    BrandId = 4,
                    BrandName = "Pure Cycles"
                },
                new Brand()
                {
                    BrandId = 5,
                    BrandName = "Ritchey"
                },
                new Brand()
                {
                    BrandId = 6,
                    BrandName = "Strider"
                },
                new Brand()
                {
                    BrandId = 7,
                    BrandName = "Sun Bicycles"
                },
                new Brand()
                {
                    BrandId = 8,
                    BrandName = "Surly"
                },
                new Brand()
                {
                    BrandId = 9,
                    BrandName = "Trek"
                }
            });
            modelBuilder.Entity<Category>().HasData(new List<Category>()
            {
                new Category()
                {
                    CategoryId = 1,
                    Name = "Children Bicycles"
                },
                new Category()
                {
                    CategoryId = 2,
                    Name = "Comfort Bicycles"
                },
                new Category()
                {
                    CategoryId = 3,
                    Name = "Cruisers Bicycles"
                },
                new Category()
                {
                    CategoryId = 4,
                    Name = "Cyclocross Bicycles"
                },
                new Category()
                {
                    CategoryId = 5,
                    Name = "Electric Bikes"
                },
                new Category()
                {
                    CategoryId = 6,
                    Name = "Mountain Bikes"
                },
                new Category()
                {
                    CategoryId = 7,
                    Name = "Road Bikes"
                }
            });
        }
    }
}

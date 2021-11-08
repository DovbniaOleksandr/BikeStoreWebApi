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
        }
    }
}

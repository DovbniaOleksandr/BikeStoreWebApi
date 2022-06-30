using BikeStore.Core.Models;
using BikeStore.DAL.Configurations;
using BikeStore.DAL.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BikeStore.DAL
{
    public class BikeStoreDBContext: IdentityDbContext<User, Role, int>
    {
        public DbSet<Bike> Bikes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Order> Orders { get; set; }

        public BikeStoreDBContext(DbContextOptions<BikeStoreDBContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base
                .OnModelCreating(modelBuilder);

            modelBuilder
                .ApplyConfiguration(new BikeConfigurations());

            modelBuilder
                .ApplyConfiguration(new CategoryConfiguration());

            modelBuilder
                .ApplyConfiguration(new BrandConfigurations());

            modelBuilder
                .ApplyConfiguration(new OrderConfigurations());

            modelBuilder
                .Seed();
        }
    }
}

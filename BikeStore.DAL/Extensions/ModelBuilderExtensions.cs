using BikeStore.Core.Enums;
using BikeStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BikeStore.DAL.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>().HasData(new List<Brand>()
            {
                new Brand()
                {
                    Id = Guid.NewGuid(),
                    BrandName = "Electra"
                },
                new Brand()
                {
                    Id = Guid.NewGuid(),
                    BrandName = "Haro"
                },
                new Brand()
                {
                    Id = Guid.NewGuid(),
                    BrandName = "Heller"
                },
                new Brand()
                {
                    Id = Guid.NewGuid(),
                    BrandName = "Pure Cycles"
                },
                new Brand()
                {
                    Id = Guid.NewGuid(),
                    BrandName = "Ritchey"
                },
                new Brand()
                {
                    Id = Guid.NewGuid(),
                    BrandName = "Strider"
                },
                new Brand()
                {
                    Id = Guid.NewGuid(),
                    BrandName = "Sun Bicycles"
                },
                new Brand()
                {
                    Id = Guid.NewGuid(),
                    BrandName = "Surly"
                },
                new Brand()
                {
                    Id = Guid.NewGuid(),
                    BrandName = "Trek"
                }
            });

            modelBuilder.Entity<Category>().HasData(new List<Category>()
            {
                new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = "Children Bicycles"
                },
                new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = "Comfort Bicycles"
                },
                new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = "Cruisers Bicycles"
                },
                new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = "Cyclocross Bicycles"
                },
                new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = "Electric Bikes"
                },
                new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = "Mountain Bikes"
                },
                new Category()
                {
                    Id = Guid.NewGuid(),
                    Name = "Road Bikes"
                }
            });

            modelBuilder.Entity<Role>().HasData(new List<Role>()
            {
                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = Roles.Admin,
                    NormalizedName = Roles.Admin.ToUpper()
                },

                new Role
                {
                    Id = Guid.NewGuid(),
                    Name = Roles.User,
                    NormalizedName = Roles.User.ToUpper()
                }
            });
        }
    }
}

using BikeStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BikeStore.DAL.Configurations
{
    public class BrandConfigurations : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder
                .ToTable("Brands");

            builder
                .HasKey(b => b.BrandId);

            builder
                .Property(e => e.BrandId)
                .UseIdentityColumn();

            builder
                .Property(b => b.BrandName)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder
                .HasMany(b => b.Bikes)
                .WithOne(b => b.Brand);
        }
    }
}

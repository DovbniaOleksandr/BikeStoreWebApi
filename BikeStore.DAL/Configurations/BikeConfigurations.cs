using BikeStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BikeStore.DAL.Configurations
{
    public class BikeConfigurations : IEntityTypeConfiguration<Bike>
    {
        public void Configure(EntityTypeBuilder<Bike> builder)
        {
            builder.ToTable("Bikes");

            builder
                .HasKey(b => b.Id);

            builder
                .Property(b => b.Id);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.HasOne(b => b.Brand)
                .WithMany(b => b.Bikes)
                .HasForeignKey(d => d.BrandId);

            builder.HasOne(b => b.Category)
                .WithMany(c => c.Bikes)
                .HasForeignKey(b => b.CategoryId);

            builder.Property(b => b.Description)
                .HasMaxLength(150)
                .IsUnicode(false);
        }
    }
}

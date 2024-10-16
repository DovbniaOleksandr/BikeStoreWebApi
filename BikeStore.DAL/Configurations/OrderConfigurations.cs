﻿using BikeStore.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BikeStore.DAL.Configurations
{
    class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder
                .HasKey(b => b.Id);

            builder
                .Property(b => b.Id);

            builder
                .Property(b => b.CreatedAt)
                .IsRequired();

            builder
                .Property(b => b.BikeId)
                .IsRequired();

            builder
                .Property(b => b.UserId)
                .IsRequired();

            builder
                .Property(b => b.IsCompleted)
                .IsRequired();
        }
    }
}

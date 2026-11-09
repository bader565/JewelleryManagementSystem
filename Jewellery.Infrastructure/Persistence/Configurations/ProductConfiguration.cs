using Jewellery.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jewellery.Infrastructure.Persistence.Configurations
{
    public class ProductConfiguration :
     IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Description)
                .HasMaxLength(1000);

            builder.Property(p => p.Barcode)
                .HasMaxLength(100);

            builder.Property(p => p.Karat)
                .HasConversion<int>();

            builder.Property(p => p.Weight)
                .HasPrecision(18, 3);

            builder.Property(p => p.PurchasePrice)
                .HasPrecision(18, 2);

            builder.Property(p => p.SellingPrice)
                .HasPrecision(18, 2);

            builder.Property(p => p.Quantity)
                .IsRequired();
        }
    }
}


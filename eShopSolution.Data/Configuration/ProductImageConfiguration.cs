﻿using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configuration
{
    public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
    {
        public void Configure(EntityTypeBuilder<ProductImage> builder)
        {
            builder.ToTable("ProductImages");
            builder.HasKey(x=>x.Id);
            builder.Property(x=>x.Id).UseIdentityColumn();
            builder.Property(x => x.Caption).HasMaxLength(200);
            builder.Property(x => x.ImagePath).IsRequired().HasMaxLength(250);
            builder.Property(x => x.SortOrder).UseIdentityColumn();
            builder.HasOne(x=>x.Product).WithMany(x=>x.ProductImages).HasForeignKey(x=>x.ProductId);
        }
    }
}

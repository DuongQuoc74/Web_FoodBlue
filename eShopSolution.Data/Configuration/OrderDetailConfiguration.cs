using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configuration
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.ToTable("OderDetails");
            builder.HasKey(x => new { x.OderId, x.ProductId });
            builder.Property(x=>x.Id).UseIdentityColumn();
            builder.HasOne(x => x.Order).WithMany(x => x.OrderDetails).HasForeignKey(x => x.OderId);
            builder.HasOne(x => x.Product).WithMany(x => x.OrderDetail).HasForeignKey(x => x.ProductId);
        }
    }
}

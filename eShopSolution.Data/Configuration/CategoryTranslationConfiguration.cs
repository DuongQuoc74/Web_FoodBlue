using eShopSolution.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Configuration
{
    public class CategoryTranslationConfiguration : IEntityTypeConfiguration<CategoryTranslation>
    {
        public void Configure(EntityTypeBuilder<CategoryTranslation> builder)
        {
            builder.ToTable("CategoryTranslations");
            builder.HasKey(x => x.Id);
            builder.Property(x=>x.Id).UseIdentityColumn();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x=>x.Description).HasMaxLength(200);

            builder.HasOne(x => x.Category).WithMany(x => x.CategoryTranslations).HasForeignKey(x => x.CategoryId);
            builder.HasOne(x=>x.Language).WithMany(x=>x.CategoryTranslations).HasForeignKey(x => x.LanguageId);
        }
    }
}

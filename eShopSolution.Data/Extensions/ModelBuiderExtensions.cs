using eShopSolution.Data.Entities;
using eShopSolution.Data.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Data.Extensions
{
    public static class ModelBuiderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = "vi", Name = "Tiếng Việt", IsDefault = true },
                new Language() { Id = "en", Name = "Tiếng Anh", IsDefault = false });

            modelBuilder.Entity<Category>().HasData(
                new Category() { Id = 1, SortOder = 1, IsShowHome = true, ParenId = null, Status = Status.Active },
                new Category() { Id = 2, SortOder = 2, IsShowHome = true, ParenId = null, Status = Status.Active });

            modelBuilder.Entity<CategoryTranslation>().HasData(
                new CategoryTranslation() { Id = 1, LanguageId = "vi", CategoryId = 1, Name = "Thịt heo", Description = "Thịt heo sạch" },
                new CategoryTranslation() { Id = 2, LanguageId = "en", CategoryId = 1, Name = "Pork", Description = "Clean pork" },
                new CategoryTranslation() { Id = 3, LanguageId = "vi", CategoryId = 2, Name = "Thịt gà", Description = "Thịt gà sạch" },
                new CategoryTranslation() { Id = 4, LanguageId = "en", CategoryId = 2, Name = "Chicken", Description = "Clean Chicken" });

            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Price = 100,OriginalPrice=50, Stock = 1, DateCreate = DateTime.Now, IsFeartured = true, ViewCout = 0 });

            modelBuilder.Entity<ProductTranslation>().HasData(
                new ProductTranslation() { Id = 1, ProductId = 1, LanguageId="vi", Name = "Thịt heo nái", Description = "Thịt heo nái sạch", Detail = "Thịt heo ở dưới quê,sạch, ngon", Title = "Thịt quê" },
                new ProductTranslation() { Id = 2, ProductId = 1, LanguageId="en", Name = "Old pig", Description = "Clean old pig", Detail = "Pork in the countryside, clean, delicious", Title = "Country meat" });

            modelBuilder.Entity<ProductInCategory>().HasData(
                new ProductInCategory() { ProductId = 1, CategoryId = 1 });

            var adminId = new Guid("1F365AFA-E7CA-42BA-B9F4-AD29A08A487B");
            var roleId = new  Guid("01A07A9D-CE50-4A34-A6C9-6D58E38E1D75");
            modelBuilder.Entity<AppRole>().HasData(
                new AppRole() { Id = roleId, Name = "admin", NormalizedName = "admin", Description = "Administrator role" });
               

            var hasher = new PasswordHasher<AppUser>();
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser() { Id = adminId, UserName = "admin",NormalizedUserName="admin",Email="duongquyquoc.dev@gmail.com",NormalizedEmail= "duongquyquoc.dev@gmail.com",EmailConfirmed=true,PhoneNumber="0387850693",PhoneNumberConfirmed=true,PasswordHash=hasher.HashPassword(null,"Admin@123"),SecurityStamp=string.Empty, FirstName="Quốc", LastName="Dương",Dob= new DateTime(2001,07,29) });

            modelBuilder.Entity<IdentityUserRole<Guid>>().HasData(new IdentityUserRole<Guid>
            {
                RoleId = roleId,
                UserId = adminId
            });

        }
    }
}

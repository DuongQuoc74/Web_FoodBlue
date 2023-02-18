using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Catalog.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Product
{
    public class ProductService : IProductService
    {
        private readonly EShopDbContext _context;
        public ProductService(EShopDbContext context) 
        { 
            _context = context;
        }
        public async Task<ProductVM> GetById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
                return new ProductVM();
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == productId && x.LanguageId == languageId);
            var productImage = await _context.ProductImages.FirstOrDefaultAsync(x => x.ProductId == productId && x.IsDefaul == true);
            var categories = await (from c in _context.Categories
                                    join ct in _context.CategoriesTranslations on c.Id equals ct.CategoryId
                                    join pic in _context.ProductInCategories on c.Id equals pic.CategoryId
                                    where pic.ProductId == productId && ct.LanguageId == languageId
                                    select ct.Name).ToListAsync();
            var data = new ProductVM()
            {
                ProductId = product != null ? product.Id : null,
                Price = product != null ? product.Price : null,
                OriginalPrice = product != null ? product.OriginalPrice : null,
                Stock = product != null ? product.Stock : null,
                ViewCout = product != null ? product.ViewCout : null,
                DateCreate = product != null ? product.DateCreate : null,

                Name = productTranslation != null ? productTranslation.Name : null,
                Title = productTranslation != null ? productTranslation.Title : null,
                Description = productTranslation!= null ? productTranslation.Description : null,
                Detail = productTranslation != null ? productTranslation.Detail : null,
                ThumbnailImage = productImage != null ? productImage.ImagePath : "np-image.jpg",

                Categories = categories,
            };
            return data;


        }
    }
}

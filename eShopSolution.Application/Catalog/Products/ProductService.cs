using eShopsolution.Utilities.Constants;
using eShopsolution.Utilities.Exceptions;
using eShopSolution.Application.Comons;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.ViewModel.Catalog.Product;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly EShopDbContext _context;
        private readonly IStorageService _storageService;
        private const string USER_CONTENT_FOLDER_NAME = "user_content";
        public ProductService(EShopDbContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task<int> CreateProduct(ProductCreateRequest request)
        {
            var lannguages = _context.Languages;
            var productTranslation = new List<ProductTranslation>();
            foreach (var language in lannguages)
            {
                if (language.Id == request.LanguageId)
                {
                    productTranslation.Add(new ProductTranslation()
                    {
                        Name = request.Name,
                        Title = request.Title,
                        Description = request.Description,
                        Detail = request.Detail,
                        LanguageId = language.Id,
                    });
                }
                else
                {
                    productTranslation.Add(new ProductTranslation()
                    {
                        Name = SystemContants.ProductCreate,
                        Description = SystemContants.ProductCreate,
                        LanguageId = language.Id,
                    });
                }
            }

            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCout = 0,
                DateCreate = DateTime.Now,
                IsFeartured = request.IsFeartured,
                ProductTranslations = productTranslation
            };

            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                 new ProductImage()
                 {
                     Caption="ThumbnailImage",
                     DateCreate= DateTime.Now,
                     IsDefaul=true,
                     SortOrder=1,
                     FileSize= request.ThumbnailImage.Length,
                     ImagePath = await this.SaveFile(request.ThumbnailImage),
                 }
                };
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product.Id;
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
                IsFeatured= product !=null ? product.IsFeartured : null,

                Name = productTranslation != null ? productTranslation.Name : null,
                Title = productTranslation != null ? productTranslation.Title : null,
                Description = productTranslation != null ? productTranslation.Description : null,
                Detail = productTranslation != null ? productTranslation.Detail : null,
                ThumbnailImage = productImage != null ? productImage.ImagePath : "np-image.jpg",

                Categories = categories,
            };
            return data;
        }
        private async Task<string> SaveFile(IFormFile file)
        {

            var originalFileName = string.Empty;
            if (file.ContentDisposition != null)
            {
                var contentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
                if (contentDisposition.FileName != null)
                {
                    originalFileName = contentDisposition.FileName.Trim('"');
                }
            }
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        public async Task<int> DeleteProduct(int ProductId)
        {
           var product = await _context.Products.FindAsync(ProductId);
            if (product == null)
                throw new EShopException("ID sản phẩm không hợp lệ!");
            var images = _context.ProductImages.Where(x => x.ProductId == ProductId);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
            }
              _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
                
        }

        public async Task<int> UpdateProduct(ProductUpdateRequest request)
        {
            var product = await _context.Products.FindAsync(request.ProductId);
            var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x=>x.ProductId== request.ProductId && x.LanguageId == request.LanguageId);
            if (product == null || productTranslation == null)
                throw new EShopException("Không tìm thấy sản phẩm!");
            product.IsFeartured = request.IsFeartured;
            productTranslation.Name= request.Name;
            productTranslation.Description= request.Description;
            productTranslation.Title= request.Title;
            productTranslation.Detail= request.Detail;

            if (request.ThumbNailImage != null)
            {
                var image= await _context.ProductImages.FirstOrDefaultAsync(x=>x.ProductId==request.ProductId);
                if(image!=null)
                {
                    image.FileSize = request.ThumbNailImage.Length;
                    image.ImagePath=await this.SaveFile(request.ThumbNailImage);
                  await  _context.SaveChangesAsync();
                }
            }
            return await _context.SaveChangesAsync();
            
        }

       
    }
}

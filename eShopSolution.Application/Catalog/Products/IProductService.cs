using eShopSolution.ViewModel.Catalog.Product;
using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Products
{
    public interface IProductService
    {
        Task<ProductVM> GetById (int id, string languageId);
        Task<int> CreateProduct(ProductCreateRequest request);
        Task<int> DeleteProduct(int ProductId);
        Task<int> UpdateProduct(ProductUpdateRequest request);
        
    }
}

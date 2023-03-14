using eShopSolution.ViewModel.Catalog.Product;
using eShopSolution.ViewModel.Common;

namespace eShopSolution.Application.Catalog.Products
{
    public interface IProductService
    {
        Task<ProductVM> GetById (int id, string languageId);
        Task<PageResult<ProductVM>> GetAllPagings(GetPagingProductRequest request );
        Task<int> CreateProduct(ProductCreateRequest request);
        Task<int> DeleteProduct(int ProductId);
        Task<int> UpdateProduct(ProductUpdateRequest request);
        
    }
}

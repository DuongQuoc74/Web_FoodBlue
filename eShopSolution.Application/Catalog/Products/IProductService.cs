using eShopSolution.ViewModel.Catalog.Product;

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

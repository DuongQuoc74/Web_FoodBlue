using eShopSolution.Application.Catalog.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopsolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService )
        {
            _productService = productService;
        }
        [HttpGet("{ProductId}/{languageId}")]
        public async Task<IActionResult> GetById(int ProductId, string languageId)
        {
            var product = await _productService.GetById(ProductId, languageId);
            if (product.ProductId == null)
                return BadRequest("Không có sản phẩm");
            return Ok(product);
        }
    }
}

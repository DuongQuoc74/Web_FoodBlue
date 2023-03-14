using eShopSolution.Application.Catalog.Products;
using eShopSolution.ViewModel.Catalog.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopsolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
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

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var productId = await _productService.CreateProduct(request);
            if (productId == 0)
                return BadRequest("Đã xảy ra lỗi!");
            if (request.LanguageId == null)
                return BadRequest("LanguageId không được trống");
            var product = await _productService.GetById(productId, request.LanguageId);
            return CreatedAtAction(nameof(GetById), new { id = productId }, product);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int ProductId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _productService.DeleteProduct(ProductId);
            if (result == 0)
                return BadRequest("Xóa sản phẩm không thành công!");
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct([FromForm] ProductUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _productService.UpdateProduct(request);
            if (result != 0)
                return BadRequest("Xóa sản phẩm thất bại!");
            if (request.LanguageId == null)
                return BadRequest("LanguageId không được để trống");

            var product = await _productService.GetById(request.ProductId, request.LanguageId);

            return Ok( product);
        }

        [HttpGet("paging")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPagings ([FromQuery] GetPagingProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _productService.GetAllPagings(request);
            return Ok( result); 
        }
    }
}

using eShopSolution.Application.Catalog.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopsolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoyService;
        public CategoriesController(ICategoryService categoyService)
        {
            _categoyService = categoyService; 
        }

        [HttpGet]
        public async Task< IActionResult> GetAll(string languageId)
        {
            var categories = await _categoyService.GetAll(languageId);
            return Ok(categories);
        }

        [HttpGet("{id}/{languageId}")]
        public async Task< IActionResult> GetById(int id, string languageId) 
        {
            var categoryId = await _categoyService.GetById(id, languageId);
            return Ok(categoryId);
        }
    }
}

using eShopSolution.Application.Systems.Languages;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopsolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguagesController : ControllerBase
    {
        private readonly ILanguageService _languageService;
        public LanguagesController(ILanguageService languageService) {
            _languageService = languageService;
        }

        [HttpGet]
        public async Task< IActionResult> GetAll()
        {
            var langugae = await _languageService.GetAll();
            return Ok(langugae);
        }
    }
}

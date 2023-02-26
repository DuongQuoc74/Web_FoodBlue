using eShopSolution.Application.Systems.Users;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace eShopsolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService) 
        {
            _userService = userService;
        }

        [HttpPost("AuthenticateAdmin")]
        [AllowAnonymous]
        public async Task<IActionResult> AutheticateAdmin([FromBody]LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _userService.AuthenticateAdmin(request);
         
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromForm] RegisterRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _userService.Register(request);
            if(result.IsSuccessed==false)
                return BadRequest(result.Message);
            return Ok(result);
        }

        [HttpGet("pading")]
        public async Task<IActionResult> GetPadingUser([FromQuery] PadingRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _userService.GetPadingRequest(request);
            return Ok(result);
        }
    }
}

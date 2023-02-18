﻿using eShopSolution.Application.Systems.Users;
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
            if (string.IsNullOrEmpty(result.ResultObj))
                return BadRequest(result);
            return Ok(result);
        }
    }
}
﻿using eShopSolution.ApiItergration;
using eShopSolution.ViewModel.System.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using eShopsolution.Utilities.Constants;
using Microsoft.AspNetCore.Authorization;

namespace eShopSolution.AdminApp.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
        private readonly ILoginApiClient _loginApiClient;
        private readonly IConfiguration _configuration;
        public LoginController(ILoginApiClient loginApiClient, IConfiguration configuration) 
        { 
            _loginApiClient= loginApiClient;
            _configuration= configuration;
        }
        [HttpGet]
        [AllowAnonymous]  
        
        public async Task< IActionResult> Index()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task< IActionResult> Login( LoginRequest request )
        {
            if(!ModelState.IsValid) 
                return View(request);
            var result = await _loginApiClient.AuthenticateAdmin(request);
            if (result.ResultObj ==null)
            {
                ModelState.AddModelError("", result.Message);
                return View();
            }

            var userPrincipal = this.ValidateToken(result.ResultObj);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false //khi đã đăng nhập thành công thì false nó sẽ  load lại trang web và bắt đăng nhập lại
            };
            //HttpContext.Session.SetString(SystemContants.DefaultLanguageId, _configuration[SystemContants.DefaultLanguageId]);
            //HttpContext.Session.SetString(SystemContants.Token, result.ResultObj);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal,
                authProperties);
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public async Task<IActionResult>Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            //HttpContext.Session.Remove(SystemContants.Token);
            return RedirectToAction("Index", "Login");
        }

        // Muntiple language
        [AllowAnonymous]
        public IActionResult SetCultureCookie(string cltr, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(cltr)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );

            return LocalRedirect(returnUrl);
        }
        //Muntiple language end

        //giải mã token
        private ClaimsPrincipal ValidateToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateLifetime = true;

            validationParameters.ValidAudience = _configuration["Tokens:Issuer"];
            validationParameters.ValidIssuer = _configuration["Tokens:Issuer"];
            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Tokens:Key"]));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

            return principal;
        }
        //giải mã token end


    }
}
using eShopSolution.ApiItergration;
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
using eShopSolution.ViewModel.Validator;
using System.ComponentModel.DataAnnotations;
using eShopSolution.AdminApp.LocalizationResources;
using Microsoft.Extensions.Localization;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace eShopSolution.AdminApp.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
        private readonly ILoginApiClient _loginApiClient;
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizerFactory _localizerFactory;
        public LoginController(ILoginApiClient loginApiClient, IConfiguration configuration, IStringLocalizerFactory localizerFactory) 
        { 
            _loginApiClient= loginApiClient;
            _configuration= configuration;
            _localizerFactory= localizerFactory;
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
        public async Task< IActionResult> Index( LoginRequest request )
        {

            var localizer = this.Localizer(request,null);
            if (localizer) 
                return View(request);
            
            var result = await _loginApiClient.AuthenticateAdmin(request);
            if (result.ResultObj ==null)
            {
                this.Localizer(request,result.Message);
                return View(request);
            }

            var userPrincipal = this.ValidateToken(result.ResultObj);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                IsPersistent = false //khi đã đăng nhập thành công thì false nó sẽ  load lại trang web và bắt đăng nhập lại
            };
            //HttpContext.Session.SetString(SystemContants.DefaultLanguageId, _configuration[SystemContants.DefaultLanguageId]);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                userPrincipal,
                authProperties);
                HttpContext.Session.SetString(SystemContants.Token, result.ResultObj);
            return RedirectToAction("Index","Home");
        }

        [HttpPost]
        public async Task<IActionResult>SignOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove(SystemContants.Token);
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

        //Localize
        private bool Localizer(LoginRequest request, string errorMessage)
        {
           
            var localizer = _localizerFactory.Create(nameof(ExpressLocalizationResource),
                    typeof(ExpressLocalizationResource).Assembly.GetName().Name);

            //View
            var validator = new LoginRequestValaditor();
            var resultValidator = validator.Validate(request);
            if (!resultValidator.IsValid)
            {
                
                var messages = new List<string>
                {
                    ExpressLocalizationResource.User.LoginRequest.UserName,
                    ExpressLocalizationResource.User.LoginRequest.PassWord,
                    ExpressLocalizationResource.User.LoginRequest.PassWordLength,
                };
                
                foreach (var error in resultValidator.Errors)
                {
                    foreach(var message in messages)
                    {
                        if (message == error.ErrorMessage)
                        {
                            ModelState.AddModelError("", localizer[message]);
                        }
                    }
                }
                return true;
            }

            //API
            if(!errorMessage.IsNullOrEmpty())
            {
                var messages = new List<string>
                {
                    ExpressLocalizationResource.User.LoginRequest.UserNameApi,
                    ExpressLocalizationResource.User.LoginRequest.PassWordApi
                };
               foreach(var message in messages)
                {
                    if(message == errorMessage) 
                    {
                        ModelState.AddModelError("", localizer[message]);
                    } 
                }
                return true;
            }
            return false;
        }
        //Localize and


    }
}

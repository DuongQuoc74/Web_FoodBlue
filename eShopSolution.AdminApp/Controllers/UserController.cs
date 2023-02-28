using eShopSolution.AdminApp.LocalizationResources;
using eShopSolution.ApiItergration;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Users;
using eShopSolution.ViewModel.Validator;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;

namespace eShopSolution.AdminApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IStringLocalizerFactory _localizerFactory;
        public UserController(IUserApiClient userApiClient, IStringLocalizerFactory localizerFactory) 
        {
            _userApiClient = userApiClient;
            _localizerFactory = localizerFactory;
        }
        public async Task< IActionResult>Index(string keyWord, int pageIndex =1, int pageSize = 5)
        {
            var request = new PadingRequest()
            {
                KeyWord = keyWord,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var data = await _userApiClient.GetPadingUser(request);
            if(data == null)
                return NotFound();
            return View(data.ResultObj);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var check = this.LocalizerRegistor(request, null);
            if (check)
                return View(request);
            var result =await _userApiClient.Register(request);
            
            if (!result.IsSuccessed)
            {
                this.LocalizerRegistor(request, result.Message);
                return View(result);
            }
            return RedirectToAction("Index", "User");
        }

        //Localize and

        private bool LocalizerRegistor(RegisterRequest request, string errorMessage)
        {

            var localizer = _localizerFactory.Create(nameof(ExpressLocalizationResource),
                    typeof(ExpressLocalizationResource).Assembly.GetName().Name);

            //View
            var validator = new RegistorRequestValidator();
            var resultValidator = validator.Validate(request);
            if (!resultValidator.IsValid)
            {

                var messages = new List<string>
                {
                    ExpressLocalizationResource.LoginRequest.UserName,
                    ExpressLocalizationResource.LoginRequest.PassWord,
                    ExpressLocalizationResource.LoginRequest.PassWordLength,
                };

                foreach (var error in resultValidator.Errors)
                {
                    foreach (var message in messages)
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
            if (!errorMessage.IsNullOrEmpty())
            {
                var messages = new List<string>
                {
                    ExpressLocalizationResource.LoginRequest.UserNameApi,
                    ExpressLocalizationResource.LoginRequest.PassWordApi
                };
                foreach (var message in messages)
                {
                    if (message == errorMessage)
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

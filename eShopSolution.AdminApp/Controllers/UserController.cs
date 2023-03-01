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

        public async Task< IActionResult>Index(string keyWord, int pageIndex =1, int pageSize = 10)
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
            var deleteSuccess = TempData["DeleteSuccess"] as string;
            if (deleteSuccess != null)
            {
                var localizer = _localizerFactory.Create(nameof(ExpressLocalizationResource),
                    typeof(ExpressLocalizationResource).Assembly.GetName().Name);
                ViewBag.DeleteSuccess = localizer[deleteSuccess];
            }
            
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
                return View(request);
            }
            
            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public IActionResult Delete(Guid id, string userName)
        {
            var request = new DeleteRequest()
            {
                Id = id,
                UserName = userName,
               
            };
            return View(request);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(DeleteRequest request)
        {
            var result = await _userApiClient.Delete(request.Id);
            if (!result.IsSuccessed)
            {
                ModelState.AddModelError("",result.Message);
                return View(request);
            }

            TempData["DeleteSuccess"] = "Delete is success!";
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
                    ExpressLocalizationResource.RegisterRequest.UserName,
                    ExpressLocalizationResource.RegisterRequest.UserNameLength,
                    ExpressLocalizationResource.RegisterRequest.PassWord,
                    ExpressLocalizationResource.RegisterRequest.PassWordLength,
                    ExpressLocalizationResource.RegisterRequest.ConfirmPassWord,
                    ExpressLocalizationResource.RegisterRequest.ConfirmPassWordNull,
                    ExpressLocalizationResource.RegisterRequest.FirstName,
                    ExpressLocalizationResource.RegisterRequest.LastName,
                    ExpressLocalizationResource.RegisterRequest.Dob,
                    ExpressLocalizationResource.RegisterRequest.Email,
                    ExpressLocalizationResource.RegisterRequest.EmailFormat,
                    ExpressLocalizationResource.RegisterRequest.PhoneNumber,
                    ExpressLocalizationResource.RegisterRequest.PhoneNumberLength,
                    ExpressLocalizationResource.RegisterRequest.PhoneNumberFormat,
                 
                    
                    
                    
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
                    ExpressLocalizationResource.RegisterRequest.UserNameApi,
                    ExpressLocalizationResource.RegisterRequest.EmailApi,
                    ExpressLocalizationResource.RegisterRequest.PhoneNumberApi,
                    ExpressLocalizationResource.RegisterRequest.AccoutCreate,
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

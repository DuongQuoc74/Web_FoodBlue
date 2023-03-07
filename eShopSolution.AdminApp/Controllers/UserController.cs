using eShopSolution.AdminApp.LocalizationResources;
using eShopSolution.ApiItergration;
using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Roles;
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
            ViewBag.KeyWord = keyWord;
            var data = await _userApiClient.GetPadingUser(request);
            var result = TempData["result"] as string;
            if (result != null)
            {
                var localizer = _localizerFactory.Create(nameof(ExpressLocalizationResource),
                    typeof(ExpressLocalizationResource).Assembly.GetName().Name);
                ViewBag.result = localizer[result];
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
            TempData["result"] = "Create user is success!";
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
                TempData["result"] = result.Message;
                return RedirectToAction("Index", "User");
            }

            TempData["result"] = "Delete is success!";
            return RedirectToAction("Index", "User");

        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            if(result.ResultObj == null)
                return View();
            var user = result.ResultObj;
            var updateRequest = new UpdateUserRequest()
            {
                FirstName = user.FristName,
                LastName = user.LastName,
                Dob = user.Dob,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
            return View(updateRequest);
        }
        [HttpPost]
        public async Task<IActionResult> Edit (UpdateUserRequest request)
        {
            var check = this.LocalizerUpadte(request, null);
            if (check)
                return View(request);
            var result = await _userApiClient.Update(request.Id, request);
            if (!result.IsSuccessed)
            {
                 this.LocalizerUpadte(request,result.Message);
                return View(request);

            }
            TempData["result"] = "User update is successful!";
            return RedirectToAction("Index", "User");
            
        }

        [HttpGet]
        public async Task<IActionResult> RoleAssign(Guid id)
        {
            var user = await _userApiClient.GetById(id);
            var roles = await _userApiClient.GetAllRoles();
            var roleAssignRequest = new RoleAssignRequest();
            foreach (var role in roles.ResultObj) 
            {
                roleAssignRequest.Items.Add( new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected=user.ResultObj.Roles.Contains(role.Name)
                });
            }
            return View(roleAssignRequest);
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign (RoleAssignRequest request)
        {
            var result = await _userApiClient.RoleAssign(request.Id, request);
            if (!result.IsSuccessed)
            {
                TempData["result"] = "Assign is fail!";
                return RedirectToAction("Index", "User");
            }


            TempData["result"] = "Assign is success!";
            return RedirectToAction("Index", "User");
        }

        //Localize and
        private bool LocalizerRegistor(RegisterRequest request, string errorMessage)
        {

            var localizer = _localizerFactory.Create(nameof(ExpressLocalizationResource),
                    typeof(ExpressLocalizationResource).Assembly.GetName().Name);

            //View
            var validator = new RegisterRequestValidator();
            var resultValidator = validator.Validate(request);
            if (!resultValidator.IsValid)
            {

                var messages = new List<string>
                {
                    ExpressLocalizationResource.User.RegisterRequest.UserName,
                    ExpressLocalizationResource.User.RegisterRequest.UserNameLength,
                    ExpressLocalizationResource.User.RegisterRequest.PassWord,
                    ExpressLocalizationResource.User.RegisterRequest.PassWordLength,
                    ExpressLocalizationResource.User.RegisterRequest.ConfirmPassWord,
                    ExpressLocalizationResource.User.RegisterRequest.ConfirmPassWordNull,
                    ExpressLocalizationResource.User.RegisterRequest.FirstName,
                    ExpressLocalizationResource.User.RegisterRequest.LastName,
                    ExpressLocalizationResource.User.RegisterRequest.Dob,
                    ExpressLocalizationResource.User.RegisterRequest.Email,
                    ExpressLocalizationResource.User.RegisterRequest.EmailFormat,
                    ExpressLocalizationResource.User.RegisterRequest.PhoneNumber,
                    ExpressLocalizationResource.User.RegisterRequest.PhoneNumberLength,
                    ExpressLocalizationResource.User.RegisterRequest.PhoneNumberFormat,                         
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
                    ExpressLocalizationResource.User.RegisterRequest.UserNameApi,
                    ExpressLocalizationResource.User.RegisterRequest.EmailApi,
                    ExpressLocalizationResource.User.RegisterRequest.PhoneNumberApi,
                    ExpressLocalizationResource.User.RegisterRequest.AccoutCreate,
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

        private bool LocalizerUpadte(UpdateUserRequest request, string errorMessage)
        {

            var localizer = _localizerFactory.Create(nameof(ExpressLocalizationResource),
                    typeof(ExpressLocalizationResource).Assembly.GetName().Name);

            //View
            var validator = new UpdateUserRequestValidator();
            var resultValidator = validator.Validate(request);
            if (!resultValidator.IsValid)
            {

                var messages = new List<string>
                {
                    ExpressLocalizationResource.User.UpdateRequest.FirstName,
                    ExpressLocalizationResource.User.UpdateRequest.LastName,
                    ExpressLocalizationResource.User.UpdateRequest.Dob,
                    ExpressLocalizationResource.User.UpdateRequest.Email,
                    ExpressLocalizationResource.User.UpdateRequest.EmailFormat,
                    ExpressLocalizationResource.User.UpdateRequest.PhoneNumber,
                    ExpressLocalizationResource.User.UpdateRequest.PhoneNumberLength,
                    ExpressLocalizationResource.User.UpdateRequest.PhoneNumberFormat,
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
                
                    ExpressLocalizationResource.User.UpdateRequest.EmailApi,
                    ExpressLocalizationResource.User.UpdateRequest.PhoneNumberApi,
                  
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

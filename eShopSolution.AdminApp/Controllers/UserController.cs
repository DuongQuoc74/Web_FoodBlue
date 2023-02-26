using eShopSolution.ApiItergration;
using eShopSolution.ViewModel.Common;
using Microsoft.AspNetCore.Mvc;

namespace eShopSolution.AdminApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserApiClient _userApiClient;
        public UserController(IUserApiClient userApiClient) 
        {
            _userApiClient = userApiClient;
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
    }
}

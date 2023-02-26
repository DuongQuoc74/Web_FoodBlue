using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Systems.Users
{
    public interface IUserService
    {
        Task<ApiResult<string>> AuthenticateAdmin(LoginRequest request);
        Task<ApiResult<bool>> Register(RegisterRequest request);
        Task<ApiResult<PageResult<UserVM>>> GetPadingRequest(PadingRequest request);

    }
}

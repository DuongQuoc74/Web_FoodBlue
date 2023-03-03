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
        Task<ApiResult< string>> Register(RegisterRequest request);
        Task<ApiResult<bool>> Delete (Guid id);
        Task<ApiResult<string>> Edit (Guid id, UpdateUserRequest request);
        Task<ApiResult<PageResult<UserVM>>> GetPadingRequest(PadingRequest request);
        Task<ApiResult<UserVM>> GetById(Guid id);

    }
}

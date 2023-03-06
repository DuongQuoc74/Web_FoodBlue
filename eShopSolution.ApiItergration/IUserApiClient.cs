using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Roles;
using eShopSolution.ViewModel.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ApiItergration
{
    public interface IUserApiClient
    {
        Task<ApiResult<PageResult<UserVM>>> GetPadingUser(PadingRequest request);
        Task<ApiResult<UserVM>> GetById(Guid id);
        Task<ApiResult<string>> Register(RegisterRequest request);
        Task<ApiResult<bool>> Delete(Guid id);
        Task<ApiResult<string>> Update(Guid id, UpdateUserRequest request);
        Task <ApiResult<List<RoleVM>>>GetAllRoles();
    }
}

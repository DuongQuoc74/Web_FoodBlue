using eShopSolution.ViewModel.Common;
using eShopSolution.ViewModel.System.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ApiItergration
{
    public interface ILoginApiClient
    {
        Task<ApiResult<string>> AuthenticateAdmin(LoginRequest request);
    }
}

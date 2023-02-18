using eShopSolution.ViewModel;
using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Systems.Languages
{
    public interface ILanguageService
    {
        Task<ApiResult<List<LanguageVM>>> GetAll();
    }
}

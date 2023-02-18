using eShopSolution.Data.EF;
using eShopSolution.ViewModel;
using eShopSolution.ViewModel.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Systems.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly EShopDbContext _context;
        public LanguageService(EShopDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResult<List<LanguageVM>>> GetAll()
        {
            var language = await _context.Languages.Select(x => new LanguageVM()
            {
                LanguageId = x.Id,
                Name = x.Name,
            }).ToListAsync();
            return new ApiSuccessResult<List<LanguageVM>>(language);
        }
    }
}

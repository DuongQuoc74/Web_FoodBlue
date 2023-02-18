using eShopSolution.Data.EF;
using eShopSolution.ViewModel.Catalog.Category;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly EShopDbContext _context;
        public CategoryService(EShopDbContext context) {
        _context= context;
        }
        public async Task<List<CategoryVM>> GetAll(string languageId)
        {
            var query = from c in _context.Categories
                        join ct in _context.CategoriesTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId== languageId
                        select new {c,ct};
            var data = await query.Select(x => new CategoryVM()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                ParentId = x.c.ParenId
            }).ToListAsync();
            return data;
        }

        public async Task<CategoryVM> GetById(int id, string languageId)
        {
            
            var query = from c in _context.Categories
                        join ct in _context.CategoriesTranslations on c.Id equals ct.CategoryId
                        where ct.LanguageId== languageId && c.Id== id
                        select new {c,ct};
            var data = await query.Select(x => new CategoryVM()
            {
                Id = x.c.Id,
                Name = x.ct.Name,
                ParentId = x.c.ParenId
            }).FirstOrDefaultAsync();
            return data?? new CategoryVM();
        }
    }
}

using eShopSolution.ViewModel.Catalog.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.Application.Catalog.Categories

{
    public interface ICategoryService
    {
        Task<List<CategoryVM>> GetAll(string languageId);
        Task<CategoryVM> GetById(int id , string languageId);
    }
}

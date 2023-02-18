using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.Catalog.Product
{
    public class ProductUpdateRequest
    {
        public int ProductId { get; set; }
        public string? LanguageId { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Detail { get; set; }
        public bool? IsFeartured { get; set; }
        public IFormFile? ThumbNailImage { get; set; }

    }
}

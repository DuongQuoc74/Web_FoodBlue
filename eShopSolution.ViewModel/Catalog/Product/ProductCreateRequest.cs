using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.Catalog.Product
{
    public class ProductCreateRequest
    {
        public decimal Price { get; set; } 
        public decimal OriginalPrice { get; set; }
        public int Stock { get; set; }
        public bool? IsFeartured { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Detail { get; set; }
        public string? LanguageId { get; set; }
        public IFormFile? ThumbnailImage { get; set; }
    }
}

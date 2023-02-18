using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.Catalog.Product
{
    public class ProductVM
    {
        public int? ProductId { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Detail { get; set; }
        public decimal? Price { get; set; }
        public decimal? OriginalPrice { get; set; }
        public decimal? Stock { get; set; }
        public string? ThumbnailImage { get; set; }
        public int? ViewCout { get; set; }
        public DateTime? DateCreate { get; set; }
        public bool? IsFeatured { set; get; }
        public List<string>? Categories { set; get; } 

    }
}

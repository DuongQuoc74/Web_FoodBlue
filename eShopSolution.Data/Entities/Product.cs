using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Stock { get; set; }
        public int ViewCout { get; set; }
        public bool? IsFeartured { get; set; }
        public DateTime DateCreate { get; set; }
        public List<ProductInCategory> ProductInCategories { get; set; }
        public List<ProductTranslation> ProductTranslations { get; set; }
        public List<ProductImage> ProductImages { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
        public List<Cart> Carts { get; set; }
        
    }
}

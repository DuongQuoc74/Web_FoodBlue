using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class OrderDetail 
    {
        public int Id { get; set; }
        public int OderId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public int  Quantity { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}

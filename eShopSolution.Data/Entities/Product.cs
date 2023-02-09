﻿using System;
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
        public bool IsFeartured { get; set; }
        public DateTime DateCreate { get; set; }
        
    }
}
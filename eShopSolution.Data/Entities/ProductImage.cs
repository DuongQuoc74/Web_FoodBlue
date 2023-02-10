﻿using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class ProductImage
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Caption { get; set; }
        public int SortOrder { get; set; }
        public DateTime DateCreate { get; set; }
        public long FileSize { get; set; }
        public bool IsDefaul { get; set; }
    }
}

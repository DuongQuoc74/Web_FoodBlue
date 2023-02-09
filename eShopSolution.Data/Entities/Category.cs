using eShopSolution.Data.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class Category
    {
       public int Id { get; set; }
        public int SortOder { get; set; }
        public bool IsShowHome { get; set; }
        public int? ParenId { get; set; }
        public Status Status { get; set; }


    }
}

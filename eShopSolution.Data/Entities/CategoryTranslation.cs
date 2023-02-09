using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.Data.Entities
{
    public class CategoryTranslation
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string LanguageId { get; set; }
    }
}

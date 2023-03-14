using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.Catalog.Product
{
    public class GetPagingProductRequest : PadingRequestBase
    {
        public string languageId { get; set; }
        public string? KeyWord { get; set; }
        public int CategoryId { get; set; }
    }
}

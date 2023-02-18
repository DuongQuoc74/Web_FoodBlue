using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.Common
{
    public class ApiResult <T>
    {
        public bool IsSuccessed { get; set; }
        public string[]? ValidationErrors { get; set; }
        public string? Message { get; set; }
        public T? ResultObj { get; set; }
    }
}

using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.System.Roles
{
    public class RoleAssignRequest
    {
        public Guid Id { get; set; }
        public List<SelectItem> Items { get; set; } = new List<SelectItem>();
    }
}

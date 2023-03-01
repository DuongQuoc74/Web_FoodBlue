using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopSolution.ViewModel.System.Users
{
    public class DeleteRequest
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }
}

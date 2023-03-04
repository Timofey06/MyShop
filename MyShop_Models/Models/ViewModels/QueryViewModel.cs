using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_Models.Models.ViewModels
{
    public  class QueryViewModel
    {
        QueryHeader QueryHeader { get; set; }
        public IEnumerable<QueryDetail> QueryDetails { get; set; }
    }
}

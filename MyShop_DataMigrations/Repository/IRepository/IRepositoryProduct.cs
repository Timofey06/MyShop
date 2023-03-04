using Microsoft.AspNetCore.Mvc.Rendering;
using MyShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_DataMigrations.Repository.IRepository
{
    public interface IRepositoryProduct:IRepository<Product>
    {
        void Update(Product obj);
        IEnumerable<SelectListItem> GetListItems(string obj);
    }
}

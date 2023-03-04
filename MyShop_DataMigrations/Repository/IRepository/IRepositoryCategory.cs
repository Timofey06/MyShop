using MyShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_DataMigrations.Repository.IRepository
{
    public interface IRepositoryCategory:
        IRepository<Category>
    {
        void Update(Category obj);
    }
}

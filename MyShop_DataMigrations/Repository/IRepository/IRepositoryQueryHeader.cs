using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop_Models;
using MyShop_Models.Models;

namespace MyShop_DataMigrations.Repository.IRepository
{
    public interface IRepositoryQueryHeader: IRepository<QueryHeader>
    {
        void Update(QueryHeader obj);
    }
}

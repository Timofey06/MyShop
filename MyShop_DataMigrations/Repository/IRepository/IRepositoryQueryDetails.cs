using MyShop_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_DataMigrations.Repository.IRepository
{
    public interface IRepositoryQueryDetail:IRepository<QueryDetail>
    {
        void Update(QueryDetail obj);
    }
}

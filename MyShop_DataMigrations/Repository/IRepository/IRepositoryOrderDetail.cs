using MyShop_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_DataMigrations.Repository.IRepository
{
    public interface IRepositoryOrderDetail:IRepository<OrderDetail>
    {
        void Update(OrderDetail obj);
    }
}

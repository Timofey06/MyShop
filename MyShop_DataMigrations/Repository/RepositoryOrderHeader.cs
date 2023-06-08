using MyShop_DataMigrations.Repository.IRepository;
using MyShop_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_DataMigrations.Repository
{
    public class RepositoryOrderHeader : Repository<OrderHeader>, IRepositoryOrderHeader
    {
        public RepositoryOrderHeader(ApplicationDbContext db) : base(db)
        {

        }
        public void Update(OrderHeader obj)
        {
            db.OrderHeader.Update(obj);
        }
    }
}

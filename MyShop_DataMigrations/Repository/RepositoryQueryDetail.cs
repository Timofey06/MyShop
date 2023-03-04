using MyShop_DataMigrations.Repository.IRepository;
using MyShop_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_DataMigrations.Repository
{
    public class RepositoryQueryDetail : Repository<QueryDetail>, IRepositoryQueryDetail
    {
        public RepositoryQueryDetail(ApplicationDbContext db) : base(db)
        {

        }
        public void Update(QueryDetail obj)
        {
            db.QueryDetail.Update(obj);
        }
    }
}

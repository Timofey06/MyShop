using MyShop_DataMigrations.Repository.IRepository;
using MyShop_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_DataMigrations.Repository
{
    public class RepositoryQueryHeader : Repository<QueryHeader>, IRepositoryQueryHeader
    {
        public RepositoryQueryHeader(ApplicationDbContext db) : base(db)
        {

        }
        public void Update(QueryHeader obj)
        {
           db.QueryHeader.Update(obj);
        }
    }
}

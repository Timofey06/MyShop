using MyShop_DataMigrations.Repository.IRepository;
using MyShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MyShop_DataMigrations.Repository
{
    public class RepositoryMyModel :  Repository<MyModel>, IRepositoryMyModel
    {
        public void Update(MyModel obj)
        {
            var objFromDb = db.MyModel.FirstOrDefault(x => x.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Number = obj.Number;
            }

        }
        public RepositoryMyModel(ApplicationDbContext db) :base(db)
        {

        }

    }
}

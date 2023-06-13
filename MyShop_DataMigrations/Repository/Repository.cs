using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using MyShop_DataMigrations.Repository.IRepository;
using MyShop_DataMigrations;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace MyShop_DataMigrations.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext db;
        private DbSet<T> dbSet;
        //using vnedreniem zavisimosty
        public Repository(ApplicationDbContext db)
        {
            this.db = db;
            db.Database.Migrate();
            dbSet = db.Set<T>();
            
        }
        public void Add(T iteam)
        {
            dbSet.Add(iteam);
        }

        public T Find(int id)
        {
            return dbSet.Find(id);
        }

        public T FirstOrDefault(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null,
            bool IsTracing = true)
        {
            IQueryable<T> quiry = dbSet;
            //filter
            if (filter != null)
            {
                quiry = quiry.Where(filter);
            }
            //proporties
            if (includeProperties != null)
            {
                //quiry.Include(includeProperties);
                foreach (var item in includeProperties.Split(','))
                {
                    quiry = quiry.Include(item);
                }
            }
            //tracing
            if (!IsTracing)
            {
                quiry = quiry.AsNoTracking();
            }
            return quiry.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null,
             bool IsTracing = true)
        {
            IQueryable<T> quiry = dbSet;
            //filter
            if (filter != null)
            {
                quiry = quiry.Where(filter);
            }
            //proporties
            if (includeProperties!=null)
            {
                //quiry.Include(includeProperties);
                foreach (var item in includeProperties.Split(','))
                {
                    quiry=quiry.Include(item);
                }
            }
            //orderBy
            if (orderBy!=null)
            {
                quiry = orderBy(quiry);
            }
            //tracing
            if (!IsTracing)
            {
                quiry=quiry.AsNoTracking();
            }
            return quiry.ToList();
        }

        public void Remove(T iteam)
        {
            dbSet.Remove(iteam);
        }

        public void Remove(IEnumerable<T> iteams)
        {
            dbSet.RemoveRange(iteams);
        }

        public void Save()
        {
            db.SaveChanges();
            
        }

        
    }
}

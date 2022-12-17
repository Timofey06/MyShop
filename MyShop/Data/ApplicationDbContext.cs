using Microsoft.EntityFrameworkCore;
using MyShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace MyShop.Data
{
    public class ApplicationDbContext:DbContext//изменили наследование
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):
            base(options)
        {
           
        }
        public DbSet<Category> Category { get; set; }
        public DbSet<MyModel> MyModel { get; set; }
        public DbSet<Product> Product { get; set; }
    }
}

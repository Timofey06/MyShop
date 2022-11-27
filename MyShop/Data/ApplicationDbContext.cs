﻿using Microsoft.EntityFrameworkCore;
using MyShop.Models;
namespace MyShop.Data
{
    public class ApplicationDbContext:DbContext
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

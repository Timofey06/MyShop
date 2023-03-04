﻿using MyShop_DataMigrations.Repository.IRepository;
using MyShop_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop_DataMigrations.Repository
{
    public class RepositoryApplicationUser:Repository<ApplicationUser>,IRepositoryApplicationUser
    {

        public RepositoryApplicationUser(ApplicationDbContext db):base(db)
        {

        }

    }
}

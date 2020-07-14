using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace TheArrowData
{
    class ArrowDbContext : DbContext
    {
        public DbSet<Store> Stores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlCe(@"Data Source=C:\data\Blogging.sdf");
        }

    }
}

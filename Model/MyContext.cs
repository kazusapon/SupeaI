using System;
using System.IO;
using Microsoft.EntityFrameworkCore;


namespace SupeaI.Models
{
    public class MyContext : DbContext
    {
        public MyContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Mst_Spend> Mst_Spend { get; set; }
        public DbSet<Mst_Income> Mst_Income { get; set; }
        public DbSet<Mst_Fixed> Mst_Fixed { get; set; }
        public DbSet<Mst_Available> Mst_Available { get; set; }
        public DbSet<Mst_User> Mst_User { get; set; }
        public DbSet<Tra_Spending> Tra_Spending { get; set; }
        public DbSet<Tra_Income> Tra_Income { get; set; }
    }
}
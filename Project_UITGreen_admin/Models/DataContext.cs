using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_UITGreen_admin.Models
{
    public class DataContext : DbContext
    {
        private const string connectionString
     = "server=localhost;port=3306;database=quanlysieuthi;uid=root;password=";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySQL(connectionString);
        }
        public DbSet<Category> Category { set; get; }
        public DbSet<Employee> Employee { set; get; }
        public DbSet<Comment> Comment { set; get; }
        public DbSet<Promotion> Promotion { set; get; }
        public DbSet<Image> Image { set; get; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasKey(c => new { c.id_cat });
            modelBuilder.Entity<Employee>().HasKey(c => new { c.id_emp });
            modelBuilder.Entity<Comment>().HasKey(c => new { c.id_cmt });
            modelBuilder.Entity<Promotion>().HasKey(c => new { c.id_promotion });
            modelBuilder.Entity<Image>().HasKey(c => new { c.id_img });
        }
    }
}

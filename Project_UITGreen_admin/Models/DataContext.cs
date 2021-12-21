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
        public DbSet<Product> Product { set; get; }
        public DbSet<Image> Image { set; get; }
        public DbSet<Image_product> Image_product { set; get; }
        public DbSet<Employee> Employee { set; get; }
        public DbSet<Comment> Comment { set; get; }
        public DbSet<Orders> Orders { set; get; }
        public DbSet<Orders_user> Orders_user { set; get; }
        public DbSet<Order_items> Order_items { set; get; }
        public DbSet<Order_user_items> Order_user_items { set; get; }
        public DbSet<Order_status> Order_status { set; get; }
        //public DbSet<Storage_import> Storage_import { set; get; }
        //public DbSet<Storage_export> Storage_export { set; get; }
        //public DbSet<Customer> Customer { set; get; }
        public DbSet<Users> Users { set; get; }
        public DbSet<Banner> Banner { set; get; }
        public DbSet<Promotion> Promotion { set; get; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasKey(c => new { c.id_cat });
            modelBuilder.Entity<Product>().HasKey(c => new { c.id_pro });
            modelBuilder.Entity<Image>().HasKey(c => new { c.id_img });
            modelBuilder.Entity<Image_product>().HasKey(c => new { c.id_img });
            modelBuilder.Entity<Employee>().HasKey(c => new { c.id_emp });
            modelBuilder.Entity<Comment>().HasKey(c => new { c.id_cmt });
            modelBuilder.Entity<Orders>().HasKey(c => new { c.id_ord });
            modelBuilder.Entity<Orders_user>().HasKey(c => new { c.id_ord });
            modelBuilder.Entity<Order_items>().HasKey(c => new { c.id_ord });
            modelBuilder.Entity<Order_items>().HasKey(c => new { c.id_pro });
            modelBuilder.Entity<Order_user_items>().HasKey(c => new { c.id_user_items });
            //modelBuilder.Entity<Storage_import>().HasKey(c => new { c.id_imp });
            //modelBuilder.Entity<Storage_export>().HasKey(c => new { c.id_exp });
            //modelBuilder.Entity<Customer>().HasKey(c => new { c.id_cus });
            modelBuilder.Entity<Users>().HasKey(c => new { c.id });
            modelBuilder.Entity<Banner>().HasKey(c => new { c.id_banner });
            modelBuilder.Entity<Order_status>().HasKey(c => new { c.id });
            modelBuilder.Entity<Promotion>().HasKey(c => new { c.id_promotion });
        }
    }

}

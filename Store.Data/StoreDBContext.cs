using Microsoft.EntityFrameworkCore;
using Store.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Data
{
    public class StoreDBContext : DbContext
    {
        public StoreDBContext()
        {

        }
        public StoreDBContext(DbContextOptions<StoreDBContext> options)
          : base(options)
        { }
        public DbSet<User> Users { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleProduct> SaleProducts { get; set; }
        public DbSet<Bill> Bills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);
            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductType)
                .WithMany(pt => pt.Products)
                .HasForeignKey(p => p.ProductTypeId);
            modelBuilder.Entity<SaleProduct>()
                .HasKey(sp => new { sp.SaleId, sp.ProductId });
            modelBuilder.Entity<SaleProduct>()
                .HasOne(bc => bc.Product)
                .WithMany(b => b.SaleProducts)
                .HasForeignKey(bc => bc.ProductId);
            modelBuilder.Entity<SaleProduct>()
                .HasOne(bc => bc.Sale)
                .WithMany(c => c.SaleProducts)
                .HasForeignKey(bc => bc.SaleId);
            modelBuilder.Entity<Sale>()
                .HasOne(b => b.Bill)
                .WithOne(b => b.Sale)
                .HasForeignKey<Bill>(s => s.SaleId);
           
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=.;database=StoreDatabase;Integrated Security=SSPI;persist security info=True;");
                optionsBuilder.EnableSensitiveDataLogging(true);
            }
        }
    }
}

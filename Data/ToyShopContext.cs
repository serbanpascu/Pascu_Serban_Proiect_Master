using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Pascu_Serban_Proiect.Models;

namespace Pascu_Serban_Proiect.Data
{
    public class ToyShopContext : DbContext
    {
        public ToyShopContext(DbContextOptions<ToyShopContext> options) : 
    base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Toy> Toys { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<BrandedToy> BrandedToys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customer");
            modelBuilder.Entity<Worker>().ToTable("Worker");
            modelBuilder.Entity<Order>().ToTable("Order");
            modelBuilder.Entity<Toy>().ToTable("Toy");
            modelBuilder.Entity<Brand>().ToTable("Brand");
            modelBuilder.Entity<BrandedToy>().ToTable("BrandedToy");

            modelBuilder.Entity<BrandedToy>()
                .HasKey(c => new { c.ToyID, c.BrandID });
        }
    }
}

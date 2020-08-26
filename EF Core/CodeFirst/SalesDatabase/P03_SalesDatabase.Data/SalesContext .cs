using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;
using System;

namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {
        public SalesContext()
        {

        }


        public SalesContext(DbContextOptions options) 
            :base(options) { }


        //TODO: DbSets
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Store> Stores { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.Connection);
            }
            base.OnConfiguring(optionsBuilder);
        }

        //TODO: other
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(e =>
            {
                e.HasKey(c => c.CustomerId);

                e.Property(c => c.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(100);

                e.Property(c => c.Email)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(80);

                e.Property(c => c.CreditCardNumber)
                .IsRequired();

               
            });

            modelBuilder.Entity<Product>(e =>
            {
                e.HasKey(p => p.ProductId);


                e.Property(p => p.Name)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(50);

                e.Property(p => p.Quantity)
                .IsRequired();

                e.Property(p => p.Price).IsRequired();

            });

            modelBuilder.Entity<Store>(e =>
            {

                e.HasKey(s => s.StoreId);

                e.Property(s => s.Name).IsRequired()
                .IsUnicode().HasMaxLength(80);
            });

            modelBuilder.Entity<Sale>(e =>
            {
                e.HasKey(s => s.SaleId);

                e.Property(s => s.Date).IsRequired();

                e.HasOne(s => s.Customer)
                .WithMany(s => s.Sales)
                .HasForeignKey(c => c.CustomerId);

                e.HasOne(p => p.Product)
                .WithMany(s => s.Sales)
                .HasForeignKey(p => p.ProductId);

                e.HasOne(s => s.Store)
                .WithMany(s => s.Sales)
                .HasForeignKey(st => st.StoreId);
            });
        }
    }
}

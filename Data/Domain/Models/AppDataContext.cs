using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Models
{
    public class AppDataContext : DbContext
    {
        public AppDataContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Quote> Quotes { get; set; }

        public DbSet<ServiceIndex> ServiceIndexs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>()
                .HasMany(z => z.Invoices)
                .WithOne(z => z.Car);

            modelBuilder.Entity<Car>()
                .HasMany(z => z.Quotes)
                .WithOne(z => z.Car);


            modelBuilder.Entity<Customer>()
                .HasMany(z => z.Invoices)
                .WithOne(z => z.Customer);


            modelBuilder.Entity<Customer>()
                .HasMany(z => z.Quotes)
                .WithOne(z => z.Customer);


            modelBuilder.Entity<Service>()
                .HasOne(z => z.Invoice)
                .WithMany(z => z.Services);

            modelBuilder.Entity<Service>()
                .HasOne(z => z.Quote)
                .WithMany(z => z.Services);


            modelBuilder.Entity<ServiceIndex>()
                .HasKey(z => z.ServiceName);

        }

    }
}

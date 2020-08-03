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
            modelBuilder.Entity<Invoice>()
                .HasMany(z => z.Services)
                .WithOne(z => z.Invoice);

            modelBuilder.Entity<Invoice>()
                .HasOne(z => z.Car)
                .WithMany(z => z.Invoices);

            modelBuilder.Entity<Invoice>()
                .HasOne(z => z.Customer)
                .WithMany(z => z.Invoices);

            modelBuilder.Entity<Invoice>()
                .HasOne(z => z.User)
                .WithMany(z => z.Invoices);

            modelBuilder.Entity<Quote>()
                .HasMany(z => z.Services)
                .WithOne(z => z.Quote);

            modelBuilder.Entity<Quote>()
                .HasOne(z => z.Car)
                .WithMany(z => z.Quotes);

            modelBuilder.Entity<Quote>()
                .HasOne(z => z.Customer)
                .WithMany(z => z.Quotes);

            modelBuilder.Entity<Quote>()
                .HasOne(z => z.User)
                .WithMany(z => z.Quotes);

            modelBuilder.Entity<ServiceIndex>()
                .HasKey(z => z.ServiceName);
        }

    }
}

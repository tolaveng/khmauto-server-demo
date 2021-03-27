using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Domain.Models
{
    public class AppDataContext : IdentityDbContext<User, UserRole, int>
    {
        public AppDataContext(DbContextOptions options): base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Quote> Quotes { get; set; }

        public DbSet<ServiceIndex> ServiceIndexs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Car>().HasKey(z => z.CarNo);

            modelBuilder.Entity<Invoice>()
                .HasOne(z => z.Car).WithMany(z => z.Invoices).HasForeignKey(z => z.CarNo);

            modelBuilder.Entity<Quote>()
                .HasOne(z => z.Car).WithMany(z => z.Quotes);

            modelBuilder.Entity<Service>()
                .HasOne(z => z.Invoice)
                .WithMany(z => z.Services);

            modelBuilder.Entity<Service>()
                .HasOne(z => z.Quote)
                .WithMany(z => z.Services);


            modelBuilder.Entity<ServiceIndex>()
                .HasKey(z => z.ServiceName);

            modelBuilder.Entity<Invoice>()
                .HasIndex(z => z.InvoiceNo).IsUnique();
        }

    }
}

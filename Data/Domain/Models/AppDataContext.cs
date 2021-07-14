using Microsoft.AspNetCore.Identity;
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

        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<ServiceIndex> ServiceIndexs { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fix Mysql max key length
            modelBuilder.Entity<User>().Property(u => u.Id).HasMaxLength(36);
            modelBuilder.Entity<UserRole>().Property(u => u.Id).HasMaxLength(36);

            // Shorten key length for Identity 
            modelBuilder.Entity<User>(entity => {
                entity.Property(m => m.Email).HasMaxLength(127);
                entity.Property(m => m.NormalizedEmail).HasMaxLength(127);
                entity.Property(m => m.NormalizedUserName).HasMaxLength(127);
                entity.Property(m => m.UserName).HasMaxLength(127);
            });
            modelBuilder.Entity<UserRole>(entity => {
                entity.Property(m => m.Name).HasMaxLength(127);
                entity.Property(m => m.NormalizedName).HasMaxLength(127);
            });
            modelBuilder.Entity<IdentityUserLogin<int>>(entity =>
            {
                entity.Property(m => m.LoginProvider).HasMaxLength(127);
                entity.Property(m => m.ProviderKey).HasMaxLength(127);
            });
            modelBuilder.Entity<IdentityUserRole<int>>(entity =>
            {
                entity.Property(m => m.UserId).HasMaxLength(127);
                entity.Property(m => m.RoleId).HasMaxLength(127);
            });
            modelBuilder.Entity<IdentityUserToken<int>>(entity =>
            {
                entity.Property(m => m.UserId).HasMaxLength(127);
                entity.Property(m => m.LoginProvider).HasMaxLength(127);
                entity.Property(m => m.Name).HasMaxLength(127);

            });

            // end fix mysql max length


            modelBuilder.Entity<Car>().HasKey(z => z.CarNo);

            modelBuilder.Entity<Invoice>()
                .HasOne(z => z.Car).WithMany(z => z.Invoices).HasForeignKey(z => z.CarNo);

            modelBuilder.Entity<Quote>()
                .HasOne(z => z.Car).WithMany(z => z.Quotes).HasForeignKey(z => z.CarNo);

            modelBuilder.Entity<Service>()
                .HasOne(z => z.Invoice)
                .WithMany(z => z.Services);

            modelBuilder.Entity<Service>()
                .HasOne(z => z.Quote)
                .WithMany(z => z.Services);


            // use data annotation
            //modelBuilder.Entity<ServiceIndex>()
            //    .Property(z => z.ServicePrice)
            //    .HasColumnType("decimal(8,2)");
                //.HasPrecision(8, 2); EF 5

            modelBuilder.Entity<Invoice>()
                .HasIndex(z => z.InvoiceNo).IsUnique();

            //modelBuilder.Entity<User>()
            //    .HasOne(u => u.RefreshToken)
            //    .WithOne(r => r.user)
            //    .HasForeignKey<RefreshToken>(r => r.UserId);
        }

    }
}

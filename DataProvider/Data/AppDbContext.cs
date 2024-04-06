using System;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DataProvider.Data
{
    public class AppDbContext : DbContext
    {
        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Transaction> Transaction { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>(entity =>
                {
                    entity.HasMany(d => d.Transactions)
                        .WithOne(d => d.Account)
                        .HasForeignKey(d => d.AccountId)
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("FK_Account_Transaction");
                });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasMany(d => d.Transactions)
                    .WithOne(d => d.Category)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Category_Transaction");
            });
        }
    }
}


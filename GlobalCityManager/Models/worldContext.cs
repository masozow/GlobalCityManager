using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GlobalCityManager.Models
{
    public partial class worldContext : DbContext
    {
        
        public worldContext()
        {
        }

        public worldContext(DbContextOptions<worldContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> City { get; set; }
        public virtual DbSet<Country> Country { get; set; }

//         protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//         {
//             if (!optionsBuilder.IsConfigured)
//             {
// //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                 optionsBuilder.UseMySql("Server=localhost;Database=world;Uid=root;Pwd=database;SslMode=none");
//             }
//         }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>(entity =>
            {
                entity.Property(e => e.CountryCode).HasDefaultValueSql("''");

                entity.Property(e => e.District).HasDefaultValueSql("''");

                entity.Property(e => e.Name).HasDefaultValueSql("''");

                entity.Property(e => e.Population).HasDefaultValueSql("'0'");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.Code)
                    .HasName("PRIMARY");

                entity.Property(e => e.Code).HasDefaultValueSql("''");

                entity.Property(e => e.Name).HasDefaultValueSql("''");

                entity.Property(e => e.Region).HasDefaultValueSql("''");
            });
        }
    }
}

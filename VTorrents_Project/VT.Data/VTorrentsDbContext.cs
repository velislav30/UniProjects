using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using VT.Models.Entities;

namespace VT.Data
{
    public class VTorrentsDbContext : DbContext
    {
        public VTorrentsDbContext() : base()
        {
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Torrent> Torrents { get; set; }
        public virtual DbSet<Catalog> Catalogs { get; set; }
        public virtual DbSet<SubType> SubTypes { get; set; }
        public virtual DbSet<UserToTorrent> UserToTorrents { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=NOTAPC\\SQLEXPRESS;Database=VTorrentNewDB;Integrated Security=true;");

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Catalog>()
                .Property(c => c.Title)
                .HasMaxLength(30)
                .IsRequired();

            modelBuilder.Entity<SubType>()
                .Property(c => c.Title)
                .HasMaxLength(30)
                .IsRequired();

            modelBuilder.Entity<Torrent>()
                .Property(c => c.Title)
                .HasMaxLength(30)
                .IsRequired();

            modelBuilder.Entity<Torrent>()
                .Property(c => c.Description)
                .HasMaxLength(300);

            modelBuilder.Entity<User>()
                .Property(c => c.Username)
                .HasMaxLength(30)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(c => c.Password)
                .HasMaxLength(30)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(c => c.FirstName)
                .HasMaxLength(30);

            modelBuilder.Entity<User>()
                .Property(c => c.LastName)
                .HasMaxLength(30);

            modelBuilder.Entity<User>()
                .Property(c => c.Email)
                .HasMaxLength(30);
        }
    }
}

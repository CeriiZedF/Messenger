using Messenger.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace Messenger.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Chats> chats { get; set; } = null!;
        public DbSet<User> users { get; set; } = null!;
        public DbSet<Content> contents { get; set; } = null!;

        public DataContext() : base() { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = App.GetConfiguration("smtp:connectionString");
            if (connectionString != null)
            {
                //var DbName = JsonSerializer.Deserialize<List<DateBase.DatabaseInfo>>("DataBase.json");
                //MessageBox.Show($"DataContext : {DbName?.First().DbName}");
                optionsBuilder.UseSqlServer(connectionString);
            }                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder  
                .Entity<User>()
                .HasMany(u => u.Chats)
                .WithOne()
                .HasForeignKey(c => c.Id_user)
                .HasPrincipalKey(u => u.Id);

            modelBuilder  
               .Entity<Content>()
               .HasOne(p => p.User)
               .WithMany()
               .HasForeignKey(p => p.Id_user)
               .HasPrincipalKey(c => c.Id);
        }
    }

    


}

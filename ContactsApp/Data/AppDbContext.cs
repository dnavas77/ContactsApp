﻿using Microsoft.EntityFrameworkCore;

namespace ContactsApp
{
    public class AppDbContext : DbContext
    {
        public DbSet<ContactsDataModel> Contacts { get; set; }

        public AppDbContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Server=DNAVAS-PC;Database=ContactsApp;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}

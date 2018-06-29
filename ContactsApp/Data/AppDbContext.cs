using Microsoft.EntityFrameworkCore;

namespace ContactsApp
{
    public class AppDbContext : DbContext
    {
        public DbSet<ContactsDataModel> Contacts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer("Server=DNAVAS-PC;Database=ContactsApp;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
        */

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ContactsDataModel>()
                .Property<string>("Groups_")
                .HasField("_groups");
        }
    }
}

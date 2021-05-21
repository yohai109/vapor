using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using vapor.Models;

namespace vapor.DAL

{
    public class VaporContext : DbContext
    {
        public VaporContext() : base("vaporContext")
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameImage> GameImages { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<ErrorViewModel> ErrorViewModels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
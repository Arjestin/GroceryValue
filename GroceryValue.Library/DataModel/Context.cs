using System.Data.Entity;

namespace GroceryValue.Library
{
    public class Context : DbContext
    {
        public DbSet<Chain> Chains { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ChainConfiguration());
            modelBuilder.Configurations.Add(new StoreConfiguration());
            modelBuilder.Configurations.Add(new ItemConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}

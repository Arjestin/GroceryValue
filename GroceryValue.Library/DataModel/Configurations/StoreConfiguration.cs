using System.Data.Entity.ModelConfiguration;

namespace GroceryValue.Library
{
    internal class StoreConfiguration : EntityTypeConfiguration<Store>
    {
        internal StoreConfiguration()
        {
            HasKey(store => store.StoreId).HasMany(store => store.Items).WithRequired(item => item.Store).HasForeignKey(item => item.StoreId);
        }
    }
}

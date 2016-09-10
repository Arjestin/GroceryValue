using System.Data.Entity.ModelConfiguration;

namespace GroceryValue.Library
{
    internal class ItemConfiguration : EntityTypeConfiguration<Item>
    {
        internal ItemConfiguration()
        {
            HasKey(item => item.ItemId);
        }
    }
}

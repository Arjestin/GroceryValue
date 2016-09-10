using System.Data.Entity.ModelConfiguration;

namespace GroceryValue.Library
{
    internal class ChainConfiguration : EntityTypeConfiguration<Chain>
    {
        internal ChainConfiguration()
        {
            HasKey(chain => chain.ChainId).HasMany(chain => chain.Stores).WithRequired(store => store.Chain).HasForeignKey(store => store.ChainId);
        }
    }
}

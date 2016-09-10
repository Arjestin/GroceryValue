using System.Data.Entity;
using System.Threading.Tasks;

namespace GroceryValue.Library
{
    public class Initializer : CreateDatabaseIfNotExists<Context>
    {
        protected override void Seed(Context context)
        {
            SeedAsync(context).Wait();
            base.Seed(context);
        }

        private static async Task SeedAsync(Context context)
        {
            AddChains(context);
            await context.SaveChangesAsync();
        }

        private static void AddChains(Context context)
        {
            var chains = Reader.ReadChains();
            context.Chains.AddRange(chains);
        }
    }
}

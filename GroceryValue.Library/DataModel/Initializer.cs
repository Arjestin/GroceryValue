using System.Data.Entity;
using System.Threading.Tasks;

namespace GroceryValue.Library
{
    public class Initializer : CreateDatabaseIfNotExists<Context>
    {
        protected override void Seed(Context context)
        {
            /*
             * The following line of code could cause a dealock
             * Solution: Task.Run(async () => { await SeedAsync(context); }).Wait();
             * Consider: https://msdn.microsoft.com/en-us/magazine/jj991977.aspx
            */

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

using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GroceryValue.Library
{
    // Making this class static allows changing some of its methods to extension methods, which is a really nice syntactic sugar.
    // However, WCF needs a service interface, which a static class cannot provide.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class Service : IService
    {
        public IEnumerable<Chain> GetChains()
        {
            List<Chain> chains;
            using (var context = new Context())
            {
                chains = context.Chains.ToList();
            }
            return chains;
        }

        public IEnumerable<Chain> GetSortedChains()
        {
            var chains = GetChains().ToList();
            chains.Sort(new ChainComparer());
            return chains;
        }

        public IEnumerable<Store> GetStores(long chainId)
        {
            List<Store> stores;
            using (var context = new Context())
            {
                stores = context.Stores.Where(store => store.ChainId == chainId).ToList();
            }
            return stores;
        }

        public IEnumerable<Store> GetSortedStores(long chainId)
        {
            var stores = GetStores(chainId).ToList();
            stores.Sort(new StoreComparer());
            return stores;
        }

        public IEnumerable<Item> GetItems(long storeId)
        {
            List<Item> items;
            using (var context = new Context())
            {
                items = context.Items.Where(item => item.StoreId == storeId).ToList();
            }
            return items;
        }

        public IEnumerable<Item> GetSortedItems(long storeId)
        {
            var items = GetItems(storeId).ToList();
            items.Sort(new ItemComparer());
            return items;
        }

        public IEnumerable<Grocery> GetGroceries(IEnumerable<long> storeIds)
        {
            var itemLists = storeIds.Select(GetItems).ToList();
            return itemLists.First().Where(item => itemLists.All(items => items.Any(i => i.Identifier == item.Identifier))).Select(item => new Grocery(item));
        }

        public IEnumerable<Grocery> GetSortedGroceries(IEnumerable<long> storeIds)
        {
            var groceries = GetGroceries(storeIds).ToList();
            groceries.Sort(new GroceryComparer());
            return groceries;
        }

        public IEnumerable<IEnumerable<float>> GetPriceLists(IEnumerable<long> storeIds, IEnumerable<long> groceryIds)
        {
            var itemLists = storeIds.Select(GetItems);
            return groceryIds.Select(groceryId => itemLists.Select(items => items.First(item => item.Identifier == groceryId).Price));
        }

        public IEnumerable<Item> GetMostExpensiveItems(long storeId)
        {
            return GetSortedItems(storeId).Reverse().Take(3);
        }

        public IEnumerable<Item> GetMostInexpensiveItems(long storeId)
        {
            return GetSortedItems(storeId).Take(3);
        }

        public async Task<bool> LoadItemsToDatabase(long storeId)
        {
            bool result;
            using (var context = new Context())
            {
                context.Database.Log += Logger.LogHandler;
                var store = context.Stores.Find(storeId);
                var chainIdentifier = context.Chains.First(c => c.ChainId == store.ChainId).Identifier;
                result = store.UpdateItems(chainIdentifier);
                await context.SaveChangesAsync();
            }
            return result;
        }
    }
}

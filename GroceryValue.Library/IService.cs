using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GroceryValue.Library
{
    /*
     * Seems like this interface, and whichever class implements it, will have a lot of different type of work on its hands
     * It will create a code base which hard to maintain and debug.
     
        Other than that - well done and well designed.

     Consider:
     a) https://en.wikipedia.org/wiki/Interface_segregation_principle
     b) https://en.wikipedia.org/wiki/Single_responsibility_principle
     c) https://en.wikipedia.org/wiki/Microservices
     d) https://en.wikipedia.org/wiki/Separation_of_concerns
     
         */
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        IEnumerable<Chain> GetChains();

        [OperationContract]
        IEnumerable<Chain> GetSortedChains();

        [OperationContract]
        IEnumerable<Store> GetStores(long chainId);

        [OperationContract]
        IEnumerable<Store> GetSortedStores(long chainId);

        [OperationContract]
        IEnumerable<Item> GetItems(long storeId);

        [OperationContract]
        IEnumerable<Item> GetSortedItems(long storeId);

        [OperationContract]
        IEnumerable<Grocery> GetGroceries(IEnumerable<long> storeIds);

        [OperationContract]
        IEnumerable<Grocery> GetSortedGroceries(IEnumerable<long> storeIds);

        [OperationContract]
        // The price lists are sorted according to the groceries' order.
        IEnumerable<IEnumerable<float>> GetPriceLists(IEnumerable<long> storeIds, IEnumerable<long> groceryIds);

        [OperationContract]
        IEnumerable<Item> GetMostExpensiveItems(long storeId);

        [OperationContract]
        IEnumerable<Item> GetMostInexpensiveItems(long storeId);

        [OperationContract]
        Task<bool> LoadItemsToDatabase(long storeId);
    }
}

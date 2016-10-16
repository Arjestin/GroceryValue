using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GroceryValue.Library
{
    //Very clean and good design!
    [DataContract]
    public class Store
    {
        public Store()
        {
            Items = new List<Item>();
        }

        [DataMember]
        public long StoreId { get; set; }

        public long ChainId { get; set; }
        public Chain Chain { get; set; }
        public long Identifier { get; set; }

        [DataMember]
        public string Name { get; set; }

        #region Additional Information

        public string Address { get; set; }
        public string City { get; set; }
        public int? Zip { get; set; }

        #endregion

        public ICollection<Item> Items { get; set; }
    }

    public class StoreComparer : IComparer<Store>
    {
        public int Compare(Store storeA, Store storeB)
        {
            return string.CompareOrdinal(storeA.Name, storeB.Name);
        }
    }
}

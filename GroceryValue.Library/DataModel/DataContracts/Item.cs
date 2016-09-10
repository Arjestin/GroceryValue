using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GroceryValue.Library
{
    [DataContract]
    public class Item
    {
        public Item()
        {
            Manufacturer = new Manufacturer();
        }

        public long ItemId { get; set; }
        public long StoreId { get; set; }
        public Store Store { get; set; }
        public long Identifier { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public float Price { get; set; }

        #region Additional Information

        public DateTime? DateAndTime { get; set; }
        public Manufacturer Manufacturer { get; set; }
        public bool? IsWeighted { get; set; }
        public string UnitOfMeasurement { get; set; }
        public float? Quantity { get; set; }
        public int? QuantityInPackage { get; set; }

        #endregion
    }

    public class ItemComparer : IComparer<Item>
    {
        public int Compare(Item itemA, Item itemB)
        {
            if (Math.Abs(itemA.Price - itemB.Price) < float.Epsilon)
            {
                return 0;
            }
            return itemA.Price > itemB.Price ? 1 : -1;
        }
    }
}

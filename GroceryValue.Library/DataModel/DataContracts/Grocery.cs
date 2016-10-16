using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GroceryValue.Library
{
    //Very clean and good design!
    [DataContract]
    public class Grocery
    {
        public Grocery(Item item)
        {
            GroceryId = item.Identifier;
            Name = item.Name;
        }

        [DataMember]
        public long GroceryId { get; set; }

        [DataMember]
        public string Name { get; set; }
    }

    public class GroceryComparer : IComparer<Grocery>
    {
        public int Compare(Grocery groceryA, Grocery groceryB)
        {
            return string.CompareOrdinal(groceryA.Name, groceryB.Name);
        }
    }
}

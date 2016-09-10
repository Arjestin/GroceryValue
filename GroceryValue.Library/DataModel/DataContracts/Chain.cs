using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace GroceryValue.Library
{
    [DataContract]
    public class Chain
    {
        public Chain()
        {
            Stores = new List<Store>();
        }

        [DataMember]
        public long ChainId { get; set; }

        public long Identifier { get; set; }

        [DataMember]
        public string Name { get; set; }

        #region Additional Information

        public DateTime? DateAndTime { get; set; }

        #endregion

        public ICollection<Store> Stores { get; set; }
    }

    public class ChainComparer : IComparer<Chain>
    {
        public int Compare(Chain chainA, Chain chainB)
        {
            return string.CompareOrdinal(chainA.Name, chainB.Name);
        }
    }
}

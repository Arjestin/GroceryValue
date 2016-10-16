using System.Collections.Generic;

namespace GroceryValue.Library
{
    //Very clean and good design!
    public class Manufacturer
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
    }

    public class ManufacturerComparer : IComparer<Manufacturer>
    {
        public int Compare(Manufacturer manufacturerA, Manufacturer manufacturerB)
        {
            return string.CompareOrdinal(manufacturerA.Name, manufacturerB.Name);
        }
    }
}

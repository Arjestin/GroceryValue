using GroceryValue.Client.ServiceReference;

namespace GroceryValue.Client
{
    public class SelectedGrocery
    {
        public SelectedGrocery(Grocery grocery, int quantity)
        {
            GroceryId = grocery.GroceryId;
            Name = grocery.Name;
            Quantity = quantity;
        }

        public long GroceryId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}

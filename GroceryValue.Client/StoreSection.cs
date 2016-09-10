using System.Collections.Generic;
using System.Windows.Forms;
using GroceryValue.Client.ServiceReference;

namespace GroceryValue.Client
{
    public class StoreSection
    {
        public Chain SelectedChain { get; set; }
        public IList<Store> Stores { get; set; }
        public Store SelectedStore { get; set; }
        public GroupBox GroupBox { get; set; }
        public TableLayoutPanel TableLayoutPanel { get; set; }
        public Label ChainLabel { get; set; }
        public ComboBox ChainComboBox { get; set; }
        public Label StoreLabel { get; set; }
        public ComboBox StoreComboBox { get; set; }
        public Label MostInexpensiveItemsTitleLabel { get; set; }
        public Label MostExpensiveItemsTitleLabel { get; set; }
        public Label MostInexpensiveItemsNameLabel { get; set; }
        public Label MostInexpensiveItemsPriceLabel { get; set; }
        public Label MostExpensiveItemsNameLabel { get; set; }
        public Label MostExpensiveItemsPriceLabel { get; set; }
        public Label MostInexpensiveItemsNamesLabel { get; set; }
        public Label MostInexpensiveItemsPricesLabel { get; set; }
        public Label MostExpensiveItemsNamesLabel { get; set; }
        public Label MostExpensiveItemsPricesLabel { get; set; }
        public ProgressBar ProgressBar { get; set; }
        public Label NotFoundLabel { get; set; }
        public Button LoadItemsButton { get; set; }
    }
}

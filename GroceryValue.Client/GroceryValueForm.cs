using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GroceryValue.Client.ServiceReference;

namespace GroceryValue.Client
{
    public partial class GroceryValueForm : Form
    {
        private static ServiceClient _client;
        private static IList<Chain> _chains;
        private static IList<StoreSection> _storeSections;
        private static IList<bool> _loadGroceriesButtonCanBeEnabled;
        private static IList<Label> _totalPriceLabels;
        private static IList<Grocery> _availableGroceries;
        private static IList<SelectedGrocery> _selectedGroceries;

        public GroceryValueForm(ServiceClient client)
        {
            _client = client;
            InitializeComponent();

            #region InitializeStoreSections

            _storeSections = new List<StoreSection>
            {
                new StoreSection
                {
                    GroupBox = Store1_GroupBox,
                    TableLayoutPanel = Store1_TableLayoutPanel,
                    ChainLabel = Store1_ChainLabel,
                    ChainComboBox = Store1_ChainComboBox,
                    StoreLabel = Store1_StoreLabel,
                    StoreComboBox = Store1_StoreComboBox,
                    MostInexpensiveItemsTitleLabel = Store1_MostInexpensiveItemsTitleLabel,
                    MostExpensiveItemsTitleLabel = Store1_MostExpensiveItemsTitleLabel,
                    MostInexpensiveItemsNameLabel = Store1_MostInexpensiveItemsNameLabel,
                    MostInexpensiveItemsPriceLabel = Store1_MostInexpensiveItemsPriceLabel,
                    MostExpensiveItemsNameLabel = Store1_MostExpensiveItemsNameLabel,
                    MostExpensiveItemsPriceLabel = Store1_MostExpensiveItemsPriceLabel,
                    MostInexpensiveItemsNamesLabel = Store1_MostInexpensiveItemsNamesLabel,
                    MostInexpensiveItemsPricesLabel = Store1_MostInexpensiveItemsPricesLabel,
                    MostExpensiveItemsNamesLabel = Store1_MostExpensiveItemsNamesLabel,
                    MostExpensiveItemsPricesLabel = Store1_MostExpensiveItemsPricesLabel,
                    ProgressBar = Store1_ProgressBar,
                    NotFoundLabel = Store1_NotFoundLabel,
                    LoadItemsButton = Store1_LoadItemsButton
                },
                new StoreSection
                {
                    GroupBox = Store2_GroupBox,
                    TableLayoutPanel = Store2_TableLayoutPanel,
                    ChainLabel = Store2_ChainLabel,
                    ChainComboBox = Store2_ChainComboBox,
                    StoreLabel = Store2_StoreLabel,
                    StoreComboBox = Store2_StoreComboBox,
                    MostInexpensiveItemsTitleLabel = Store2_MostInexpensiveItemsTitleLabel,
                    MostExpensiveItemsTitleLabel = Store2_MostExpensiveItemsTitleLabel,
                    MostInexpensiveItemsNameLabel = Store2_MostInexpensiveItemsNameLabel,
                    MostInexpensiveItemsPriceLabel = Store2_MostInexpensiveItemsPriceLabel,
                    MostExpensiveItemsNameLabel = Store2_MostExpensiveItemsNameLabel,
                    MostExpensiveItemsPriceLabel = Store2_MostExpensiveItemsPriceLabel,
                    MostInexpensiveItemsNamesLabel = Store2_MostInexpensiveItemsNamesLabel,
                    MostInexpensiveItemsPricesLabel = Store2_MostInexpensiveItemsPricesLabel,
                    MostExpensiveItemsNamesLabel = Store2_MostExpensiveItemsNamesLabel,
                    MostExpensiveItemsPricesLabel = Store2_MostExpensiveItemsPricesLabel,
                    ProgressBar = Store2_ProgressBar,
                    NotFoundLabel = Store2_NotFoundLabel,
                    LoadItemsButton = Store2_LoadItemsButton
                },
                new StoreSection
                {
                    GroupBox = Store3_GroupBox,
                    TableLayoutPanel = Store3_TableLayoutPanel,
                    ChainLabel = Store3_ChainLabel,
                    ChainComboBox = Store3_ChainComboBox,
                    StoreLabel = Store3_StoreLabel,
                    StoreComboBox = Store3_StoreComboBox,
                    MostInexpensiveItemsTitleLabel = Store3_MostInexpensiveItemsTitleLabel,
                    MostExpensiveItemsTitleLabel = Store3_MostExpensiveItemsTitleLabel,
                    MostInexpensiveItemsNameLabel = Store3_MostInexpensiveItemsNameLabel,
                    MostInexpensiveItemsPriceLabel = Store3_MostInexpensiveItemsPriceLabel,
                    MostExpensiveItemsNameLabel = Store3_MostExpensiveItemsNameLabel,
                    MostExpensiveItemsPriceLabel = Store3_MostExpensiveItemsPriceLabel,
                    MostInexpensiveItemsNamesLabel = Store3_MostInexpensiveItemsNamesLabel,
                    MostInexpensiveItemsPricesLabel = Store3_MostInexpensiveItemsPricesLabel,
                    MostExpensiveItemsNamesLabel = Store3_MostExpensiveItemsNamesLabel,
                    MostExpensiveItemsPricesLabel = Store3_MostExpensiveItemsPricesLabel,
                    ProgressBar = Store3_ProgressBar,
                    NotFoundLabel = Store3_NotFoundLabel,
                    LoadItemsButton = Store3_LoadItemsButton
                }
            };

            #endregion

            _loadGroceriesButtonCanBeEnabled = new List<bool>
            {
                false,
                false,
                false
            };
            _totalPriceLabels = new List<Label>
            {
                Store1_TotalLabel,
                Store2_TotalLabel,
                Store3_TotalLabel
            };
        }

        private async void GroceryWarriorForm_LoadAsync(object sender, EventArgs e)
        {
            _chains = await Task.Run(() => _client.GetSortedChains());
            foreach (var section in _storeSections)
            {
                section.ChainComboBox.DataSource = _chains.Select(chain => chain.Name).ToList();
            }
        }

        #region StoresTabPageEventHandlers

        private async void ChainComboBox_SelectedValueChangedAsync(object sender, EventArgs e)
        {
            var section = GetStoreSection(sender);
            var selectedChainIndex = section.ChainComboBox.SelectedIndex;
            section.SelectedChain = _chains[selectedChainIndex];
            section.Stores = await Task.Run(() => _client.GetSortedStores(section.SelectedChain.ChainId));
            section.StoreComboBox.DataSource = section.Stores.Select(store => store.Name).ToList();
        }

        private async void StoreComboBox_SelectedValueChangedAsync(object sender, EventArgs e)
        {
            var section = GetStoreSection(sender);
            section.SelectedStore = section.Stores[section.StoreComboBox.SelectedIndex];
            SetLoadGroceriesButton(_storeSections.IndexOf(section), false);
            SetTopItemLabels(section, false);
            SetStoreNotFoundControls(section, false);
            SetStoreProgressBar(section, true);
            var mostInexpensiveItems = await Task.Run(() => _client.GetMostInexpensiveItems(section.SelectedStore.StoreId));
            var mostExpensiveItems = await Task.Run(() => _client.GetMostExpensiveItems(section.SelectedStore.StoreId));
            SetStoreProgressBar(section, false);
            if (mostInexpensiveItems.Count == 0 || mostExpensiveItems.Count == 0)
            {
                SetStoreNotFoundControls(section, true);
                return;
            }
            DisplayTopItems(section, mostInexpensiveItems, mostExpensiveItems);
            SetTopItemLabels(section, true);
            SetLoadGroceriesButton(_storeSections.IndexOf(section), true);
        }

        private async void Store_LoadItemsButton_Click(object sender, EventArgs e)
        {
            var section = GetStoreSection(sender);
            SetStoreNotFoundControls(section, false);
            SetStoreProgressBar(section, true);
            SetStoreComboBoxes(section, false);
            var result = await Task.Run(() => _client.LoadItemsToDatabase(section.SelectedStore.StoreId));
            SetStoreComboBoxes(section, true);
            if (result == false)
            {
                SetStoreProgressBar(section, false);
                SetStoreNotFoundControls(section, true);
                MessageBox.Show("טעינת החנות נכשלה");
                return;
            }
            StoreComboBox_SelectedValueChangedAsync(section.StoreComboBox, e);
        }

        private async void LoadGroceriesButton_ClickAsync(object sender, EventArgs e)
        {
            SetLoadGroceriesControls(true);
            var selectedStoreIds = _storeSections.Select(section => section.SelectedStore.StoreId).ToList();
            _availableGroceries = await Task.Run(() => _client.GetSortedGroceries(selectedStoreIds));
            _selectedGroceries = new List<SelectedGrocery>();
            if (_availableGroceries.Count == 0)
            {
                LoadGroceriesLabel.ForeColor = Color.DarkRed;
                LoadGroceriesLabel.Text = "לא נמצאו מוצרים להשוואה";
                AvailableGroceriesNamesListBox.DataSource = null;
                SelectedGroceriesNamesListBox.DataSource = null;
                SelectedGroceriesQuantityListBox.DataSource = null;
                AvailableGroceriesNumericUpDown.Enabled = false;
                AddButton.Enabled = false;
            }
            else
            {
                LoadGroceriesLabel.ForeColor = Color.DarkGreen;
                LoadGroceriesLabel.Text = $"נמצאו {_availableGroceries.Count} מוצרים להשוואה";
                AvailableGroceriesNamesListBox.DataSource = _availableGroceries.Select(grocery => grocery.Name).ToList();
                SelectedGroceriesNamesListBox.DataSource = _selectedGroceries.Select(grocery => grocery.Name).ToList();
                SelectedGroceriesQuantityListBox.DataSource = _selectedGroceries.Select(grocery => grocery.Quantity).ToList();
                AvailableGroceriesNumericUpDown.Enabled = true;
                AddButton.Enabled = true;
            }
            SetLoadGroceriesControls(false);
        }

        #endregion

        #region StoresTabPageEventHandlerExtensions

        private static StoreSection GetStoreSection(object sender)
        {
            foreach (var section in _storeSections)
            {
                IEnumerable<bool> conditions = new[]
                {
                    sender.GetType() == typeof(ComboBox) && (section.ChainComboBox == sender || section.StoreComboBox == sender),
                    sender.GetType() == typeof(Button) && section.LoadItemsButton == sender
                };
                if (conditions.Any(condition => condition))
                {
                    return section;
                }
            }
            throw new GroceryValueException($"GroceryValueException: Control {sender.GetType()} {sender} triggered an unsupported event.");
        }

        private void SetLoadGroceriesButton(int sectionIndex, bool setting)
        {
            _loadGroceriesButtonCanBeEnabled[sectionIndex] = setting;
            LoadGroceriesButton.Enabled = _loadGroceriesButtonCanBeEnabled.All(condition => condition);
        }

        private static void SetTopItemLabels(StoreSection section, bool setting)
        {
            section.MostInexpensiveItemsTitleLabel.Visible = setting;
            section.MostExpensiveItemsTitleLabel.Visible = setting;
            section.MostInexpensiveItemsNameLabel.Visible = setting;
            section.MostInexpensiveItemsPriceLabel.Visible = setting;
            section.MostExpensiveItemsNameLabel.Visible = setting;
            section.MostExpensiveItemsPriceLabel.Visible = setting;
            section.MostInexpensiveItemsNamesLabel.Visible = setting;
            section.MostInexpensiveItemsPricesLabel.Visible = setting;
            section.MostExpensiveItemsNamesLabel.Visible = setting;
            section.MostExpensiveItemsPricesLabel.Visible = setting;
        }

        private static void SetStoreNotFoundControls(StoreSection section, bool setting)
        {
            section.NotFoundLabel.Visible = setting;
            section.LoadItemsButton.Visible = setting;
        }

        private static void SetStoreProgressBar(StoreSection section, bool setting)
        {
            if (setting)
            {
                section.ProgressBar.Show();
            }
            else
            {
                section.ProgressBar.Hide();
                section.ProgressBar.Value = 0;
            }
        }

        private static void DisplayTopItems(StoreSection section, IEnumerable<Item> mostInexpensiveItems, IEnumerable<Item> mostExpensiveItems)
        {
            var mostInexpensiveItemsNames = new StringBuilder();
            var mostInexpensiveItemsPrices = new StringBuilder();
            var mostExpensiveItemsNames = new StringBuilder();
            var mostExpensiveItemsPrices = new StringBuilder();
            foreach (var item in mostInexpensiveItems)
            {
                mostInexpensiveItemsNames.AppendLine(item.Name);
                mostInexpensiveItemsPrices.AppendLine(item.Price.ToString(new CultureInfo("he-IL")));
            }
            foreach (var item in mostExpensiveItems)
            {
                mostExpensiveItemsNames.AppendLine(item.Name);
                mostExpensiveItemsPrices.AppendLine(item.Price.ToString(new CultureInfo("he-IL")));
            }
            section.MostInexpensiveItemsNamesLabel.Text = mostInexpensiveItemsNames.ToString();
            section.MostInexpensiveItemsPricesLabel.Text = mostInexpensiveItemsPrices.ToString();
            section.MostExpensiveItemsNamesLabel.Text = mostExpensiveItemsNames.ToString();
            section.MostExpensiveItemsPricesLabel.Text = mostExpensiveItemsPrices.ToString();
        }

        private static void SetStoreComboBoxes(StoreSection section, bool setting)
        {
            section.ChainComboBox.Enabled = setting;
            section.StoreComboBox.Enabled = setting;
        }

        private void SetLoadGroceriesControls(bool setting)
        {
            if (setting)
            {
                LoadGroceriesLabel.Visible = false;
                LoadGroceriesButton.Enabled = false;
                LoadGroceriesProgressBar.Show();
            }
            else
            {
                LoadGroceriesProgressBar.Hide();
                LoadGroceriesProgressBar.Value = 0;
                LoadGroceriesButton.Enabled = true;
                LoadGroceriesLabel.Visible = true;
            }
        }

        #endregion

        #region GroceriesTabPageEventHandlers

        private void AddButton_Click(object sender, EventArgs e)
        {
            SelectedGroceriesQuantityListBox.DataSource = null;
            var selectedGroceryIndex = AvailableGroceriesNamesListBox.SelectedIndex;
            var selectedGrocery = _selectedGroceries.FirstOrDefault(grocery => grocery.GroceryId == _availableGroceries[selectedGroceryIndex].GroceryId);
            if (selectedGrocery != null)
            {
                selectedGrocery.Quantity += (int) AvailableGroceriesNumericUpDown.Value;
            }
            else
            {
                SelectedGroceriesNamesListBox.DataSource = null;
                _selectedGroceries.Add(new SelectedGrocery(_availableGroceries[selectedGroceryIndex], (int) AvailableGroceriesNumericUpDown.Value));
                SelectedGroceriesNamesListBox.DataSource = _selectedGroceries.Select(grocery => grocery.Name).ToList();
                SelectedGroceriesNumericUpDown.Enabled = true;
                RemoveButton.Enabled = true;
                LoadComparisonButton.Enabled = true;
            }
            SelectedGroceriesQuantityListBox.DataSource = _selectedGroceries.Select(grocery => grocery.Quantity).ToList();
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            SelectedGroceriesQuantityListBox.DataSource = null;
            var selectedGroceryIndex = SelectedGroceriesNamesListBox.SelectedIndex;
            if (_selectedGroceries[selectedGroceryIndex].Quantity - (int) SelectedGroceriesNumericUpDown.Value > 0)
            {
                _selectedGroceries[selectedGroceryIndex].Quantity -= (int) SelectedGroceriesNumericUpDown.Value;
            }
            else
            {
                SelectedGroceriesNamesListBox.DataSource = null;
                _selectedGroceries.RemoveAt(selectedGroceryIndex);
                SelectedGroceriesNamesListBox.DataSource = _selectedGroceries.Select(grocery => grocery.Name).ToList();
                if (_selectedGroceries.Count == 0)
                {
                    LoadComparisonButton.Enabled = false;
                    RemoveButton.Enabled = false;
                    SelectedGroceriesNumericUpDown.Enabled = false;
                }
            }
            SelectedGroceriesQuantityListBox.DataSource = _selectedGroceries.Select(grocery => grocery.Quantity).ToList();
        }

        private async void LoadComparisonButton_ClickAsync(object sender, EventArgs e)
        {
            SetLoadComparisonControls(true);
            SetComparisonTabPage(false);
            var selectedStoreIds = _storeSections.Select(section => section.SelectedStore.StoreId).ToList();
            var selectedGroceryIds = _selectedGroceries.Select(grocery => grocery.GroceryId).ToList();
            var priceLists = await Task.Run(() => _client.GetPriceLists(selectedStoreIds, selectedGroceryIds));
            DisplayComparison(priceLists);
            SetComparisonTabPage(true);
            SetLoadComparisonControls(false);
        }

        #endregion

        #region StoresTabPageEventHandlerExtensions

        private void SetLoadComparisonControls(bool setting)
        {
            if (setting)
            {
                LoadComparisonLabel.Visible = false;
                LoadComparisonButton.Enabled = false;
                LoadComparisonProgressBar.Show();
            }
            else
            {
                LoadComparisonProgressBar.Hide();
                LoadComparisonProgressBar.Value = 0;
                LoadComparisonButton.Enabled = true;
                LoadComparisonLabel.Visible = true;
            }
        }

        private void SetComparisonTabPage(bool setting)
        {
            ComparisonDataGridView.Visible = setting;
            TotalTableLayoutPanel.Visible = setting;
        }

        private void DisplayComparison(IEnumerable<List<float>> priceLists)
        {
            var totalPrices = _storeSections.Select(section => new float()).ToList();
            ComparisonDataGridView.DataSource = null;
            using (var comparisonTable = new DataTable())
            {
                comparisonTable.Columns.Add("כמות", typeof(int));
                comparisonTable.Columns.Add("מוצר", typeof(string));
                foreach (var section in _storeSections)
                {
                    comparisonTable.Columns.Add(section.SelectedChain.Name + Environment.NewLine + section.SelectedStore.Name, typeof(string));
                }
                var comparisonRows = _selectedGroceries.Zip(priceLists, (grocery, prices) => new {Grocery = grocery, Prices = prices});
                foreach (var comparisonRow in comparisonRows)
                {
                    var i = 0;
                    var dataRow = comparisonTable.NewRow();
                    dataRow[i++] = comparisonRow.Grocery.Quantity;
                    dataRow[i++] = comparisonRow.Grocery.Name;
                    foreach (var price in comparisonRow.Prices)
                    {
                        var totalPrice = price*comparisonRow.Grocery.Quantity;
                        dataRow[i++] = $"{totalPrice:C2}";
                        totalPrices[comparisonRow.Prices.IndexOf(price)] += totalPrice;
                    }
                    comparisonTable.Rows.Add(dataRow);
                }
                ComparisonDataGridView.DataSource = comparisonTable;
            }
            var totalPricesAndLabels = totalPrices.Zip(_totalPriceLabels, (price, label) => new {Price = price, Label = label});
            foreach (var totalPriceAndLabel in totalPricesAndLabels)
            {
                totalPriceAndLabel.Label.Text = $"{totalPriceAndLabel.Price:C2}";
            }
        }

        #endregion
    }
}

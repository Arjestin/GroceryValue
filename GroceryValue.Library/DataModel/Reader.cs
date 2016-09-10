using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;

namespace GroceryValue.Library
{
    public static class Reader
    {
        public static IEnumerable<Chain> ReadChains()
        {
            var chains = new List<Chain>();
            var resources = $@"{BaseDirectory.GetBaseDirectory()}\GroceryValue.Resources";
            var directories = Directory.GetDirectories(resources);
            if (directories.Length == 0)
            {
                throw new GroceryValueException($"GroceryValueException: No chain directories were found in {resources}.");
            }
            foreach (var directory in directories)
            {
                var file = Directory.GetFiles(directory, "Stores*.xml").FirstOrDefault();
                if (string.IsNullOrEmpty(file))
                {
                    throw new GroceryValueException($"GroceryValueException: No stores file was found in {directory}.");
                }
                var document = XDocument.Load(file);
                var node = document.XPathSelectElement("/Root");
                var chain = new Chain
                {
                    Identifier = node.ReadLongInteger("ChainId"),
                    Name = node.ReadString("ChainName")
                };
                if (chain.Identifier == 0 || string.IsNullOrEmpty(chain.Name) || chains.Select(c => c.Identifier).Contains(chain.Identifier))
                {
                    continue;
                }
                //node.ReadAdditionalInformation(chain);
                chain.Stores = document.ReadStores(directory) as List<Store>;
                chains.Add(chain);
            }
            return chains;
        }

        private static IEnumerable<Store> ReadStores(this XNode document, string directory)
        {
            var stores = new List<Store>();
            var nodes = document.XPathSelectElements("//Store");
            foreach (var node in nodes)
            {
                var store = new Store
                {
                    Identifier = node.ReadLongInteger("StoreId"),
                    Name = node.ReadString("StoreName")
                };
                if (store.Identifier == 0 || string.IsNullOrEmpty(store.Name) || stores.Select(s => s.Identifier).Contains(store.Identifier))
                {
                    continue;
                }
                //node.ReadAdditionalInformation(store);
                var file = Directory.GetFiles(directory, $"PriceFull*-{store.Identifier:D3}-*.xml").FirstOrDefault();
                if (!string.IsNullOrEmpty(file))
                {
                    store.Items = file.ReadItems() as List<Item>;
                }
                stores.Add(store);
            }
            return stores;
        }

        private static IEnumerable<Item> ReadItems(this string file)
        {
            var items = new List<Item>();
            var document = XDocument.Load(file);
            var nodes = document.XPathSelectElements("//Item");
            foreach (var node in nodes)
            {
                var item = new Item
                {
                    Identifier = node.ReadLongInteger("ItemCode"),
                    Name = node.ReadString("ItemName"),
                    Price = node.ReadFloatingPoint("ItemPrice")
                };
                if (item.Identifier == 0 || string.IsNullOrEmpty(item.Name) || Math.Abs(item.Price) < float.Epsilon || items.Select(i => i.Identifier).Contains(item.Identifier))
                {
                    continue;
                }
                //node.ReadAdditionalInformation(item);
                items.Add(item);
            }
            return items;
        }

        public static bool UpdateItems(this Store store, long chainIdentifier)
        {
            var directory = $@"{BaseDirectory.GetBaseDirectory()}\GroceryValue.Resources\{chainIdentifier}";
            var file = Directory.GetFiles(directory, $"PriceFull*-{store.Identifier:D3}-*.xml").FirstOrDefault();
            if (string.IsNullOrEmpty(file))
            {
                return false;
            }
            store.Items = file.ReadItems() as List<Item>;
            return true;
        }

        private static void ReadAdditionalInformation(this XContainer node, Chain chain)
        {
            // TODO: Read LastUpdateDate and LastUpdateTime to chain.DateAndTime
        }

        private static void ReadAdditionalInformation(this XContainer node, Store store)
        {
            store.Address = node.TryReadString("Address");
            store.City = node.TryReadString("City");
            store.Zip = node.TryReadInteger("ZipCode");
        }

        private static void ReadAdditionalInformation(this XContainer node, Item item)
        {
            // TODO: Read PriceUpdateDate to Item.DateAndTime
            item.Manufacturer.Name = node.TryReadString("ManufacturerName");
            item.Manufacturer.Country = node.TryReadString("ManufactureCountry");
            item.Manufacturer.Description = node.TryReadString("ManufacturerItemDescription");
            item.IsWeighted = node.ReadBoolean("bIsWeighted");
            item.UnitOfMeasurement = node.TryReadString("UnitOfMeasure");
            item.Quantity = node.TryReadFloatingPoint("Quantity");
            item.QuantityInPackage = node.TryReadInteger("QtyInPackage");
        }

        private static string ReadString(this XContainer node, string elementName)
        {
            return node.Element(elementName)?.Value.Trim();
        }

        private static string TryReadString(this XContainer node, string elementName)
        {
            var elementValue = node.ReadString(elementName);
            IEnumerable<bool> conditions = new[]
            {
                string.Compare(elementValue, "unknown", StringComparison.OrdinalIgnoreCase) != 0,
                string.Compare(elementValue, "לא ידוע", StringComparison.OrdinalIgnoreCase) != 0
            };
            return conditions.All(condition => condition) ? elementValue : null;
        }

        private static int ReadInteger(this XContainer node, string elementName)
        {
            var elementValue = node.ReadString(elementName);
            int integer;
            int.TryParse(elementValue, out integer);
            return integer;
        }

        private static int? TryReadInteger(this XContainer node, string elementName)
        {
            var integer = node.ReadInteger(elementName);
            return integer != 0 ? integer as int? : null;
        }

        private static float ReadFloatingPoint(this XContainer node, string elementName)
        {
            var elementValue = node.ReadString(elementName);
            float floatingPoint;
            float.TryParse(elementValue, out floatingPoint);
            return floatingPoint;
        }

        private static float? TryReadFloatingPoint(this XContainer node, string elementName)
        {
            var floatingPoint = node.ReadFloatingPoint(elementName);
            return Math.Abs(floatingPoint) > float.Epsilon ? floatingPoint as float? : null;
        }

        private static long ReadLongInteger(this XContainer node, string elementName)
        {
            var elementValue = node.ReadString(elementName);
            long longInteger;
            long.TryParse(elementValue, out longInteger);
            return longInteger;
        }

        private static bool ReadBoolean(this XContainer node, string elementName)
        {
            var elementValue = node.ReadString(elementName);
            bool boolean;
            bool.TryParse(elementValue, out boolean);
            return boolean;
        }
    }
}

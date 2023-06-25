using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterGameConcept
{
    public class Item
    {
        public string Name { get; }
        public uint Quantity { get; set; }
        public StatusCondition Status { get; private set; }
        public int Healing { get; private set; }
        public int Damage { get; private set; }

        public Item(string name, uint quantity = 0, StatusCondition status = StatusCondition.Empty, int healing = 0, int damage = 0)
        {
            Name = name;
            Quantity = quantity;
            Status = status;
            Healing = healing;
            Damage = damage;
        }
    }

    public class Inventory
    {
        private Dictionary<string, Item> items;

        public Inventory()
        {
            items = new Dictionary<string, Item>();
        }

        public void AddItem(string itemName, uint quantity = 1)
        {
            if (items.ContainsKey(itemName))
            {
                items[itemName].Quantity += quantity;
            }
            else
            {
                Item newItem = new Item(itemName, quantity);
                items.Add(itemName, newItem);
            }
        }

        public void RemoveItem(string itemName, uint quantity = 1)
        {
            if (items.ContainsKey(itemName))
            {
                items[itemName].Quantity -= quantity;
                if (items[itemName].Quantity <= 0)
                {
                    items.Remove(itemName);
                }
            }
        }

        public void UseItem(string itemName, Monster mon)
        {
            if (!items.ContainsKey(itemName))
            {
                //Item is not in dict, return errow
                throw new Exception("Item not in dictionary!");
            }


            if (items[itemName].Quantity == 0)
            {
                //Cant use item, have none in backpack
                return;
            }

            if (items[itemName].Healing > 0)
            {
                mon.Heal(items[itemName].Healing);
            }
            if (items[itemName].Damage > 0)
            {
                mon.TakeDamage(items[itemName].Damage);
            }
            if (items[itemName].Status == mon.Status)
            {
                mon.Status = StatusCondition.Empty;
            }
            RemoveItem(items[itemName].Name);
        }

        public uint GetItemQuantity(string itemName)
        {
            if (items.ContainsKey(itemName))
            {
                return items[itemName].Quantity;
            }
            return 0;
        }

        public void DisplayInventory()
        {
            foreach (var item in items)
            {
                Console.WriteLine($"{item.Key}: {item.Value.Quantity}");
            }
        }


        //Can it even be in this class?
        public void SaveToFile(string filePath)
        {
            string jsonContent = JsonSerializer.Serialize(items.Values, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonContent);
        }

        public void LoadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonContent = File.ReadAllText(filePath);
                items = JsonSerializer.Deserialize<Dictionary<string, Item>>(jsonContent);
            }
        }
    }
}


//Inventory inventory = new Inventory();

//// Add items to the inventory
//inventory.AddItem("Potion", 5);
//inventory.AddItem("Poké Ball", 10);
//inventory.AddItem("Revive");

//// Remove items from the inventory
//inventory.RemoveItem("Potion", 2);
//inventory.RemoveItem("Rare Candy");

//// Retrieve item quantities
//int potionQuantity = inventory.GetItemQuantity("Potion");
//int pokeBallQuantity = inventory.GetItemQuantity("Poké Ball");

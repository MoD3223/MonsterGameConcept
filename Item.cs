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
        public bool QuestItem;
        public Move? move;
        public uint CatchChance;

        public Item(string name, uint quantity = 0, bool quest = false, StatusCondition status = StatusCondition.Empty, int healing = 0, int damage = 0, Move? m = null, uint catchChance = 0)
        {
            Name = name;
            Quantity = quantity;
            QuestItem = quest;
            Status = status;
            Healing = healing;
            Damage = damage;
            move = m;
            CatchChance = catchChance;
        }
    }

    public class Inventory
    {
        private Dictionary<string, Item> items; //Items usable in inventory
        public Dictionary<string, Item> AllItems; //ALL items avaible


        //TODO: Add reading list of all avaible items from file
        //TODO: Add saving Inventory to file and other stuff as savefile

        public Inventory()
        {
            items = new Dictionary<string, Item>();
            AllItems = LoadFromFile(""); //TODO: Fix this :p
        }

        public void AddItem(string itemName, uint quantity = 1)
        {
            if (items.ContainsKey(itemName))
            {
                items[itemName].Quantity += quantity;
            }
            else
            {
                items.Add(itemName, AllItems[itemName]);
            }
        }

        public void RemoveItem(string itemName, uint quantity = 1)
        {
            if (items.ContainsKey(itemName))
            {
                if (items[itemName].Quantity < quantity)
                {
                    throw new Exception("Jak???");
                }
                else if (items[itemName].Quantity == quantity)
                {
                    items.Remove(itemName);
                }
                else
                {
                    items[itemName].Quantity -= quantity;
                }
            }
        }

        public void UseItem(string itemName,ref Monster mon)
        {
            if (!items.ContainsKey(itemName))
            {
                //Item is not in dict, return errow
                throw new Exception("Item not in dictionary!");
            }
            else if (items[itemName].Quantity == 0)
            {
                //Cant use item, have none in backpack
                return;
            }
            else if (items[itemName].QuestItem) {
                //Cant use item, its for quests
                return;
            }
            else
            {
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
                if (items[itemName].move != null)
                {
                    mon.Moves.Add(items[itemName].move);
                }
                if (items[itemName].CatchChance != 0)
                {
                    MainPage.TryCatch(ref mon, items[itemName]);
                    //TODO:???????????? Make it so you can use it on your own pokemon??
                }
                RemoveItem(items[itemName].Name);
            }
        }

        //Public getter for private Dict
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

        public Dictionary<string,Item> LoadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonContent = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<Dictionary<string, Item>>(jsonContent);
            }
            //There should be also some text on UI indicated we hit this.
            return new Dictionary<string, Item>();
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

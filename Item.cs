using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
        public ItemPurpose Purpose { get; private set; }
        public Move? move;
        public uint CatchChance;

        public Item(string name, uint quantity = 0, ItemPurpose p = ItemPurpose.Normal, StatusCondition status = StatusCondition.Empty, int healing = 0, int damage = 0, Move? m = null, uint catchChance = 0)
        {
            Name = name;
            Quantity = quantity;
            Purpose = p;
            Status = status;
            Healing = healing;
            Damage = damage;
            move = m;
            CatchChance = catchChance;
        }

        public enum ItemPurpose
        {
            Normal,
            Quest,
            Tool,
            Cooking
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
                if (items[itemName].Purpose == Item.ItemPurpose.Tool)
                {
                    items[itemName].Quantity = quantity;
                }
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

        public void UseItem(string itemName,ref Monster? mon)
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
            else if (items[itemName].Purpose == Item.ItemPurpose.Quest) {
                //Cant use item, its for quests
                return;
            }
            else
            {
                if (mon != null)
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
                        if (mon.Moves.Contains(items[itemName].move))
                        {
                            //Monster already knows item, can't learn it again
                        }
                        else
                        {
                            mon.Moves.Add(items[itemName].move);
                        }
                        
                    }
                    if (items[itemName].CatchChance != 0)
                    {
                        MainPage.TryCatch(ref mon, items[itemName]);
                        //TODO:???????????? Test this stuff, it really smells
                    }
                }
                else
                {
                    CheckHit(items[itemName]);
                }
                RemoveItem(items[itemName].Name); //For non-use items (Pickaxe, Fishing rod etc) the quantity will serve as durability
            }
        }

        public static void CheckHit(Item item)
        {
            //TODO: Make it so you set the item "in-hand" via menu and when you press ctrl it tries to use that item
            if (item.Purpose == Item.ItemPurpose.Tool)
            {
                if (item.Name.Contains("Pickaxe", StringComparison.OrdinalIgnoreCase))
                {
                    //TODO: Add this to update
                    //Destroyes the Rock, in Update();
                    //RaycastHit hit;
                    //if (Physics.Raycast(transform.position, transform.forward, out hit))
                    //{
                    //    if (hit.collider.CompareTag("Rock"))
                    //    {
                    //        Destroy(hit.collider.gameObject);
                    //    }
                    //}
                    //Plays animation
                    //pickaxeAnimator.SetTrigger("Use"); // Play the "Use" animation

                }
                else if (item.Name.Contains("Fishing rod", StringComparison.OrdinalIgnoreCase))
                {
                    //Idea: You cant move while fishing, after 3 seconds you press CTRL again to fishout item, all avaible fishing items will be in a small list and you will get random 1 item.
                    //Idea: Small % (like 5) to trigger a fight
                    //Idea: Fishes as usable items
                }
            }

            
        }

        public void Cook(Item item)
        {
            if (item.Purpose == Item.ItemPurpose.Cooking)
            {
                string tmp = item.Name.Replace("Raw","Cooked");
                MainPage.inv.RemoveItem(item.Name);
                MainPage.inv.AddItem(tmp);
                //Cooked the item
            }
            else
            {
                //Cant cook the item
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

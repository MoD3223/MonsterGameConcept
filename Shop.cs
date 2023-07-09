using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterGameConcept
{
    public static class Shop
    {
        public static Dictionary<Item, uint> ShopItems = new Dictionary<Item, uint>();

        public static void BuyItem(KeyValuePair<Item,uint> item,ref uint money)
        {
            uint quantity = 1;
            //add some ui to choose quantity

            if (item.Value * quantity > money)
            {
                //not enough money, cant buy it
            }
            else
            {
                money -= (uint)(item.Value * quantity);
                MainPage.inv.AddItem(item.Key.Name, quantity);
            }

        }

        public static void SellItem(KeyValuePair<Item,uint> item, ref uint money)
        {
            uint quantity = 1;

            if (quantity > MainPage.inv.GetItemQuantity(item.Key.Name))
            {
                //Cant sell, you chose too high quantity
            }
            else
            {
                //We get 1/3rd of the item value when selling it
                money += (item.Value * quantity) / 3;
            }
        }










    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterGameConcept
{
    public class Quest
    {
        public string questName;
        public string questDescription;
        public Dictionary<string, QuestTasks> questTasks;
        public Dictionary<string,Item>? requiredItems;
        public Dictionary<uint, Statistic>? requiredMonsters; //Cant have two monsters with same uint!!!
        public Dictionary<string, Item> itemRewards;
        public QuestStatus questStatus;
        public bool repeatable;

        //TODO: Add quest log and repeatable quests, maybe even quest conditions


        public enum QuestStatus
        {
            Unavaible,
            Avaible,
            Started,
            Ready,
            Completed
        }

        //Use new Item for each quest so dictionaries don't overlap
        public Quest(string qname, string qdesc, Dictionary<string, QuestTasks> qtasks, Dictionary<string, Item>? items, Dictionary<uint, Statistic>? mons, Dictionary<string, Item> rewards, QuestStatus qs = QuestStatus.Avaible, bool re = false)
        {
            questName = qname;
            questDescription = qdesc;
            questTasks = qtasks;
            requiredItems = items;
            requiredMonsters = mons;
            itemRewards = rewards;
            questStatus = qs;
            repeatable = re;
        }


        public void QuestStarted()
        {
            this.questStatus = QuestStatus.Started;
            //Add some console text or smth here :p

            foreach (var item in requiredMonsters)
            {
                if (RandomEncounterGenerator.allAvaibleMonsters.Any(stat => stat.Name == item.Value.Name))
                {
                    uint kd = RandomEncounterGenerator.allAvaibleMonsters.First(stat => stat.Name == item.Value.Name).Killed;
                    if (kd != 0)
                    {
                        uint newkey = item.Key + kd;
                        var value = item.Value;

                        requiredMonsters.Remove(item.Key);
                        requiredMonsters.Add(newkey, value);
                    }
                }
            }

        }

        public void GiveQuestItem(ref Dictionary<string,Item> items)
        {

            if (questStatus == QuestStatus.Started)
            {

                foreach (var item in items)
                {
                    if (requiredItems.ContainsKey(item.Key) && item.Value.Purpose == Item.ItemPurpose.Quest)
                    {
                        if (requiredItems[item.Key].Quantity > item.Value.Quantity)
                        {
                            //We dont have enough items, give all we can
                            requiredItems[item.Key].Quantity -= item.Value.Quantity;
                            items.Remove(item.Key);
                        }
                        else
                        {
                            //We have enough items, give all needed
                            uint temp;
                            temp = item.Value.Quantity - requiredItems[item.Key].Quantity;
                            requiredItems.Remove(item.Key);
                            item.Value.Quantity -= temp;

                            //Adding additional random reward for completing optional challenge
                            if (questTasks[item.Key].Optional)
                            {
                                //Move inv from MainPage to somewhere else :P
                                KeyValuePair<string,Item> t = MainPage.inv.AllItems.ElementAt(MainPage.rand.Next(MainPage.inv.AllItems.Count));

                                while (itemRewards.ContainsKey(t.Key))
                                {
                                    t = MainPage.inv.AllItems.ElementAt(MainPage.rand.Next(MainPage.inv.AllItems.Count));
                                }
                                
                                this.itemRewards.Add(t.Key,t.Value);
                                itemRewards[t.Key].Quantity = (uint)MainPage.rand.Next(1, 10);
                            }

                            CheckQuest();
                        }
                    }
                }

            }
        }

        public void GiveQuestItem(ref Dictionary<string,Item> items, Item item)
        {
            if (questStatus == QuestStatus.Started)
            {
                if (requiredItems.ContainsValue(item) && item.Purpose == Item.ItemPurpose.Quest)
                {
                    if (items[item.Name].Quantity > item.Quantity)
                    {
                        items[item.Name].Quantity -= item.Quantity;
                        requiredItems.Remove(item.Name);
                    }
                    else
                    {
                        //Do not have enough items, cant give yet :p
                    }
                }
            }
        }

        private void CompleteQuest()
        {
            if (this.questStatus == QuestStatus.Ready)
            {
                questStatus = QuestStatus.Completed;
                //Add some console text or smth here :p
            }
        }

        public void CheckKill(Monster enemy)
        {
            if (enemy.BaseStats.Name == requiredMonsters[enemy.BaseStats.Killed].Name)
            {
                requiredMonsters.Remove(enemy.BaseStats.Killed);
                CheckQuest();
            }
        }
        
        public void CheckQuest()
        {
            if ((requiredMonsters.Count == 0 || requiredMonsters == null) && (requiredItems.Count == 0 || requiredItems == null))
            {
                questStatus = QuestStatus.Ready;
            }
        }
    }
}

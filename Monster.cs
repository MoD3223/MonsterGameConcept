using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterGameConcept
{
    public class Monster
    {
        public string? UniqueName { get; private set; }
        public uint Level { get; private set; }
        public uint Experience { get; private set; }
        public uint MaxExperience { get; private set; }
        public int Health { get; private set; }
        public uint MaxHealth { get; private set; }
        public Rarity Rareness { get; private set; }
        public List<Move> Moves { get; private set; }
        public StatusCondition Status { get; set; }
        public Statistic BaseStats { get; private set; }
        public uint Attack { get; private set; }
        public uint Defense { get; private set; }
        public uint Speed { get; private set; }
        public uint SpecialAttack { get; private set; }
        public uint SpecialDefense { get; private set; }
        public uint? EvolutionLevel { get; private set; }

        Random r = new Random();
        

        public Monster(string? name, uint level,Rarity rareness, List<Move> moves, Statistic baseStats, uint? evolutionLevel)
        {
            UniqueName = name;
            Level = level;
            Experience = 0;
            Rareness = rareness;
            Moves = moves;
            Status = StatusCondition.Normal;
            BaseStats = baseStats;
            EvolutionLevel = evolutionLevel;
            CalculateStats();
            Health = (int)MaxHealth;
        }
        //TODO: put this in some player class, idk
        public static List<Monster?> monsterParty = new List<Monster?>(4); 
        public static List<Monster> monsterContainer = new List<Monster>();


        //TODO: Make it so rarity affects the monster in more ways



        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
            {
                Health = 0;
                if (r.Next(10) == 1)
                {
                    Status = StatusCondition.Dead;
                    return;
                }
                Status = StatusCondition.Unconscious;
            }
        }


        public void Heal(int amount)
        {
            if (Status != StatusCondition.Unconscious)
            {
                Health += amount;
                if (Health > MaxHealth)
                    Health = (int)MaxHealth;
            }
        }

        public void GainExperience(uint experiencePoints)
        {
            Experience += experiencePoints;
            if (Experience >= MaxExperience)
            {
                Experience -= MaxExperience;
                LevelUp();
                //Allows gaining more than 1 level at a time
                GainExperience(0);
            }
        }

        private void LevelUp()
        {
            Level++;
            LearnMovesByLevel();
            MaxExperience = CalculateNextLevelExperience();
            AddStats();
            Health = (int)MaxHealth;
            if (this.BaseStats.EvolutionLevel <= this.Level)
            {
                Evolve(this);
            }
        }

        private uint CalculateNextLevelExperience()
        {
            return Level * 100;
        }

        public void LearnMovesByLevel()
        {
            if (Level % 10 == 0)
            {
                int rand = r.Next(0, Moves.Count);
                LearnMove(Moves[rand]);
            }
        }

        public void LearnMove(Move move)
        {
            if (Moves.Exists(m => m.Name == move.Name))
            {
                return; // Move already known, cannot learn it again
            }

            if (move.RequiredLevel <= Level)
            {
                Moves.Add(move);
            }
            else
            {
                //We dont have required level!
            }
        }

        public void UseMove(int moveIndex, Monster target)
        {
            //Maybe require move instead of moveIndex?
            Move move = this.Moves[moveIndex];
            int damage = CalculateDamage(move, target);
            if (move.Category == MoveCategory.Support)
            {
                ApplyMoveEffects(move);
                this.Heal(move.Power * 10);
            }
            else
            {
                target.ApplyMoveEffects(move);
                target.TakeDamage(damage);
            }
            move.PP--;
        }

        private int CalculateDamage(Move move, Monster target)
        {
            uint level = Level;
            uint attackerStat = GetAttackStat(move.Category);
            uint defenderStat = target.GetDefenseStat(move.Category);
            double modifier = CalculateModifier(move,target);

            int damage = (int)(((2 * level) * (double)(move.Power * attackerStat / (double)defenderStat)) * modifier);
            if (move.MoveType == target.BaseStats.MonsterType)
            {
                damage *= 2;
            }
            return damage;
        }

        private double CalculateModifier(Move move, Monster target)
        {
            if (move.MoveType == Type.Fire && target.BaseStats.MonsterType == Type.Grass)
            {
                return 2.0;
            }
            else if (move.MoveType == Type.Water && target.BaseStats.MonsterType == Type.Fire)
            {
                return 2.0;
            }
            else if (move.MoveType == Type.Grass && target.BaseStats.MonsterType == Type.Water)
            {
                return 2.0;
            }
            else if ((move.MoveType == Type.Air && target.BaseStats.MonsterType == Type.Grass) || (move.MoveType == Type.Air && target.BaseStats.MonsterType == Type.Water))
            {
                return 1.5;
            }
            else if ((move.MoveType == Type.Power && target.BaseStats.MonsterType == Type.Normal) || (move.MoveType == Type.Power && target.BaseStats.MonsterType == Type.Grass))
            {
                return 1.5;
            }
            else if (move.MoveType == Type.Special && target.BaseStats.MonsterType == Type.Power)
            {
                return 2.0;
            }
            else if (move.MoveType == Type.Dark && target.BaseStats.MonsterType == Type.Special)
            {
                return 2.0;
            }
            else if (move.MoveType == Type.Rock && target.BaseStats.MonsterType == Type.Fairy)
            {
                return 2.0;
            }
            else if (move.MoveType == Type.Cosmic && target.BaseStats.MonsterType == Type.Dark)
            {
                return 2.0;
            }
            else
            {
                return 1.0; // Default effectiveness multiplier
            }
        }


        private void ApplyMoveEffects(Move move)
        {
            if (move.MoveStatus == StatusCondition.Empty)
            {
                return;
            }
            this.Status = move.MoveStatus;
        }

        public void CalculateStats()
        {
            MaxHealth = (uint)Math.Max(BaseStats.HP + r.Next(-10, 10), 0);
            Attack = (uint)Math.Max(BaseStats.Attack + r.Next(-10, 10), 0);
            Defense = (uint)Math.Max(BaseStats.Defense + r.Next(-10, 10), 0);
            Speed = (uint)Math.Max(BaseStats.Speed+ r.Next(-10, 10), 0);
            SpecialAttack = (uint)Math.Max(BaseStats.SpecialAttack + r.Next(-10, 10), 0);
            SpecialDefense = (uint)Math.Max(BaseStats.SpecialDefense + r.Next(-10, 10),0);

            for (int i = 0; i < this.Level; i++)
            {
                AddStats();
            }

        }

        private void AddStats()
        {
            MaxHealth += (uint)(r.Next(5) * (int)this.Rareness);
            Attack += (uint)(r.Next(5) * (int)this.Rareness);
            Defense += (uint)(r.Next(5) * (int)this.Rareness);
            Speed += (uint)(r.Next(5) * (int)this.Rareness);
            SpecialAttack += (uint)(r.Next(5) * (int)this.Rareness);
            SpecialDefense += (uint)(r.Next(5) * (int)this.Rareness);
        }

        private uint GetAttackStat(MoveCategory category)
        {
            if (category == MoveCategory.Special)
            {
                return SpecialAttack;
            }
            else
            {
                return Attack;
            }
        }

        private uint GetDefenseStat(MoveCategory category)
        {
            if (category == MoveCategory.Special)
            {
                return SpecialDefense;
            }
            else
            {
                return Defense;
            }
        }
        public static Monster? GetFirstNotNull(List<Monster> items)
        {
            foreach (var item in items)
            {
                if (item != null)
                {
                    return item;
                }
            }

            return null;
        }


        public void SwapList(List<Monster?> list, int index1, int index2)
        {
            Monster? temp = list[index1];
            list[index1] = list[index2];
            list[index2] = temp;
            PushNullsToBack(list);
        }

        public void PushNullsToBack(List<Monster?> list)
        {
            int nullCount = list.RemoveAll(item => item == null);
            for (int i = 0; i < nullCount; i++)
            {
                list.Add(null);
            }
        }

        public void Evolve(Monster mon)
        {
            uint MaxHealth = mon.MaxHealth - mon.BaseStats.HP;
            uint Attack = mon.Attack - mon.BaseStats.Attack;
            uint Defense = mon.Defense - mon.BaseStats.Defense;
            uint Speed = mon.Speed - mon.BaseStats.Speed;
            uint SpecialAttack = mon.SpecialAttack - mon.BaseStats.SpecialAttack;
            uint SpecialDefense = mon.SpecialDefense - mon.BaseStats.SpecialDefense;

            int index = monsterParty.IndexOf(mon);
            //TODO: Add calculating evolution level and fix this
            monsterParty[index] = new Monster(mon.UniqueName, mon.Level, (Rarity)((int)mon.Rareness + 1), mon.Moves,mon.BaseStats.Evolution,mon.BaseStats.Evolution.EvolutionLevel);
            monsterParty[index].MaxHealth += MaxHealth;
            monsterParty[index].Attack += Attack;
            monsterParty[index].Defense += Defense;
            monsterParty[index].Speed += Speed;
            monsterParty[index].SpecialAttack += SpecialAttack;
            monsterParty[index].SpecialDefense += SpecialDefense;
        }

        public void ChangeName(Monster mon, string newName)
        {
            mon.UniqueName = newName;   
        }
        //Ref?
        public uint CatchChance(ref Monster mon, ref Item item)
        {
            uint ret = 0;
            if (mon.Status != StatusCondition.Normal)
            {
                ret += 10;
            }

            ret += (uint)(50 - ((mon.Health / mon.MaxHealth) * 100) + item.CatchChance);
            //inv.removeitem??
            return ret;
        }

    }
    //Koniec klasy

    public enum Type
    {
        Normal,
        Fire,
        Water,
        Grass,
        Air,
        Power,
        Special,
        Dark,
        Rock,
        Fairy,
        Cosmic
        // Additional types...
    }

    public enum MoveCategory
    {
        Physical,
        Special,
        Status,
        Support
        //Additional categories...
    }

    public enum StatusCondition
    {
        Unconscious,
        Normal,
        Poison,
        Paralyze,
        Sleep,
        Binding,
        Dead,
        Empty //Used only to not override status if new move doesn't have any
        // Additional status conditions...
    }

    public enum Rarity
    {
        None, //Just for syntax purposes so actual rarities start from 1 :p
        Normal,
        Rare,
        Very_rare,
        Ultra_Rare,
        Special,
        Legendary
    }


    //Basic stats
    public class Statistic
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Type MonsterType { get; private set; }
        public uint HP { get; private set; }
        public uint Attack { get; private set; }
        public uint Defense { get; private set; }
        public uint Speed { get; private set; }
        public uint SpecialAttack { get; private set; }
        public uint SpecialDefense { get; private set; }
        public Statistic? Evolution { get; private set; }
        public uint? EvolutionLevel { get; private set; }
        public uint Killed { get; set; }

        public Statistic(string name,string description,Type type,uint hp, uint attack, uint defense, uint speed, uint specialAttack, uint specialDefense,Statistic? evolution,uint? evolutionLevel, uint killed = 0)
        {
            Name = name;
            Description = description;
            MonsterType = type;
            HP = hp;
            Attack = attack;
            Defense = defense;
            Speed = speed;
            SpecialAttack = specialAttack;
            SpecialDefense = specialDefense;
            Evolution = evolution;
            EvolutionLevel = evolutionLevel;
            Killed = killed;
        }
    }



}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterGameConcept
{
    public class Monster
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public uint Level { get; private set; }
        public uint Experience { get; private set; }
        public uint MaxExperience { get; private set; }
        public int Health { get; private set; }
        public uint MaxHealth { get; private set; }
        public Type MonsterType { get; private set; }
        public Rarity Rareness { get; private set; }
        public List<Move> Moves { get; private set; }
        public StatusCondition Status { get; set; }
        public Statistic BaseStats { get; private set; }
        public uint Attack { get; private set; }
        public uint Defense { get; private set; }
        public uint Speed { get; private set; }
        public uint SpecialAttack { get; private set; }
        public uint SpecialDefense { get; private set; }

        Random r = new Random();
        

        public Monster(string name, uint level, uint maxHealth, uint maxExperience, Type type,Rarity rareness, List<Move> moves, Statistic baseStats)
        {
            Name = name;
            Level = level;
            MaxHealth = maxHealth;
            Health = (int)MaxHealth;
            MaxExperience = maxExperience;
            Experience = 0;
            MonsterType = type;
            Rareness = rareness;
            Moves = moves;
            Status = StatusCondition.Normal;
            BaseStats = baseStats;
            CalculateStats();
        }

        //List<Monster?> monsterParty = new List<Monster?>(4); 
        //List<Monster?> monsterContainer = new List<Monster?>(100);


        //TODO: Make it so rarity affects the monster



        public void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health < 0)
            {
                Health = 0;
                if (r.Next(100) == 1)
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
        }

        private uint CalculateNextLevelExperience()
        {
            return Level * 100;
        }

        private void LearnMovesByLevel()
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

            if (move.RequiredMonsterType == MonsterType && move.RequiredLevel <= Level)
            {
                //TODO: Make it so only 4 moves are avaible, if all slots are taken ask for replacement
                Moves.Add(move);
            }
            else if (move.RequiredMonsterType == MonsterType)
            {
                //We have required type but not level
            }
            else
            {
                //We dont have required type!
            }
        }

        public void UseMove(int moveIndex, Monster target)
        {
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
            return damage;
        }

        private double CalculateModifier(Move move, Monster target)
        {
            if (move.MoveType == Type.Fire && target.MonsterType == Type.Grass)
    {
        return 2.0;
    }
    else if (move.MoveType == Type.Water && target.MonsterType == Type.Fire)
    {
        return 2.0;
    }
    else if (move.MoveType == Type.Grass && target.MonsterType == Type.Water)
    {
        return 2.0;
    }
    else if ((move.MoveType == Type.Air && target.MonsterType == Type.Grass) || (move.MoveType == Type.Air && target.MonsterType == Type.Water))
    {
        return 1.5;
    }
    else if ((move.MoveType == Type.Power && target.MonsterType == Type.Normal) || (move.MoveType == Type.Power && target.MonsterType == Type.Grass))
    {
        return 1.5;
    }
    else if (move.MoveType == Type.Special && target.MonsterType == Type.Power)
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

        private void CalculateStats()
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
            MaxHealth += (uint)r.Next(10);
            Attack += (uint)r.Next(5);
            Defense += (uint)r.Next(5);
            Speed += (uint)r.Next(5);
            SpecialAttack += (uint)r.Next(5);
            SpecialDefense += (uint)r.Next(5);
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
        Special
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
        public uint HP { get; private set; }
        public uint Attack { get; private set; }
        public uint Defense { get; private set; }
        public uint Speed { get; private set; }
        public uint SpecialAttack { get; private set; }
        public uint SpecialDefense { get; private set; }

        public Statistic(uint hp, uint attack, uint defense, uint speed, uint specialAttack, uint specialDefense)
        {
            HP = hp;
            Attack = attack;
            Defense = defense;
            Speed = speed;
            SpecialAttack = specialAttack;
            SpecialDefense = specialDefense;
        }
    }
    //Statistic bulba = new Statistic(1, 2, 3, 4, 5, 6);



}

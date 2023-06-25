using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterGameConcept
{
    public class Move
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Power { get; private set; }
        public MonsterType RequiredType { get; private set; }
        public MoveCategory Category { get; private set; }
        public MoveType Type { get; private set; }
        public int PP { get; set; }
        public int MaxPP { get; private set; }
        public StatusCondition MoveStatus { get; private set; }
        public int CritChanceProc { get; private set; }
        public int AccuracyProc { get; private set; }
        public int RequiredLevel { get; private set; }

        public Move(string name,string description, int power, MonsterType reqtype, MoveCategory category,MoveType type, int pp,StatusCondition movestatus,int crit, int acc, int reqlevel)
        {
            Name = name;
            Description = description;
            Power = power;
            RequiredType = reqtype;
            Category = category;
            Type = type;
            PP = pp;
            MaxPP = pp;
            MoveStatus = movestatus;
            CritChanceProc = crit;
            AccuracyProc = acc;
            RequiredLevel = reqlevel;
        }
    }
}

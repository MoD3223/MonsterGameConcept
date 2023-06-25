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
        public int Power { get; private set; }
        public MonsterType RequiredType { get; private set; }
        public MoveCategory Category { get; private set; }
        public int PP { get; set; }
        public int MaxPP { get; private set; }
        //Why???
        //public MoveEffect Effect { get; private set; }
        // Additional properties like accuracy, priority, and additional effects

        public int RequiredLevel { get; private set; }

        public Move(string name, int power, MonsterType type, MoveCategory category, int pp)
        {
            Name = name;
            Power = power;
            RequiredType = type;
            Category = category;
            PP = pp;
            MaxPP = pp;
        }
    }
}

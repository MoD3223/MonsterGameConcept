using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonsterGameConcept
{
    public class QuestTasks
    {
        public bool Optional;
        public string Name; //Name of Item or Monster to kill
        public string Description;

        public QuestTasks(string name, string description, bool optional = false)
        {
            Name = name;
            Description = description;
            Optional = optional;
        }



    }
}

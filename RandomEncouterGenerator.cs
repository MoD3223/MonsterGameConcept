using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MonsterGameConcept
{
    public class RandomEncounterGenerator
    {
        public static List<Statistic> allAvaibleMonsters;
        public static List<Move> allAvailableMoves;

        public RandomEncounterGenerator()
        {
            allAvaibleMonsters = LoadMonstersFromFile("monsterData.json"); //Just for Statistics class, not Monsters!!!
            allAvailableMoves = LoadMovesFromFile("movesData.json");
        }


        private List<Move> LoadMovesFromFile(string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            string jsonContent;

            using (StreamReader reader = new StreamReader(filePath))
            {
                jsonContent = reader.ReadToEnd();
            }

            List<Move> moves = new List<Move>();

            try
            {
                Move[] moveArray = JsonSerializer.Deserialize<Move[]>(jsonContent);

                if (moveArray != null)
                {
                    moves.AddRange(moveArray);
                }
            }
            catch (JsonException e)
            {
                Console.WriteLine("Error parsing JSON: " + e.Message);
            }

            return moves;
        }

        private List<Statistic> LoadMonstersFromFile(string fileName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            string jsonContent;

            using (StreamReader reader = new StreamReader(filePath))
            {
                jsonContent = reader.ReadToEnd();
            }

            List<Statistic> monsters = new List<Statistic>();

            try
            {
                Statistic[] monsterArray = JsonSerializer.Deserialize<Statistic[]>(jsonContent);

                if (monsterArray != null)
                {
                    monsters.AddRange(monsterArray);
                }
            }
            catch (JsonException e)
            {
                Console.WriteLine("Error parsing JSON: " + e.Message);
            }

            return monsters;
        }



        public Monster GenerateRandomEncounter()
        {
            //TODO: add random rarity
            Random r = new Random();
            Statistic based = allAvaibleMonsters[r.Next(allAvaibleMonsters.Count)];
            Monster randomMonster = new Monster(null, AvgPartyLevel(r.Next(-5,5)), (Rarity)1, PopulateMoves(5), based, based.EvolutionLevel);
            return randomMonster;
        }

        public Monster GenerateRandomEncounter(Type type)
        {
            //Can spawn monster with level higher than its evolution level. If that happens it will evolve at next levelup;
            Random r = new Random();
            Statistic based = ReturnSpecifiedMonsters(type)[r.Next(ReturnSpecifiedMonsters(type).Count)];
            Monster randomMonster = new Monster(null, AvgPartyLevel(r.Next(-5, 5)), (Rarity)1, PopulateMoves(5), based,based.EvolutionLevel);
            return randomMonster;
        }


        public List<Move> PopulateMoves(int number)
        {

            List<Move> selectedMoves = new List<Move>(number);
            Random r = new Random();

            for (int i = 0; i < number; i++)
            {
                int temp = r.Next(0, allAvailableMoves.Count);
                Move move = allAvailableMoves[temp];
                if (!selectedMoves.Contains(move))
                {
                    selectedMoves.Add(move);
                }
            }
            return selectedMoves;

        }

        public uint AvgPartyLevel(int a)
        {
            uint sum = 0;
            int notnulls = Monster.monsterParty.Count(xd => xd != null);
            for (int i = 0; i < 4; i++)
            {
                if (Monster.monsterParty[1].Level == null)
                {
                    //Do nothing
                }
                else
                {
                    sum += Monster.monsterParty[i].Level;
                }
            }
            return (uint)((sum/notnulls) + a);
        }

        public List<Statistic> ReturnSpecifiedMonsters(Type type)
        {
            return allAvaibleMonsters.Where(item => item.MonsterType == type).ToList();
        }




    }
}

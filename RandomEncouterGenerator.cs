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
        private List<Statistic> allAvaibleMonsters;
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

            Random r = new Random();
            Monster randomMonster = new Monster(null, AvgPartyLevel(r.Next(-5,5)), (Rarity)1, PopulateMoves(5), allAvaibleMonsters[r.Next(allAvaibleMonsters.Count)], null);
            return randomMonster;
        }

        public Monster GenerateRandomEncounter(Type type)
        {
            Random r = new Random();
            Monster randomMonster = new Monster(null, AvgPartyLevel(r.Next(-5, 5)), (Rarity)1, PopulateMoves(5), ReturnSpecifiedMonsters(type)[r.Next(ReturnSpecifiedMonsters(type).Count)]);
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
            for (int i = 0; i < 4; i++)
            {
                sum += Monster.monsterParty[i].Level;
            }
            return sum + (uint)a;
        }

        public List<Statistic> ReturnSpecifiedMonsters(Type type)
        {
            return allAvaibleMonsters.Where(item => item.MonsterType == type).ToList();
        }




    }
}

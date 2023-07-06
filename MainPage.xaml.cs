namespace MonsterGameConcept;

public partial class MainPage : ContentPage
{
	int count = 0;

    public static Inventory inv = new Inventory();
	public static Random rand = new Random();
	public static List<Quest> myQuests = new List<Quest>(); //Maybe also add list of completed quests?

    public MainPage()
	{
		InitializeComponent();
		
		inv.AddItem("Potion");
		//Item potion = new Item("Potion", default);
		//Quest quest = new Quest("test","desc test",new List<string> { "lmao", "xd" },new Dictionary<string, Item> { {"Potion",potion } ...)
		
    }

	private void OnCounterClicked(object sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}

	public static void StepOnGrass()
	{
        Random r = new Random();
        if (r.Next(10) == 1)
		{
            RandomEncounterGenerator ra = new RandomEncounterGenerator();
            Monster enemy = ra.GenerateRandomEncounter();
			StartFight(Monster.GetFirstNotNull(Monster.monsterParty), enemy);
        }
    }

	public static void StepOnGrass(Type t)
	{
        Random r = new Random();
        if (r.Next(10) == 1)
        {
            RandomEncounterGenerator ra = new RandomEncounterGenerator();
            Monster enemy = ra.GenerateRandomEncounter(t);
            StartFight(Monster.GetFirstNotNull(Monster.monsterParty), enemy);
        }
    }

	public static void StartFight(Monster player, Monster enemy)
	{
		//TODO: Make some actual alghoritm to choose what enemy does :p
		//shouldnt I use ref here so it's not local?
		//TODO: Fix that stuff, it needs heavy testing, maybe move to Monster and instead of player use this.?
        Random r = new Random();

		if (Choice() == 0)
		{
            if (player.Speed >= enemy.Speed)
            {
                player.UseMove(ChooseMove(), enemy);
                enemy.UseMove(r.Next(0, enemy.Moves.Count), player);
            }
            else
            {
                enemy.UseMove(r.Next(0, enemy.Moves.Count), player);
                player.UseMove(ChooseMove(), enemy);
            }
        }
		else if (Choice() == 1)
		{
			//Bring UI to choose item


		}
		else if (Choice() == 2)
		{

		}
		else if (Choice() == 3)
		{
			if (r.Next(10) == 1)
			{
                //Failed to run away
                enemy.UseMove(r.Next(0, enemy.Moves.Count), player);
            }
			else
			{
				return;
			}
		}

        if (player.Health == 0)
        {
            //Bring UI to choose another monster
            player = ChooseMonster();
			StartFight(player, enemy);
        }
        else if (enemy.Health == 0)
        {
            player.GainExperience(enemy.Level * 10);
			enemy.BaseStats.Killed++;

			foreach (Quest item in myQuests)
			{
				item.CheckKill(enemy);
			}

        }
        else
        {
            StartFight(player, enemy);
        }


    }


	public static void TryCatch(ref Monster target, Item item)
	{
		if (MainPage.rand.Next(100) < target.CatchChance(ref target,ref item))
		{
            target.LearnMovesByLevel();
            target.CalculateStats();
            if (Monster.GetFirstNotNull(Monster.monsterParty) == null)
			{
				//Monsters added to container
				target.LearnMovesByLevel();
				target.CalculateStats();
				Monster.monsterContainer.Add(target);
				
			}
			else
			{
				Monster temp = Monster.monsterParty.First(item => item == null);

                int i = Monster.monsterParty.IndexOf(temp);
                Monster.monsterParty[i] = target;
			}
		}
		//Message: Failed to catch monster
	}



	public static int ChooseMove()
	{
		//TODO: Pick from UI what move to pick, use list.IndexOf(move)?;
		
		return 1;
	}

	public static Monster ChooseMonster()
	{
		//Show some UI where you pick the monster
		return Monster.monsterParty[1];
	}

	public static int Choice()
	{
		//0 = use move, 1 = use item,2 = swap monster, 3 flee
		if (true)
		{
			return 0;
		}
		else if (true)
		{
			return 1;
		}
		else
		{

		}
	}
}


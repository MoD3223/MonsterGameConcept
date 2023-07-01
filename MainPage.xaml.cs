namespace MonsterGameConcept;

public partial class MainPage : ContentPage
{
	int count = 0;
    

    public MainPage()
	{
		InitializeComponent();
		Inventory inv = new Inventory();
		inv.AddItem("Potion");

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

		}
		else
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
        }
        else if (enemy.Health == 0)
        {
            player.GainExperience(enemy.Level * 10);
        }
        else
        {
            StartFight(player, enemy);
        }


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
		//0 = use move, 1 = use item, 2 flee
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


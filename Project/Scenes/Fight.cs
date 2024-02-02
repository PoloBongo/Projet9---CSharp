public class Fight
{
    private bool tourAlier = false;
    Random random = new Random();

    public void startCombat(EntityContainer entities)
    {
        EntityAbstract allie = entities.AlliesList[0];
        EntityAbstract enemie = entities.EnemiesList[0];

        AfficherEtatDesCombattants(allie, enemie);

        while (allie._health > 0 && enemie._health > 0)
        {
            DetermineTour(allie, enemie);

            if (tourAlier)
            {
                HandleAllieTurn(entities, ref allie, enemie);
            }
            else
            {
                HandleEnemyTurn(allie, enemie);
            }

            AfficherEtatDesCombattants(allie, enemie);
        }
    }
    private void DetermineTour(EntityAbstract allie, EntityAbstract enemie)
    {
        tourAlier = allie._speed > enemie._speed || tourAlier;
    }

    private void HandleAllieTurn(EntityContainer entities, ref EntityAbstract allie, EntityAbstract enemie)
    {
        List<string> options = new List<string> { "Changer d'allié", "Attaquer" };
        int selectedIndex = RunOptions(options, allie, enemie);

        switch (selectedIndex)
        {
            case 0:
                ChangeAllie(entities, ref allie, enemie);
                break;
            case 1:
                AttackEnemy(entities, allie, enemie);
                break;
        }

        CheckHealth(enemie, allie);
        tourAlier = false;
    }

    private void ChangeAllie(EntityContainer entities, ref EntityAbstract allie, EntityAbstract enemie)
    {
        List<string> alliesNames = entities.AlliesList.Select(a => a._name).ToList();
        int selectedIndex = RunOptions(alliesNames, allie, enemie);

        allie = entities.AlliesList[selectedIndex];
        Console.WriteLine($"Vous avez choisi l'allié : {allie._name}");
    }


    private void AttackEnemy(EntityContainer entities, EntityAbstract allie, EntityAbstract enemie)
    {
        Console.WriteLine($"Choisis la capacité que tu veux utiliser pour attaquer :\n");
        List<string> displayedCapacities = new List<string>();

        for (int i = 0; i < allie._ListCapacities.Count; i++)
        {
            if (allie._level >= allie._ListCapacities[i]._level && !displayedCapacities.Contains(allie._ListCapacities[i]._name))
            {
                Console.WriteLine($"- {allie._ListCapacities[i]._name}");
                displayedCapacities.Add(allie._ListCapacities[i]._name);
            }
        }

        int selectedIndex = RunOptions(displayedCapacities, allie, enemie);
        enemie.TakeDamage(allie._ListCapacities[selectedIndex]._damage);

        AfficherEtatDesCombattants(allie, enemie);

        Console.WriteLine($"vie {allie._name} : {allie._health}");
        Console.WriteLine($"vie {enemie._name} : {enemie._health}");
    }


    private void HandleEnemyTurn(EntityAbstract allie, EntityAbstract enemie)
    {

        int nombreAleatoire = random.Next(1, 4);

        switch (nombreAleatoire)
        {
            case 1:
                int damage = 50;
                allie.TakeDamage(damage);
                Console.WriteLine($"{enemie._name} attaque {allie._name} et inflige {damage} dégâts!");
                break;
            case 2:
                int healAmount = 10;
                enemie.AddHealth(healAmount);
                Console.WriteLine($"{enemie._name} se soigne et regagne {healAmount} points de vie!");
                break;
            case 3:
                int staminaAmount = 10;
                enemie.AddStamina(staminaAmount);
                Console.WriteLine($"{enemie._name} regagne {staminaAmount} points d'endurance!");
                break;
        }

        CheckHealth(enemie, allie);
        tourAlier = true;
    }


    private void CheckHealth(EntityAbstract enemie, EntityAbstract allie)
    {
        if (enemie._health <= 0)
        {
            Console.WriteLine(@"Tu as gagné");
            int nombreAleatoire = random.Next(2, 10 * enemie._level);
            allie.AddExperience(nombreAleatoire);

        }
        else if (allie._health <= 0)
        {
            Console.WriteLine("Tu as perdu");

        }
    }

    private int RunOptions(List<string> options, EntityAbstract allie, EntityAbstract enemie)
    {
        ConsoleKey keyPressed;
        int selectedIndex = 0;
        do
        {

            Console.Clear();
            DisplayOptions(options, selectedIndex, allie, enemie);

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            keyPressed = keyInfo.Key;

            if (keyPressed == ConsoleKey.UpArrow)
            {
                selectedIndex = (selectedIndex > 0) ? selectedIndex - 1 : options.Count - 1;
            }
            else if (keyPressed == ConsoleKey.DownArrow)
            {
                selectedIndex = (selectedIndex < options.Count - 1) ? selectedIndex + 1 : 0;
            }
        } while (keyPressed != ConsoleKey.Enter);

        return selectedIndex;
    }


    private void AfficherEtatDesCombattants(EntityAbstract allie, EntityAbstract enemie)
    {
        Console.WriteLine(@"
 ██████╗ ██████╗ ███╗   ███╗██████╗  █████╗ ████████╗
██╔════╝██╔═══██╗████╗ ████║██╔══██╗██╔══██╗╚══██╔══╝
██║     ██║   ██║██╔████╔██║██████╔╝███████║   ██║   
██║     ██║   ██║██║╚██╔╝██║██╔══██╗██╔══██║   ██║   
╚██████╗╚██████╔╝██║ ╚═╝ ██║██████╔╝██║  ██║   ██║   
 ╚═════╝ ╚═════╝ ╚═╝     ╚═╝╚═════╝ ╚═╝  ╚═╝   ╚═╝   
    

");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write($"vie {allie._name} : {allie._health} ");

        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"vie {enemie._name} : {enemie._health} ");

        Console.ResetColor();
    }

    private void DisplayOptions(List<string> options, int selectedIndex, EntityAbstract allie, EntityAbstract enemie)
    {
        AfficherEtatDesCombattants(allie, enemie);

        Console.WriteLine("Veuillez choisir une option :");
        for (int i = 0; i < options.Count; i++)
        {
            if (i == selectedIndex)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.White;
                Console.WriteLine($"* {options[i]}");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.WriteLine($"  {options[i]}");
            }
        }
        Console.ResetColor();
    }
}

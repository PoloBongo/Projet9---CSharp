using MapEntities;
using System.IO;

public class Fight
{
    private bool tourAlier = false;
    Random random = new Random();
    List<string> alliesNames;

    public void startCombat(EntityContainer entities1, EntityContainer entities2, bool sanglier, Player player)
    {
        EntityAbstract allie = entities1.AlliesList[0];
        EntityAbstract enemie = entities2.EnemiesList[0];
        if (sanglier)
        {
            for (int i = 0; i < entities2.EnemiesList.Count(); i++)
            {
                if (entities2.EnemiesList[i]._name == "Sanglier")
                {
                    enemie = entities2.EnemiesList[i];
                }
            }
        }

        AfficherEtatDesCombattants(allie, enemie);

        while (allie._health > 0 && enemie._health > 0)
        {
            DetermineTour(allie, enemie);

            if (tourAlier)
            {
                HandleAllieTurn(entities1, ref allie, enemie, player);
            }
            else
            {
                if (entities2.EnemiesList[0]._difficultyIA == "Normal")
                {
                    HandleEnemyTurnIARandom(allie, enemie, player);
                }
                else if (entities2.EnemiesList[0]._difficultyIA == "Dificil")
                {
                    HandleEnemyTurnIADificil(allie, enemie, player);
                    CheckHealth(enemie, allie, player);
                    tourAlier = true;
                }
                else if (entities2.EnemiesList[0]._difficultyIA == "Hard")
                {
                    HandleEnemyTurnIAHard(allie, enemie, player);
                    CheckHealth(enemie, allie, player);
                    tourAlier = true;
                }

            }

            AfficherEtatDesCombattants(allie, enemie);
        }
    }
    private void DetermineTour(EntityAbstract allie, EntityAbstract enemie)
    {
        tourAlier = allie._speed > enemie._speed || tourAlier;
    }

    private void HandleAllieTurn(EntityContainer entities, ref EntityAbstract allie, EntityAbstract enemie, Player p)
    {
        List<string> options;
        int selectedIndex;

        if (CheckStaminaAllie(allie, entities))
        {
            options = new List<string> { "Changer d'allié", "Attaquer", "Inventory" };
            selectedIndex = RunOptions(options, allie, enemie);
            switch (selectedIndex)
            {
                case 0:
                    ChangeAllie(entities, ref allie, enemie);
                    break;
                case 1:
                    AttackEnemy(allie, enemie, entities);
                    break;
                case 2:
                    playerInventory(p, allie, enemie);
                    break;
            }
        }
        else
        {
            options = new List<string> { "Changer d'allié", "Utiliser potion de stamina" };
            selectedIndex = RunOptions(options, allie, enemie);
            switch (selectedIndex)
            {
                case 0:
                    ChangeAllie(entities, ref allie, enemie);
                    break;
                case 1:
                    p.RemoveAlcool(1);
                    allie.AddStamina(20);
                    break;
            }
        }

        CheckHealth(enemie, allie, p);
        tourAlier = false;
    }

    public void playerInventory(Player player, EntityAbstract allie, EntityAbstract enemie)
    {
        List<string> options;
        int selectedIndex;
        options = new List<string> { $"Soin {player.NBAlcool}", "Stamina"};
        selectedIndex = RunOptions(options, allie, enemie);
        switch (selectedIndex)
        {
            case 0:
                Soin(player, allie);
                break;
            case 1:
                Stamina(player, allie);
                break;
        }
    }

    public void Soin(Player player, EntityAbstract allie) 
    {
        player.RemoveViande(1);
        allie.AddHealth(20);
    }

    public void Stamina(Player player, EntityAbstract allie)
    {
        player.RemoveAlcool(1);
        allie.AddStamina(20);
    }

    private void ChangeAllie(EntityContainer entities, ref EntityAbstract allie, EntityAbstract enemie)
    {
        alliesNames = entities.AlliesList
            .Where(a => a._currentBlocked != 1)
            .Select(a => a._name)
            .ToList();

        EntityAbstract newAllie = null;
        do
        {
            int selectedIndex = RunOptions(alliesNames, allie, enemie);
            string selectedName = alliesNames[selectedIndex];
            newAllie = entities.AlliesList.FirstOrDefault(a => a._name == selectedName);
        } while (newAllie == null);

        allie = newAllie;
        Console.WriteLine($"Vous avez choisi l'allié : {allie._name}");
        AfficherEtatDesCombattants(allie, enemie);
    }


    private void AttackEnemy(EntityAbstract allie, EntityAbstract enemie, EntityContainer entities)
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

        if (allie._stamina >= allie._ListCapacities[selectedIndex]._stamina)
        {
            ManageDamageByType(selectedIndex, allie, enemie);
        }
        
        AfficherEtatDesCombattants(allie, enemie);
    }

    private void ManageDamageByType(int selectedIndex, EntityAbstract allie, EntityAbstract enemie)
    {
        /* Si l'enemy est de type Humain alors je lui fait 50% de dégâts en plus*/
        if (allie._type == "Logia" && enemie._type == "Humain")
        {
            enemie.TakeDamage(allie._ListCapacities[selectedIndex]._damage * 1.50f);
            allie.LessStamina(allie._ListCapacities[selectedIndex]._stamina);
        }
        /* Si l'enemy est de type Paramecia alors je lui fait 25% de dégâts en plus*/
        else if (allie._type == "Logia" && enemie._type == "Paramecia")
        {
            enemie.TakeDamage(allie._ListCapacities[selectedIndex]._damage * 1.25f);
            allie.LessStamina(allie._ListCapacities[selectedIndex]._stamina);
        }
        /* Si l'enemy est de type Zoan alors je lui fait 10% de dégâts en moins*/
        else if (allie._type == "Logia" && enemie._type == "Zoan")
        {
            enemie.TakeDamage(allie._ListCapacities[selectedIndex]._damage * 0.90f);
            allie.LessStamina(allie._ListCapacities[selectedIndex]._stamina);
        }
        /* Si l'enemy est de type Humain alors je lui fait 25% de dégâts en plus*/
        else if (allie._type == "Paramecia" && enemie._type == "Humain")
        {
            enemie.TakeDamage(allie._ListCapacities[selectedIndex]._damage * 1.25f);
            allie.LessStamina(allie._ListCapacities[selectedIndex]._stamina);
        }
        /* Si l'enemy est de type Logia alors je lui fait 60% de dégâts en moins*/
        else if (allie._type == "Paramecia" && enemie._type == "Logia")
        {
            enemie.TakeDamage(allie._ListCapacities[selectedIndex]._damage * 0.40f);
            allie.LessStamina(allie._ListCapacities[selectedIndex]._stamina);
        }
        /* Si l'enemy est de type Humain alors je lui fait 10% de dégâts en moins*/
        else if (allie._type == "Paramecia" && enemie._type == "Humain")
        {
            enemie.TakeDamage(allie._ListCapacities[selectedIndex]._damage * 0.90f);
            allie.LessStamina(allie._ListCapacities[selectedIndex]._stamina);
        }
    }

    private void HandleEnemyTurnIARandom(EntityAbstract allie, EntityAbstract enemie, Player p)
    {
        int nombreAleatoire = random.Next(1, 4);
        int randomAttackEnemy = random.Next(0, 2);

        switch (nombreAleatoire)
        {
            case 1:
                allie.TakeDamage(enemie._ListCapacities[randomAttackEnemy]._damage);
                Console.WriteLine($"{enemie._name} attaque {allie._name} et inflige {enemie._ListCapacities[0]._damage} dégâts!");
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

        CheckHealth(enemie, allie, p);
        tourAlier = true;
    }

    private void HandleEnemyTurnIADificil(EntityAbstract allie, EntityAbstract enemie, Player p)
    {
        int UpAttackIndex = -1;
        float damageUp = float.MinValue;
       

        for (int i = 0; i < enemie._ListCapacities.Count(); i++)
        {
            float damage = 0;
            switch (enemie._ListCapacities[i]._type)
            {
                case "Eau":
                    damage = enemie._ListCapacities[i]._damage / allie._resistanceEau;
                    break;
                case "Feu":
                    damage = enemie._ListCapacities[i]._damage / allie._resistanceFeu;
                    break;
                case "Vent":
                    damage = enemie._ListCapacities[i]._damage / allie._resistanceVent;
                    break;
                case "Physique":
                    damage = enemie._ListCapacities[i]._damage / allie._resistancePhysique;
                    break;
            }

            if (damage > damageUp)
            {
                damageUp = damage;
                UpAttackIndex = i;
            }
        }

        if (UpAttackIndex != -1)
        {
            allie.TakeDamage(damageUp);
        }
    }

    private void HandleEnemyTurnIAHard(EntityAbstract allie, EntityAbstract enemie, Player p)
    {
        HandleEnemyTurnIADificil(allie, enemie, p);



    }

    private bool CheckStaminaAllie(EntityAbstract allie, EntityContainer entities)
    {
        for (int i = 0; i < allie._ListCapacities.Count(); i++)
        {
            if (allie._stamina >= allie._ListCapacities[i]._stamina)
            {
                return true;
            }
        }
        /* A LAISSER SI VOUS VOULEZ QUE QUAND IL L ALLIE N'A PLUS DE STAMINA QU'IL SOIT REMVOE DE LA LISTE DE SELECTION DES ALLIES*/
/*        var entitie = allie.GetInfoEntityUpdateBlocked("../../../Entities/entity.json");
        var targetAlliesUpdate = entities.AlliesList.FirstOrDefault(a => a._name.Equals(allie._name, StringComparison.OrdinalIgnoreCase));

        if (targetAlliesUpdate != null)
        {
            targetAlliesUpdate._blocked = allie._blocked;
            allie.UpdateJsonBlocked(entities, "../../../Entities/entity.json", 1);
        }*/
        /* FIN */

        return false;
    }

    private void CheckHealth(EntityAbstract enemie, EntityAbstract allie, Player p)
    {
        if (enemie._health <= 0)
        {
            Console.WriteLine(@"Tu as gagné");
            int nombreAleatoire = random.Next(2, 10 * enemie._level);
            allie.AddExperience(nombreAleatoire);
            enemie.Loot(p);

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
        Console.Clear();
        Console.WriteLine(@"
 ██████╗ ██████╗ ███╗   ███╗██████╗  █████╗ ████████╗
██╔════╝██╔═══██╗████╗ ████║██╔══██╗██╔══██╗╚══██╔══╝
██║     ██║   ██║██╔████╔██║██████╔╝███████║   ██║   
██║     ██║   ██║██║╚██╔╝██║██╔══██╗██╔══██║   ██║   
╚██████╗╚██████╔╝██║ ╚═╝ ██║██████╔╝██║  ██║   ██║   
 ╚═════╝ ╚═════╝ ╚═╝     ╚═╝╚═════╝ ╚═╝  ╚═╝   ╚═╝   
    

");


        Console.ResetColor();

        DisplayHealthBar(allie, enemie);

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

    private void DisplayHealthBar(EntityAbstract allie, EntityAbstract enemie)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Stamina de {allie._name}");
        DrawStaminaBar(allie._stamina, allie._maxStamina);
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(allie._name);
        DrawHealthBar(allie._health, allie._maxhealth);
        Console.WriteLine();

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(enemie._name);
        DrawHealthBar(enemie._health, enemie._maxhealth);
        Console.WriteLine();
    }

    static void DrawHealthBar(float currentHealth, int maxHealth)
    {
        int maxHealthImplicit = maxHealth ; 
        int barWidth = 40;

        currentHealth = Math.Max(currentHealth, 0);

        int currentWidth = (int)((double)currentHealth / maxHealthImplicit * barWidth);

        ConsoleColor color;
        double healthPercentage = (double)currentHealth / maxHealthImplicit;

        if (healthPercentage > 0.75)
            color = ConsoleColor.Green;
        else if (healthPercentage > 0.5)
            color = ConsoleColor.Yellow;
        else if (healthPercentage > 0.25)
            color = ConsoleColor.Red;
        else
            color = ConsoleColor.DarkRed;

        Console.ForegroundColor = color;
        Console.Write("[");
        for (int i = 0; i < barWidth; i++)
        {
            if (i < currentWidth)
                Console.Write("█");
            else
                Console.Write(" ");
        }
        Console.Write("]");
        Console.ResetColor();
        Console.WriteLine($" {currentHealth}/{maxHealthImplicit}");
    }

    static void DrawStaminaBar(float currentHealth, int maxHealth)
    {
        int maxHealthImplicit = maxHealth;
        int barWidth = 40;

        currentHealth = Math.Max(currentHealth, 0);

        int currentWidth = (int)((double)currentHealth / maxHealthImplicit * barWidth);

        ConsoleColor color = ConsoleColor.Magenta;

        Console.ForegroundColor = color;
        Console.Write("[");
        for (int i = 0; i < barWidth; i++)
        {
            if (i < currentWidth)
                Console.Write("█");
            else
                Console.Write(" ");
        }
        Console.Write("]");
        Console.ResetColor();
        Console.WriteLine($" {currentHealth}/{maxHealthImplicit}");
    }
}
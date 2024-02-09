
using MapEntities;

public class Enemy : EntityAbstract
{
    Random random = new Random();
    public string difficultyIA;
    public override void DisplayDetails()
    {
        Console.WriteLine($"Name : {name} Health: {health}, Stamina: {stamina}, Speed: {speed}, Level: {level}");
    }

    public override void AddHealth(int add)
    {
        health += add;
    }

    public override void TakeDamage(float damage)
    {
        health -= damage;
    }

    public override void AddStamina(int add)
    {
        stamina += add;
    }

    public override void LessStamina(float less)
    {
        stamina -= less;
    }
    public override void AddExperience(int add)
    {
        experience += add;
    }

    public override void AddLevel()
    {
        if (experience >= maxExerience)
        {
            int tmp = experience - maxExerience;
            experience = 0;
            experience = tmp;
            level++;
            maxExerience = 100 * level;
        }
    }

    public override void Loot(Player p)
    {
        int nbLoot = random.Next(1, 3);
        if (name == "Marine")
        {
            p.AddAlcool(nbLoot);
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"\tTu as recuperé {nbLoot} bouteille(s) d'alcool(s)");
            Console.ResetColor();
        }
        if (name == "Sanglier")
        {
            p.AddViande(nbLoot);
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"\tTu as recuperé {nbLoot} viande(s)");
            Console.ResetColor();
        }
    }
}

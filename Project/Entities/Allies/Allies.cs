
using MapEntities;
using Newtonsoft.Json;
using System;
using System.IO;

public class Allies : EntityAbstract
{
    public override void DisplayDetails()
    {
        Console.WriteLine($"Name : {name} Health: {health}, Stamina: {stamina}, Speed: {speed}, Level: {level} - {experience} / {maxExerience}");
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
        string path = "../../../Entities/entity.json";
        stamina -= less;
        var entities = GetInfoEntityUpdateLevel(path);
        var targetAlliesUpdate = entities.AlliesList.FirstOrDefault(a => a.name.Equals(this.name, StringComparison.OrdinalIgnoreCase));

        if (targetAlliesUpdate != null)
        {
            targetAlliesUpdate.stamina = stamina;
            UpdateJsonStamina(entities, path);
        }
    }
    public override void AddExperience(int add)
    {
        experience += add;
        AddLevel();
    }

    public override void AddLevel()
    {
        string path = "../../../Entities/entity.json";
        if (experience >= maxExerience)
        {
            int tmp = experience - maxExerience;
            experience = 0;
            experience = tmp;
            level++;
            maxExerience = 100 * level;

            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine($"LEVEL UP {level} : {experience}/{maxExerience} ");
            Console.ResetColor();

            var entities = GetInfoEntityUpdateLevel(path);
            var targetAlliesUpdate = entities.AlliesList.FirstOrDefault(a => a.name.Equals(this.name, StringComparison.OrdinalIgnoreCase));
                
            if (targetAlliesUpdate != null)
            {
                targetAlliesUpdate.level = level;
                UpdateJsonLevel(entities, path);
            }
        }
    }
    public override void Loot(Player p){}
}

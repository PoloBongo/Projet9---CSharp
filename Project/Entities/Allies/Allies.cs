
using MapEntities;
using Newtonsoft.Json;
using System;
using System.IO;

public class Allies : EntityAbstract
{
    public override void DisplayDetails()
    {
        Console.WriteLine($"Name : {_name} Health: {_health}, Stamina: {_stamina}, Speed: {_speed}, Level: {_level}");
    }

    public override void AddHealth(int add)
    {
        _health += add;
    }

    public override void TakeDamage(float damage)
    {
        _health -= damage;
    }

    public override void AddStamina(int add)
    {
        _stamina += add;
    }

    public override void LessStamina(float less)
    {
        _stamina -= less;
        var entities = GetInfoEntityUpdateLevel(path);
        var targetAlliesUpdate = entities.AlliesList.FirstOrDefault(a => a._name.Equals(this._name, StringComparison.OrdinalIgnoreCase));

        if (targetAlliesUpdate != null)
        {
            targetAlliesUpdate._stamina = _stamina;
            UpdateJsonStamina(entities, path);
        }
    }
    public override void AddExperience(int add)
    {
        _experience += add;
        AddLevel();
    }

    public override void AddLevel()
    {
        if (_experience >= _maxExerience)
        {
            int tmp = _experience - _maxExerience;
            _experience = 0;
            _experience = tmp;
            _level++;
            _maxExerience = 100 * _level;
            Console.WriteLine($"Tu as monter de nv {_level} : {_experience}/{_maxExerience} ");

            var entities = GetInfoEntityUpdateLevel(path);
            var targetAlliesUpdate = entities.AlliesList.FirstOrDefault(a => a._name.Equals(this._name, StringComparison.OrdinalIgnoreCase));
                
            if (targetAlliesUpdate != null)
            {
                targetAlliesUpdate._level = _level;
                UpdateJsonLevel(entities, path);
            }
        }
    }
    public override void Loot(Player p){}
}

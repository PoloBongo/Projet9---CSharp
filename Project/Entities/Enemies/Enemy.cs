
using Newtonsoft.Json;

public class Enemy : EntityAbstract
{
    public List<EntitiesCapacities> _ListCapacities { get; set; }
    public EntityContainer alliesContainer;
    public override void DisplayDetails()
    {
        Console.WriteLine($"Name : {_name} Health: {_health}, Stamina: {_stamina}, Speed: {_speed}, Level: {_level}");
    }

    public override void SetStatsEntity(EntityAbstract entity)
    {
        _name = entity._name;
        _health = entity._health;
        _stamina = entity._stamina;
        _speed = entity._speed;
        _level = entity._level;
    }

    public override void AddHealth(int add)
    {
        _health += add;
    }

    public override void TakeDamage(int damage)
    {
        _health -= damage;
    }

    public override void AddStamina(int add)
    {
        _stamina += add;
    }

    public override void LessStamina(int less)
    {
        _stamina -= less;
    }
    public override void AddExperience(int add)
    {
        _experience += add;
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
        }
    }
}

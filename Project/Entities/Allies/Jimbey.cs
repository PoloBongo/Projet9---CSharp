
public class Jimbey : EntityAbstract
{
    public override void DisplayDetails()
    {
        Console.WriteLine($"Name : {_name} Health: {_health}, Stamina: {_stamina}, Speed: {_speed}, Level: {_level}");
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
        AddLevel();
    }

    public override void AddLevel()
    {
        Console.WriteLine($"cc");
        if (_experience >= _maxExerience)
        {
            int tmp = _experience - _maxExerience;
            _experience = 0;
            _experience = tmp;
            _level++;
            _maxExerience = 100 * _level;
            Console.WriteLine($"Tu as monter de nv {_level} : {_experience}/{_maxExerience} ");
        }
    }
}

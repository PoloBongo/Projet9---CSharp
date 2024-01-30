
public abstract class EntityAbstract
{
    public string _name;
    public int _health;
    public int _stamina;
    public int _speed;
    public int _level;
    public int _experience;
    public int _maxExerience;

    public abstract void DisplayDetails();
    public abstract void AddHealth(int add);
    public abstract void AddStamina(int add);
    public abstract void LessStamina(int less);
    public abstract void TakeDamage(int less);
    public abstract void AddLevel();
    public abstract void AddExperience(int add);
}


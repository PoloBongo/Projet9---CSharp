
public abstract class EntityAbstract
{
    public string _name;
    public int _health;
    public int _stamina;
    public int _speed;
    public int _level;

    public abstract void DisplayDetails();
    public abstract void AddHealth(int add);
    public abstract void AddStamina(int add);
    public abstract void LessStamina(int less);
    public abstract void TakeDamage(int less);
}


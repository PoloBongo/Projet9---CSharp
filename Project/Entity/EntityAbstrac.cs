
public abstract class EntityAbstrac
{
    public int Heal;
    public int Attack;
    public int Aki;
    public int Precision;
    public int Speed;
    public int Level;

    public abstract void DisplayDetails();

    public abstract void AddHeal(int add);

    public abstract void AddStamina(int add);
    public abstract void TakeDamage(int add);
}


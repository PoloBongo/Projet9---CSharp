
public abstract class EntityAbstrac
{
    public int Heal { get; set; }
    public int Attack { get; set; }
    public int Aki { get; set; }
    public int Precision { get; set; }
    public int Speed { get; set; }
    public int Level { get; set; }

    public abstract void DisplayDetails();

    public abstract void AddHeal(int add);

    public abstract void AddStamina(int add);
}


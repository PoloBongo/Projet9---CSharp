
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

    public abstract void SetStatsMarine(EntityAbstract entity);

    public static EntityAbstract CreateEntity(string name)
    {
        /* Si l'entité existe dans le fichier alors il créer une instance de celle-ci sinon ça va pas */
        switch (name.ToLower())
        {
            case "marine":
                return new Marine();
            case "jimbey":
                return new Jimbey();
            default:
                throw new ArgumentException("l'entité n'existe pas : " + name);
        }
    }
}

class Marine : EntityAbstrac
{
    public override void DisplayDetails()
    {
        Console.WriteLine($"Heal: {Heal}, Attack: {Attack}, Stamina: {Stamina}, Precision: {Precision}, Speed: {Speed}, Level: {Level}");
    }

    public override void AddHeal(int add)
    {
        Heal += add;
    }

    public override void AddStamina(int add)
    {
        Stamina += add;
    }

    public override void lessStamina(int add)
    {
        Stamina -= add;
    }

    public override void TakeDamage(int damage)
    {
        Heal -= damage;
    }
}

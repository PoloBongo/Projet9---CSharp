
class Marine : EntityAbstrac
{
    public override void DisplayDetails()
    {
        Console.WriteLine($"Heal: {_heal}, Attack: {_attack}, Stamina: {_stamina}, Precision: {_precision}, Speed: {_speed}, Level: {_level}");
    }

    public override void AddHeal(int add)
    {
        _heal += add;
    }

    public override void TakeDamage(int damage)
    {
        _heal -= damage;
    }

    public override void AddStamina(int add)
    {
        _stamina += add;
    }

    public override void lessStamina(int less)
    {
        _stamina -= less;
    }
}

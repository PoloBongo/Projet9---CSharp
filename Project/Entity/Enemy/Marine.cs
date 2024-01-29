
class Marine : EntityAbstrac
{
    public override void DisplayDetails()
    {
        Console.WriteLine($"Heal: {Heal}, Attack: {Attack}, Aki: {Aki}, Precision: {Precision}, Speed: {Speed}, Level: {Level}");
    }

    public override void AddHeal(int add)
    {
        Heal += add;
    }

    public override void AddStamina(int add)
    {
    }
}

public class Initialisation
{
    public void creationEntity()
    {
        Marine marineUnit = new Marine();
        marineUnit.Heal = 100;
        marineUnit.Attack = 75;
        marineUnit.Aki = 50;
        marineUnit.Precision = 90;
        marineUnit.Speed = 60;
        marineUnit.Level = 5;
        marineUnit.DisplayDetails();
    }
}

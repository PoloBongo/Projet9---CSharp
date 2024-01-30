public class Initialization
{
    public void creationEntity(EntityAbstract marine, EntityAbstract jimbey)
    {
        marine._name = "marine";
        marine._health = 100;
        marine._stamina = 50;
        marine._speed = 60;
        marine._level = 5;

        jimbey._name = "jimbey";
        jimbey._health = 100;
        jimbey._stamina = 50;
        jimbey._speed = 70;
        jimbey._level = 1;
        jimbey._experience = 99;
        jimbey._maxExerience = 100;
    }
}

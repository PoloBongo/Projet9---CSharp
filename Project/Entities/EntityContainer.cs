public class EntityContainer
{
    public List<Allies> alliesList { get; set; } = new List<Allies>();
    public List<Enemy> enemiesList { get; set; }

    public List<Allies> GetAlliesAlive()
    {
        List<Allies> alliesAlive = alliesList.Where(a => a.health > 0).ToList();
        return alliesAlive;
    }

    public List<Enemy> GetAvailableEnemies()
    {
        List<Enemy> enemyAlive = enemiesList.Where(e => e.health > 0).ToList();
        return enemyAlive;
    }
}

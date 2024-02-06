public class EntityContainer
{
    public List<Allies> AlliesList { get; set; } = new List<Allies>();
    public List<Enemy> EnemiesList { get; set; }

    public List<Allies> GetAlliesAlive()
    {
        List<Allies> alliesAlive = AlliesList.Where(a => a._health > 0).ToList();
        return alliesAlive;
    }

    public List<Enemy> GetAvailableEnemies()
    {
        List<Enemy> enemyAlive = EnemiesList.Where(e => e._health > 0).ToList();
        return enemyAlive;
    }
}

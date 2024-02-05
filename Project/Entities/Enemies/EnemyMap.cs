
namespace MapEntities;

public class EnemyMap
{
    private int LocalX;
    private int LocalY;
    private int WorldX;
    private int WorldY;
    private bool CombatStart;

    public EnemyMap(int worldX, int worldY, int localX, int localY)
    {
        WorldX = worldX;
        WorldY = worldY;
        LocalX = localX;
        LocalY = localY;
    }

    public int WORLDX
    {

        get => WorldX;
        set => WorldX = value;
    }

    public int WORLDY
    {

        get => WorldY;
        set => WorldY = value;
    }

    public int LOCALX
    {

        get => LocalX;
        set => LocalX = value;
    }

    public int LOCALY
    {

        get => LocalY;
        set => LocalY = value;
    }

    public bool COMBATSTART
    {
        get => CombatStart;
        set => CombatStart = value;
    }
}
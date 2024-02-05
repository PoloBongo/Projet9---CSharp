
namespace MapEntities
{
    public class Player
    {
        public int LocalX { get; set; }
        public int LocalY { get; set; }
        public int WorldX { get; set; }
        public int WorldY { get; set; }

        private int nbViande = 4;
        private int nbAlcool = 4;

        public Player(int worldX, int worldY, int localX, int localY)
        {
            WorldX = worldX;
            WorldY = worldY;
            LocalX = localX;
            LocalY = localY;
        }

        public int NBViande
        {
            get => nbViande;
            set => nbViande = value;
        }

        public int NBAlcool
        {
            get => nbAlcool;
            set => nbAlcool = value;
        }

        public void AddViande(int _viande)
        {
            nbViande += _viande;
        }

        public void RemoveViande(int _viande)
        {
            nbViande -= _viande;
        }

        public void AddAlcool(int _alcool)
        {
            nbViande += _alcool;
        }

        public void RemoveAlcool(int _alcool)
        {
            nbViande -= _alcool;
        }
    }
}
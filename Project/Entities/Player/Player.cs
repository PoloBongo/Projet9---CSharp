
namespace MapEntities
{
    public class Player
    {
        private int LocalX;
        private int LocalY;
        private int WorldX;
        private int WorldY;

        private int nbViande = 4;

        private int nbAlcool = 4;
        private int nbGold = 30;

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

        public int NBGold
        {
            get => nbGold;
            set => nbGold = value;
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

        public void AddGoldl(int _gold)
        {
            nbViande += _gold;
        }

        public void RemoveGold(int _alcool)
        {
            nbViande -= _alcool;
        }
    }
}

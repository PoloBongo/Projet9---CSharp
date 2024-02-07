
namespace MapEntities
{
    public class Player
    {
        private int LocalX;
        private int LocalY;
        private int WorldX;
        private int WorldY;

        private int nbViande = 3;

        private int nbAlcool = 1;
        private int nbGold = 100;

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
            nbAlcool += _alcool;
        }

        public void RemoveAlcool(int _alcool)
        {
            nbAlcool -= _alcool;
        }

        public void AddGoldl(int _gold)
        {
            nbGold += _gold;
        }

        public void RemoveGold(int _alcool)
        {
            nbGold -= _alcool;
        }
    }
}
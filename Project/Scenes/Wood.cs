
using MapGame;

namespace Wood
{
    public class WoodPiece
    {
        public int PositionX { get; }
        public int PositionY { get; }

        public WoodPiece(int positionX, int positionY)
        {
            PositionX = positionX;
            PositionY = positionY;
        }
    }

    public class WoodCollector
    {
        private int woodCollected;
        private int worldX;
        private int worldY;
        private Map map;
        private List<WoodPiece> woodPieces;

        public WoodCollector(int worldX, int worldY, int localX, int localY, int initialWoodCollected, Map map, List<WoodPiece> woodPieces)
        {
            this.worldX = worldX;
            this.worldY = worldY;
            woodCollected = initialWoodCollected;
            this.map = map;
            this.woodPieces = woodPieces;
        }

        public int WoodCollected
        {
            get { return woodCollected; }
            set { woodCollected = value; }
        }

        public int WorldX
        {
            get { return worldX; }
            set { worldX = value; }
        }

        public int WorldY
        {
            get { return worldY; }
            set { worldY = value; }
        }

        public List<(int, int)> WoodPieces
        {
            get
            {
                List<(int, int)> woodPieceCoordinates = new List<(int, int)>();
                foreach (var woodPiece in woodPieces) // Utilisez la variable membre ici
                {
                    woodPieceCoordinates.Add((woodPiece.PositionX, woodPiece.PositionY));
                }
                return woodPieceCoordinates;
            }
        }






        public void CollectWood()
        {
            if (woodCollected < 5)
            {
                woodCollected++;
                Console.WriteLine($"Morceau de bois ramassé. Vous avez maintenant {woodCollected}/5.");

                // Récupérer les coordonnées du morceau de bois
                var woodPieceCoordinates = map.WoodPieces;
                foreach (var woodPiece in woodPieceCoordinates)
                {
                    int woodPieceX = woodPiece.PositionX;
                    int woodPieceY = woodPiece.PositionY;

                    if (woodPieceX == worldX && woodPieceY == worldY)
                    {
                        // Remplacer le morceau de bois par de l'herbe sur la carte
                        map.ClearWoodPiecePosition(woodPieceX, woodPieceY);
                        Console.WriteLine($"Suppression du morceau de bois de la carte : {woodPieceX}, {woodPieceY}");
                        break;
                    }
                }


            }
            else
            {
                Console.WriteLine("Vous avez déjà ramassé suffisamment de bois.");
            }
        }
    }
}

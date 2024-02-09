
using MapEntities;
using MapGame;
using Wood;

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
        /*private Map map;*/
        private List<WoodPiece> woodPieces;

        public WoodCollector(int worldX, int worldY, int localX, int localY, int initialWoodCollected, Map map, List<WoodPiece> woodPieces)
        {
            this.worldX = localX;
            this.worldY = localY;
            woodCollected = initialWoodCollected;
            /*this.map = map;*/
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

        public void CollectWood(Map map, Player player)
        {
            if (woodCollected < 5)
            {
                for (int i = 0; i < map.WoodPieces.Count; i++)
                {
                    var woodPiece = map.WoodPieces[i];
                    // Vérifier si le joueur est sur la même position que le morceau de bois
                    if (player.LOCALX == woodPiece.PositionX && player.LOCALY == woodPiece.PositionY)
                    {
                        woodCollected++;
                        Console.WriteLine($"Morceau de bois ramassé. Vous avez maintenant {woodCollected}/5.");

                        // Remplacer le morceau de bois par de l'herbe sur la carte
                        map.ClearWoodPiecePosition(woodPiece.PositionX, woodPiece.PositionY);
                        break; // Sortir de la boucle après avoir ramassé le morceau de bois
                    }
                }
            }
            else
            {
                Console.WriteLine("Vous avez déjà ramassé suffisamment de bois.");
            }
        }
        public void UpdateWoodPieces(List<WoodPiece> woodPieces)
        {
            this.woodPieces = woodPieces;
        }



    }
}


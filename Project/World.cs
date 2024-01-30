using MapGame;
using PlayerGame;

namespace MapGame
{
    public class World
    {
        private Map[,] worldMaps;
        private int worldSize = 3;

        public World()
        {
            worldMaps = new Map[worldSize, worldSize];
            InitializeWorld();
        }

        private void InitializeWorld()
        {
            for (int i = 0; i < worldSize; i++)
            {
                for (int j = 0; j < worldSize; j++)
                {
                    worldMaps[i, j] = new Map(20, 20);
                    bool isCenterMap = (i == 1 && j == 1); // Vérifier si c'est la carte centrale
                    char[,] selectedLayout = CreateRandomLayout(isCenterMap);
                    worldMaps[i, j].InitializeCustomMap(selectedLayout);
                }
            }
        }

        private char[,] CreateRandomLayout(bool isCenterMap)
        {
            char[,] layout = new char[20, 20];
            Random random = new Random();

            // Initialisation de la première ligne et colonne
            for (int i = 0; i < 20; i++)
            {
                layout[0, i] = random.Next(4) != 0 ? '.' : '~';
                layout[i, 0] = random.Next(4) != 0 ? '.' : '~';
            }

            // Remplissage du reste du layout
            for (int i = 1; i < 20; i++)
            {
                for (int j = 1; j < 20; j++)
                {
                    int grassCount = 0;
                    if (layout[i - 1, j] == '.') grassCount++;
                    if (layout[i, j - 1] == '.') grassCount++;
                    if (layout[i - 1, j - 1] == '.') grassCount++;

                    layout[i, j] = random.Next(3) < grassCount ? '.' : '~';
                }
            }

            if (isCenterMap)
            {
                int centerX = 10, centerY = 10;
                layout[centerX, centerY] = '.';
                layout[centerX - 1, centerY] = '.';
                layout[centerX + 1, centerY] = '.';
                layout[centerX, centerY - 1] = '.';
                layout[centerX, centerY + 1] = '.';
            }

            return layout;
        }

        public Map GetMapAt(int x, int y)
        {
            if (x >= 0 && x < worldSize && y >= 0 && y < worldSize)
            {
                return worldMaps[x, y];
            }
            else
            {
                return null;
            }
        }

        public void MovePlayerToNewMap(Player player)
        {
            int mapHeight = 20;
            int mapWidth = 20;

            // Vérifie si le joueur atteint le bord supérieur de la carte
            if (player.LocalX < 0)
            {
                if (player.WorldX > 0)
                {
                    player.WorldX--;
                    player.LocalX = mapHeight - 1;
                }
                else
                {
                    player.LocalX = 0; // Empêche le joueur de quitter le monde
                }
            }
            // Vérifie si le joueur atteint le bord inférieur de la carte
            else if (player.LocalX >= mapHeight)
            {
                if (player.WorldX < worldSize - 1)
                {
                    player.WorldX++;
                    player.LocalX = 0;
                }
                else
                {
                    player.LocalX = mapHeight - 1;
                }
            }
            // Vérifie si le joueur atteint le bord gauche de la carte
            else if (player.LocalY < 0)
            {
                if (player.WorldY > 0)
                {
                    player.WorldY--;
                    player.LocalY = mapWidth - 1; 
                }
                else
                {
                    player.LocalY = 0;
                }
            }
            // Vérifie si le joueur atteint le bord droit de la carte
            else if (player.LocalY >= mapWidth)
            {
                if (player.WorldY < worldSize - 1) 
                {
                    player.WorldY++; 
                    player.LocalY = 0; 
                }
                else
                {
                    player.LocalY = mapWidth - 1;
                }
            }

            // Mettre à jour la carte actuelle après le déplacement
            Map currentMap = GetMapAt(player.WorldX, player.WorldY);
            if (currentMap != null)
            {
                EnsurePlayerOnLand(currentMap, player);

                currentMap.PlacePlayer(player.LocalX, player.LocalY);
            }
        }

        private void EnsurePlayerOnLand(Map currentMap, Player player)
        {
            if (currentMap.IsWater(player.LocalX, player.LocalY))
            {
                // Trouver la case d'herbe la plus proche pour repositionner le joueur
                for (int offsetX = -1; offsetX <= 1; offsetX++)
                {
                    for (int offsetY = -1; offsetY <= 1; offsetY++)
                    {
                        int newX = player.LocalX + offsetX;
                        int newY = player.LocalY + offsetY;

                        if (newX >= 0 && newX < 20 && newY >= 0 && newY < 20)
                        {
                            if (currentMap.IsGrass(newX, newY))
                            {
                                // Place le joueur sur cette position d'herbe
                                currentMap.PlacePlayer(newX, newY);
                                player.LocalX = newX;
                                player.LocalY = newY;
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}

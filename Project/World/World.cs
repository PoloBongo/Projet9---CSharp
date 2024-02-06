using MapGame;
using MapEntities;
using System;
using System.Collections.Generic;

namespace MapGame
{
    public class World
    {
        private Map[,] worldMaps;
        private int worldSize = 3;
        private List<EnemyMap> enemyMaps = new List<EnemyMap>();
        private List<int> positionX = new List<int>();
        private bool CombatStart = false;
        Fight fight = new Fight();
        Random random = new Random();

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
                    bool isBorderMap = IsBorderMap(i, j);
                    bool isCenterMap = (i == 1 && j == 1);
                    bool isSpecialMap = (i == 0 && j == 2);
                    char[,] selectedLayout = CreateRandomLayout(isBorderMap, isCenterMap, isSpecialMap, i, j);
                    worldMaps[i, j] = new Map(20, 20);
                    worldMaps[i, j].InitializeCustomMap(selectedLayout);
                    if (!(i == 1 && j == 1) && !(i == 0 && j == 2))
                    {
                        PlaceEnemiesRandomly(worldMaps[i, j], i, j);
                    }
                }
            }
            InitializeEnemy();
        }

        private bool IsBorderMap(int x, int y)
        {
            // Une carte est sur le bord si elle est sur la première ou la dernière ligne/colonne
            return x == 0 || y == 0 || x == worldSize - 1 || y == worldSize - 1;
        }

        private char[,] CreateRandomLayout(bool isBorderMap, bool isCenterMap, bool isSpecialMap, int mapX, int mapY)
        {
            char[,] layout = new char[20, 20];
            Random random = new Random();

            // Initialiser tout le terrain avec de l'herbe
            for (int i = 0; i < 20; i++)
                for (int j = 0; j < 20; j++)
                    layout[i, j] = '.';

            // Générer des lacs aléatoirement pour les cartes non spéciales
            if (!isCenterMap && !isSpecialMap)
            {
                // Déterminer le nombre de lacs à créer
                int numberOfLakes = random.Next(3, 5); // Entre 3 et 5 lacs

                // Créer chaque lac
                for (int lake = 0; lake < numberOfLakes; lake++)
                {
                    // Choisir un point de départ pour le lac
                    int lakeStartX = random.Next(3, 17);
                    int lakeStartY = random.Next(3, 17);
                    // Déterminer la taille du lac
                    int lakeWidth = random.Next(5, 10);
                    int lakeHeight = random.Next(3, 9);

                    // Remplir la zone du lac avec de l'eau
                    for (int x = lakeStartX; x < lakeStartX + lakeWidth; x++)
                        for (int y = lakeStartY; y < lakeStartY + lakeHeight; y++)
                            if (x >= 0 && x < 20 && y >= 0 && y < 20)
                                layout[x, y] = '~';
                }
            }

            if (isCenterMap)
            {
                // Déterminer le nombre de maisons à créer
                int numberOfHouses = random.Next(4, 6);

                // Créer chaque maison
                for (int house = 0; house < numberOfHouses; house++)
                {
                    // Choisir une taille impaire aléatoire pour la maison
                    int houseWidth = random.Next(1, 3) * 2 + 1; // Donne un nombre impair entre 3 et 5
                    int houseHeight = random.Next(1, 3) * 2 + 1; // Donne un nombre impair entre 3 et 5

                    // Choisir un emplacement aléatoire pour la maison
                    int houseX, houseY;
                    bool spaceFound;
                    do
                    {
                        spaceFound = true;
                        houseX = random.Next(1, 20 - houseWidth);
                        houseY = random.Next(1, 20 - houseHeight);

                        // Vérifier si l'espace est libre et sans maisons adjacentes
                        for (int x = houseX - 1; x <= houseX + houseWidth && spaceFound; x++)
                            for (int y = houseY - 1; y <= houseY + houseHeight; y++)
                                if (x >= 0 && x < 20 && y >= 0 && y < 20 && layout[x, y] != '.')
                                    spaceFound = false;

                    } while (!spaceFound);

                    // Construire la maison
                    for (int x = houseX; x < houseX + houseWidth; x++)
                        for (int y = houseY; y < houseY + houseHeight; y++)
                            layout[x, y] = 'H'; // H représente un bâtiment

                    // Placer une porte au milieu d'un des côtés de la maison
                    switch (random.Next(4))
                    {
                        case 0: // Porte en droite
                            layout[houseX + houseWidth / 2, houseY] = ']'; 
                            break;
                        case 1: // Porte en gauche
                            layout[houseX + houseWidth / 2, houseY + houseHeight - 1] = '['; 
                            break;
                        case 2: // Porte à haut
                            layout[houseX, houseY + houseHeight / 2] = '―'; 
                            break;
                        case 3: // Porte à bas
                            layout[houseX + houseWidth - 1, houseY + houseHeight / 2] = '―'; 
                            break;
                    }
                }
            }

            if (isSpecialMap)
            {
                // Créer une forteresse ou tout autre structure spéciale
            }

            /*// S'assurer que les bords sont praticables
            for (int i = 0; i < 20; i++)
            {
                layout[0, i] = layout[19, i] = '.';
                layout[i, 0] = layout[i, 19] = '.';
            }*/

            // Gérer les cartes de bord
            // Gérer les cartes de bord
            if (isBorderMap)
            {
                // Appliquer l'eau et le sable sur les côtés extérieurs
                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        // Placer l'eau de façon aléatoire sur les bords
                        if (mapX == 0 && i < random.Next(1, 5)) layout[i, j] = '~';
                        if (mapX == worldSize - 1 && i > 15 + random.Next(0, 4)) layout[i, j] = '~';
                        if (mapY == 0 && j < random.Next(1, 5)) layout[i, j] = '~';
                        if (mapY == worldSize - 1 && j > 15 + random.Next(0, 4)) layout[i, j] = '~';
                    }
                }

                // Ajouter du sable pour la transition
                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 20; j++)
                    {
                        if (layout[i, j] == '.' && AdjacentToWater(layout, i, j))
                        {
                            layout[i, j] = 'S'; // 'S' pour sable
                        }
                    }
                }
            }

            return layout;
        }

        private bool AdjacentToWater(char[,] layout, int x, int y)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int checkX = x + i;
                    int checkY = y + j;

                    if (checkX >= 0 && checkX < 20 && checkY >= 0 && checkY < 20)
                    {
                        if (layout[checkX, checkY] == '~') return true;
                    }
                }
            }
            return false;
        }

        public Map? GetMapAt(int x, int y)
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
            // Obtenir la carte actuelle
            Map? currentMap = GetMapAt(player.WORLDX, player.WORLDY);
            if (currentMap == null) return;

            int mapHeight = 18;
            int mapWidth = 18;

            // Stocker les anciennes coordonnées locales
            int oldLocalX = player.LOCALX;
            int oldLocalY = player.LOCALY;

            bool needsNewMap = false;

            // Déterminer si le joueur atteint les bords de la carte
            if (player.LOCALX < 1)
            {
                player.WORLDX--;
                player.LOCALX = mapWidth;
                needsNewMap = true;
            }
            else if (player.LOCALX > mapHeight)
            {
                player.WORLDX++;
                player.LOCALX = 0;
                needsNewMap = true;
            }
            else if (player.LOCALY < 1)
            {
                player.WORLDY--;
                player.LOCALY = mapHeight;
                needsNewMap = true;
            }
            else if (player.LOCALY > mapWidth)
            {
                player.WORLDY++;
                player.LOCALY = 0;
                needsNewMap = true;
            }

            // Vérifier les limites du monde
            if (player.WORLDX < 0 || player.WORLDX >= worldSize || player.WORLDY < 0 || player.WORLDY >= worldSize)
            {
                needsNewMap = false;
                player.WORLDX = Math.Max(0, Math.Min(player.WORLDX, worldSize - 1));
                player.WORLDY = Math.Max(0, Math.Min(player.WORLDY, worldSize - 1));
            }

            if (needsNewMap)
            {
                // Effacer la position du joueur sur l'ancienne carte
                currentMap.ClearPlayerPosition(oldLocalX, oldLocalY);

                // Placer le joueur sur la nouvelle carte
                Map? newMap = GetMapAt(player.WORLDX, player.WORLDY);
                if (newMap != null)
                {
                    newMap.PlacePlayer(player.LOCALX, player.LOCALY);
                }
            }
        }

        public void CheckForEncounter(Player player, Allies allies, Enemy enemy)
        {
            if (enemyMaps.Count != 0)
            {
                for (int i = 0; i < enemyMaps.Count; i++)
                {
                    if (enemyMaps[i].LOCALX == player.LOCALX && enemyMaps[i].WORLDX == player.WORLDX && enemyMaps[i].WORLDY == player.WORLDY && !enemyMaps[i].COMBATSTART)
                    {
                        // Gérer la rencontre entre le joueur et l'ennemi
                        HandleEncounter(allies, enemy, player);
                        enemyMaps[i].COMBATSTART = true;
                    }
                }
            }
        }

        public void CheckRandEnemy(Player player, Allies allies, Enemy enemy)
        {
            int randEnemy = random.Next(1, 19);
            if (randEnemy == player.LOCALX)
            {
                fight.startCombat(allies.entitiesContainer, enemy.entitiesContainer, true, player);
            }
        }

        private void HandleEncounter(Allies allies, Enemy enemy, Player p)
        {
            // Combat entre le joueur et l'ennemi
            fight.startCombat(allies.entitiesContainer, enemy.entitiesContainer, false, p);
        }

        private void EnsurePlayerOnLand(Map map, Player player)
        {
            if (map.IsWater(player.LOCALX, player.LOCALY) || !map.CanMoveTo(player.LOCALX, player.LOCALY))
            {
                // Trouver la case d'herbe la plus proche pour repositionner le joueur
                for (int offsetX = -1; offsetX <= 1; offsetX++)
                {
                    for (int offsetY = -1; offsetY <= 1; offsetY++)
                    {
                        int newX = player.LOCALX + offsetX;
                        int newY = player.LOCALY + offsetY;

                        if (newX >= 0 && newX < 20 && newY >= 0 && newY < 20 && map.IsGrass(newX, newY))
                        {
                            map.ClearPlayerPosition(player.LOCALX, player.LOCALY); // Effacer l'ancienne position
                            map.PlacePlayer(newX, newY); // Placer le joueur sur cette position d'herbe
                            player.LOCALX = newX;
                            player.LOCALY = newY;
                            return;
                        }
                    }
                }

                // Si aucune case d'herbe n'est trouvée, placez le joueur sur la position originale
                map.PlacePlayer(player.LOCALX, player.LOCALY);
            }
            else
            {
                map.PlacePlayer(player.LOCALX, player.LOCALY);
            }
        }
        private void InitializeEnemy()
        {
            int enemyLocalX = random.Next(1, 19);
            int enemyLocalY = random.Next(1, 19);

            EnemyMap newEnemyMap = new EnemyMap(1, 1, enemyLocalX, enemyLocalY);
            enemyMaps.Add(newEnemyMap);

            Map centerMap = worldMaps[1, 1];
            centerMap.PlaceEnemy(newEnemyMap.LOCALX, newEnemyMap.LOCALY);
        }

        private void PlaceEnemiesRandomly(Map map, int positionX, int positionY)
        {
            Random random = new Random();
            int numberOfEnemies = random.Next(1, 3);

            for (int i = 0; i < numberOfEnemies; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(20);
                    y = random.Next(20);
                } while (map.IsWater(x, y) || map.IsPlayer(x, y) || map.matrix[x, y] == 'O');

                EnemyMap newEnemyMap = new EnemyMap(positionX, positionY, x, y);
                enemyMaps.Add(newEnemyMap);
                map.PlaceEnemy(x, y);
            }
        }

        public List<EnemyMap> GetEnemyMaps()
        {
            return enemyMaps;
        }
    }
}
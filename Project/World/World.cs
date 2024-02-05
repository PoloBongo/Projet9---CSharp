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
                    bool isCenterMap = (i == 1 && j == 1);
                    bool isSpecialMap = (i == 0 && j == 2);
                    char[,] selectedLayout = CreateRandomLayout(isCenterMap, isSpecialMap);
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

        private char[,] CreateRandomLayout(bool isCenterMap, bool isSpecialMap = false)
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
                // Créer un village au centre
                for (int x = 10; x <= 16; x++)
                    for (int y = 12; y <= 18; y++)
                        layout[x, y] = 'H'; // H représente un bâtiment

                layout[13, 18] = layout[13, 12] = ']'; // Portes sur les côtés gauche et droit
            }

            if (isSpecialMap)
            {
                // Créer une forteresse ou tout autre structure spéciale
            }

            // S'assurer que les bords sont praticables
            for (int i = 0; i < 20; i++)
            {
                layout[0, i] = layout[19, i] = '.';
                layout[i, 0] = layout[i, 19] = '.';
            }

            return layout;
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
            Map? currentMap = GetMapAt(player.WORLDX, player.WORLDY);
            if (currentMap == null) return;

            int oldLocalX = player.LOCALX;
            int oldLocalY = player.LOCALY;

            int oldWorldX = player.WORLDX;
            int oldWorldY = player.WORLDY;

            int mapHeight = 18;
            int mapWidth = 18;

            bool needsNewMap = false;

            // Déterminer la nouvelle position mondiale du joueur
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
                Map? oldMap = GetMapAt(oldWorldX, oldWorldY);
                if (oldMap != null)
                {
                    oldMap.ClearPlayerPosition(oldLocalX, oldLocalY);
                }

                // Placer le joueur sur la nouvelle carte
                Map? newMap = GetMapAt(player.WORLDX, player.WORLDY);
                if (newMap != null)
                {
                    newMap.PlacePlayer(player.LOCALX, player.LOCALX);
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
            if(randEnemy == player.LOCALX)
            {
                fight.startCombat(allies.entitiesContainer, enemy.entitiesContainer, true, player);
            }
        }

        private void HandleEncounter(Allies allies, Enemy enemy, Player player)
        {
            // Combat entre le joueur et l'ennemi
            fight.startCombat(allies.entitiesContainer, enemy.entitiesContainer, false, player);
        }

        private void EnsurePlayerOnLand(Map currentMap, Player player)
        {
            if (currentMap.IsWater(player.LOCALX, player.LOCALY))
            {
                // Trouver la case d'herbe la plus proche pour repositionner le joueur
                for (int offsetX = -1; offsetX <= 1; offsetX++)
                {
                    for (int offsetY = -1; offsetY <= 1; offsetY++)
                    {
                        int newX = player.LOCALX + offsetX;
                        int newY = player.LOCALY + offsetY;

                        if (newX >= 0 && newX < 20 && newY >= 0 && newY < 20)
                        {
                            if (currentMap.IsGrass(newX, newY))
                            {
                                // Place le joueur sur cette position d'herbe
                                currentMap.PlacePlayer(newX, newY);
                                player.LOCALX = newX;
                                player.LOCALY = newY;
                                return;
                            }
                        }
                    }
                }
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
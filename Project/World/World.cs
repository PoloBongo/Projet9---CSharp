using MapEntities;
using Newtonsoft.Json;
using Project.Quest;
using static Project.Quest.QuestNPC;

namespace MapGame
{
    public class World
    {
        public EntityContainer EntityContainer { get; private set; }

        private Map[,] worldMaps;
        private int worldSize = 3;
        private List<EnemyMap> enemyMaps = new List<EnemyMap>();
        private List<int> positionX = new List<int>();
        private bool CombatStart = false;
        Fight fight = new Fight();
        Random random = new Random();

        private List<QuestNPC> questNPCs;


        public Player player;
        public List<Npc> npcs;


        public World()
        {
            {
                worldMaps = new Map[worldSize, worldSize];
                npcs = new List<Npc>();
                EntityContainer = new EntityContainer();

                InitializeWorld();

                // Initialisation des quêtes après avoir initialisé worldMaps
                questNPCs = new List<QuestNPC>
            {
                new QuestNPC(1, 1, "Quest 1 Description", worldMaps[0, 2]),
                new QuestNPC(2, 3, "Quest 2 Description", worldMaps[0, 2]),
            };
            }
            worldMaps = new Map[worldSize, worldSize];
            npcs = new List<Npc>();
            EntityContainer = new EntityContainer();
            InitializeWorld();
        }
        public List<QuestNPC> GetQuestNPCs()
        {
            return questNPCs;
        }
        private void InitializeWorld()
        {
            // Assurez-vous que worldMaps est initialisé correctement
            if (worldMaps == null)
            {
                Console.WriteLine("Erreur : le tableau worldMaps n'est pas initialisé.");
                return;
            }

            for (int i = 0; i < worldSize; i++)
            {
                for (int j = 0; j < worldSize; j++)
                {
                    // Vérifiez si les indices i et j sont valides
                    if (i < 0 || i >= worldSize || j < 0 || j >= worldSize)
                    {
                        Console.WriteLine("Erreur : indices i ou j hors limites.");
                        continue; // Passez à l'itération suivante de la boucle
                    }

                    bool isBorderMap = IsBorderMap(i, j);
                    bool isCenterMap = (i == 1 && j == 1);
                    bool isSpecialMap = (i == 0 && j == 2);
                    char[,] selectedLayout = CreateRandomLayout(isBorderMap, isCenterMap, isSpecialMap, i, j);

                    // Vérifiez si worldMaps[i, j] est null avant de l'initialiser
                    if (worldMaps[i, j] == null)
                    {
                        worldMaps[i, j] = new Map(20, 20);
                    }

                    // Vérifiez à nouveau si worldMaps[i, j] est null avant d'appeler InitializeCustomMap
                    if (worldMaps[i, j] != null)
                    {
                        worldMaps[i, j].InitializeCustomMap(selectedLayout);
                    }
                    else
                    {
                        Console.WriteLine($"Erreur : worldMaps[{i}, {j}] est null.");
                        continue; // Passez à l'itération suivante de la boucle
                    }

                    if (!(i == 1 && j == 1) && !(i == 0 && j == 2))
                    {
                        PlaceEnemiesRandomly(worldMaps[i, j], i, j);
                    }
                    if (i == 1 && j == 1)
                    {
                        if (worldMaps[i, j] == null)
                        {
                            Console.WriteLine($"Erreur : La carte à la position ({i}, {j}) n'est pas initialisée.");
                        }
                        else if (questNPCs == null)
                        {
                            Console.WriteLine("Erreur : questNPCs n'est pas initialisé.");
                        }
                        else
                        {
                            Map map = worldMaps[i, j]; // Obtenez la carte à la position actuelle
                            questNPCs.Add(new QuestNPC(10, 10, "Test", map));
                        }
                    }
                }
            }
          InitializeEnemy();
       }





        public char[,] CreateRandomLayout(bool isBorderMap, bool isCenterMap, bool isSpecialMap, int mapX, int mapY)
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
                CreateFortress(layout);
            }

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
        private void CreateFortress(char[,] layout)
        {
            int fortressWidth = 10;
            int fortressHeight = 10;
            int startX = (20 - fortressWidth) / 2;
            int startY = (20 - fortressHeight) / 2;

            // Construire les murs de la forteresse
            for (int x = startX; x < startX + fortressWidth; x++)
            {
                for (int y = startY; y < startY + fortressHeight; y++)
                {
                    if (x == startX || x == startX + fortressWidth - 1 || y == startY || y == startY + fortressHeight - 1)
                    {
                        layout[x, y] = 'F'; // 'F' pour les murs de la forteresse
                    }
                    else
                    {
                        layout[x, y] = ' '; // Espace ou autre caractère pour l'intérieur de la forteresse
                    }
                }
            }

            // Ajouter une porte à la forteresse
            layout[startX + fortressWidth / 2, startY] = 'D'; // 'D' pour porte

            // Placer des ennemis à l'intérieur de la forteresse
            PlaceEnemiesInFortress(startX, startY, fortressWidth, fortressHeight, layout);
        }

        private void PlaceEnemiesInFortress(int startX, int startY, int width, int height, char[,] layout)
        {
            for (int i = 0; i < 5; i++) // Nombre d'ennemis
            {
                int x, y;
                do
                {
                    x = random.Next(startX + 1, startX + width - 1);
                    y = random.Next(startY + 1, startY + height - 1);
                } while (layout[x, y] != ' '); // Modifier pour correspondre à l'espace vide

                EnemyMap newEnemyMap = new EnemyMap(1, 1, x, y);
                enemyMaps.Add(newEnemyMap);
            }
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
        private bool IsBorderMap(int x, int y)
        {
            // Une carte est sur le bord si elle est sur la première ou la dernière ligne/colonne
            return x == 0 || y == 0 || x == worldSize - 1 || y == worldSize - 1;
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
                    // Si la position initiale sur la nouvelle carte est de l'eau, chercher une position d'herbe
                    if (newMap.IsWater(player.LOCALX, player.LOCALY))
                    {
                        PlacePlayerOnNearestGrass(newMap, player);
                    }
                    else
                    {
                        // Si la position initiale est praticable, placer le joueur là
                        newMap.PlacePlayer(player.LOCALX, player.LOCALY);
                    }
                }
            }
        }

        private void PlacePlayerOnNearestGrass(Map map, Player player)
        {
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    if (map.IsGrass(x, y))
                    {
                        map.PlacePlayer(x, y);
                        player.LOCALX = x;
                        player.LOCALY = y;
                        return;
                    }
                }
            }
        }

        public bool IsNextToWood(Player player)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            Map currentMap = GetMapAt(player.WORLDX, player.WORLDY);
            int playerLocalX = player.LOCALX;
            int playerLocalY = player.LOCALY;

            // Vérifier les cases autour de la position du joueur
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (currentMap.IsWood(playerLocalX + i, playerLocalY + j))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool IsPlayerNextToDoor(Player player)
        {
            Map currentMap = GetMapAt(player.WORLDX, player.WORLDY);
            return currentMap != null && currentMap.IsNextToDoor(player.LOCALX, player.LOCALY);
        }

        public bool IsPlayerNextToFortressDoor(Player player)
        {
            Map currentMap = GetMapAt(player.WORLDX, player.WORLDY);
            return currentMap != null && currentMap.IsNextToFortressDoor(player.LOCALX, player.LOCALY);
        }


        public void CheckForEncounter(Player player, Allies allies, Enemy enemy)
        {
            if (enemyMaps.Count != 0)
            {
                for (int i = 0; i < enemyMaps.Count; i++)
                {
                    if (enemyMaps[i].LOCALX == player.LOCALX && enemyMaps[i].WORLDX == player.WORLDX && enemyMaps[i].WORLDY == player.WORLDY && !enemyMaps[i].COMBATSTART)
                    {
                        int randChance = random.Next(100);

                        int chanceStartCombat1 = 50;
                        int chanceStartCombat2 = 30;
                        int chanceStartCombat3 = 20; 

                        if (randChance < chanceStartCombat1)
                        {
                            HandleEncounter(allies, enemy, player, 1);
                        }
                        else if (randChance < chanceStartCombat1 + chanceStartCombat2)
                        {
                            HandleEncounter(allies, enemy, player, 2);
                        }
                        else if (randChance < chanceStartCombat1 + chanceStartCombat2 + chanceStartCombat3)
                        {
                            HandleEncounter(allies, enemy, player, 3);
                        }

                        enemyMaps[i].COMBATSTART = true;
                    }
                }
            }
        }

        private void HandleEncounter(Allies allies, Enemy enemy, Player p, int combatType)
        {
            // Combat entre le joueur et l'ennemi
            fight.startCombat(allies.entitiesContainer, false, p, combatType);
        }

        private void InitializeEnemy()
        {
            // Générer un nombre aléatoire entre 0 et 99
            int randChance = random.Next(100);
            int chanceSpawnEnemy = 70; // Par exemple, 70% de chance de faire apparaître un ennemi lors de l'initialisation

            // Vérifier si le nombre aléatoire est inférieur au seuil de pourcentage
            if (randChance < chanceSpawnEnemy)
            {
                int enemyLocalX = random.Next(1, 19);
                int enemyLocalY = random.Next(1, 19);

                EnemyMap newEnemyMap = new EnemyMap(1, 1, enemyLocalX, enemyLocalY);
                enemyMaps.Add(newEnemyMap);

                Map centerMap = worldMaps[1, 1];
                centerMap.PlaceEnemy(newEnemyMap.LOCALX, newEnemyMap.LOCALY);
            }
        }

        private void PlaceEnemiesRandomly(Map map, int positionX, int positionY)
        {
            int chanceSpawnEnemy = 50; // Par exemple, 50% de chance de placer un ennemi aléatoire

            for (int i = 0; i < 3; i++) // Vous pouvez ajuster le nombre d'ennemis à placer
            {
                // Générer un nombre aléatoire entre 0 et 99 pour chaque ennemi
                int randChance = random.Next(100);

                // Vérifier si le nombre aléatoire est inférieur au seuil de pourcentage
                if (randChance < chanceSpawnEnemy)
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
        }
        public void CheckRandEnemy(Player player, Allies allies, Enemy enemy)
        {
            int randEnemy = random.Next(1, 19);
            if (randEnemy == player.LOCALX)
            {
                int randChance = random.Next(100);
                int chanceStartCombat1 = 50;  // Par exemple, 50% de chance pour le premier type de combat

                if (randChance < chanceStartCombat1)
                {
                    fight.startCombat(allies.entitiesContainer, true, player, 1); // Passer en paramètre le type de combat
                }
            }
        }

        public void StartFortressBattle(Player player, World world)
        {
            // Combat à la forteresse
        }

        public List<EnemyMap> GetEnemyMaps()
        {
            return enemyMaps;
        }

        public void DisplayInventoryAndTeam(Player player, EntityContainer entityContainer)
        {
            int inventoryX = 43;
            int inventoryY = 2;

            // Créez un cadre pour l'inventaire
            DrawBox(inventoryX - 2, inventoryY - 1, 30, 15);

            // Titre de l'inventaire
            Console.SetCursorPosition(inventoryX, inventoryY++);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t     Inventaire");
            Console.ForegroundColor = ConsoleColor.Gray;

            // Affiche les détails de l'inventaire
            Console.SetCursorPosition(inventoryX, inventoryY++);
            Console.WriteLine($"  Viande : {player.NBViande}");
            Console.SetCursorPosition(inventoryX, inventoryY++);
            Console.WriteLine($"  Alcool : {player.NBAlcool}");
            Console.SetCursorPosition(inventoryX, inventoryY++);
            Console.WriteLine($"  Or      : {player.NBGold}");

            // Séparation visuelle
            Console.SetCursorPosition(inventoryX, inventoryY++);
            Console.WriteLine(new string('-', 30));

            if (entityContainer.AlliesList != null)
            {
                // Affiche l'équipe des alliés
                Console.SetCursorPosition(inventoryX, inventoryY++);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\t\tEquipe");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(inventoryX, inventoryY++);

                for (int i = 0; i < entityContainer.AlliesList.Count; i++)
                {
                    var ally = entityContainer.AlliesList[i];
                    if (ally != null)
                    {
                        UpdateInfoAllies(entityContainer, "../../../Entities/entity.json");
                        Console.SetCursorPosition(inventoryX, inventoryY);

                        // Nom du personnage en couleur différente
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(ally._name);
                        Console.ResetColor();

                        // HP en couleur différente
                        Console.Write(" - HP:");
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(ally._health);
                        Console.ResetColor();

                        // Stamina en couleur différente
                        Console.Write(" - Stamina:");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine(ally._stamina);
                        Console.ResetColor();

                        inventoryY++;
                    }
                    else
                    {
                        Console.SetCursorPosition(inventoryX, inventoryY++);
                        Console.WriteLine("Allié non initialisé");
                    }
                }
            }
        }

        public void UpdateInfoAllies(EntityContainer entityContainer, string path)
        {
            string json = File.ReadAllText(path);
            var entities = JsonConvert.DeserializeObject<EntityContainer>(json);

            foreach (var ally in entities.AlliesList)
            {
                var targetAlly = entityContainer.AlliesList.FirstOrDefault(a => a._name.Equals(ally._name, StringComparison.OrdinalIgnoreCase));
                if (targetAlly != null)
                {
                    targetAlly._health = ally._health;
                    targetAlly._stamina = ally._stamina;
                }
            }
        }

        // Méthode pour dessiner un cadre autour de l'inventaire
        private void DrawBox(int x, int y, int width, int height)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            string horizontalLine = "+" + new string('-', width + 2) + "+";

            // Dessine la ligne supérieure
            Console.SetCursorPosition(x, y);
            Console.Write(horizontalLine);

            // Dessine les côtés
            for (int i = 1; i < height - 1; i++)
            {
                Console.SetCursorPosition(x, y + i);
                Console.Write("|");
                Console.SetCursorPosition(x + width + 3, y + i);
                Console.Write("|");
            }

            // Dessine la ligne inférieure
            Console.SetCursorPosition(x, y + height - 1);
            Console.Write(horizontalLine);

            Console.ResetColor();
        }
    }
}
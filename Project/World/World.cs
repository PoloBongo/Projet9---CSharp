﻿using MapEntities;
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
        List<string> alliesNames;

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

            }
            worldMaps = new Map[worldSize, worldSize];
            npcs = new List<Npc>();
            EntityContainer = new EntityContainer();
            InitializeWorld();
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
                    bool isBorderMap = IsBorderMap(i, j);
                    bool isCenterMap = (i == 1 && j == 1);
                    bool isSpecialMap = (i == 0 && j == 2);
                    char[,] selectedLayout = CreateRandomLayout(isBorderMap, isCenterMap, isSpecialMap, i, j);
                    worldMaps[i, j] = new Map(20, 20);
                    worldMaps[i, j].InitializeCustomMap(selectedLayout);
                    if (i != 1 && j != 1 && i != 0 && j != 2)
                    {
                        PlaceEnemiesRandomly(worldMaps[i, j], i, j);
                    }
                }
            }
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
                int numberOfLakes = random.Next(3, 5);

                for (int lake = 0; lake < numberOfLakes; lake++)
                {

                    int lakeStartX = random.Next(3, 17);
                    int lakeStartY = random.Next(3, 17);
                    
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
                // Détermine le nombre de maisons à placer
                int numberOfHouses = random.Next(3, 5);

                // Crée chaque maison
                for (int house = 0; house < numberOfHouses; house++)
                {
                    int houseWidth = random.Next(1, 3) * 2 + 1;
                    int houseHeight = random.Next(1, 3) * 2 + 1;

                    int houseX, houseY;
                    bool spaceFound;
                    do
                    {
                        spaceFound = true;
                        houseX = random.Next(1, 20 - houseWidth);
                        houseY = random.Next(1, 20 - houseHeight);

                        for (int x = houseX - 1; x <= houseX + houseWidth && spaceFound; x++)
                            for (int y = houseY - 1; y <= houseY + houseHeight; y++)
                                if (x >= 0 && x < 20 && y >= 0 && y < 20 && layout[x, y] != '.')
                                    spaceFound = false;

                    } while (!spaceFound);

                    // Construire la maison uniquement sur la carte du village
                    if (mapX == 1 && mapY == 1)
                    {
                        for (int x = houseX; x < houseX + houseWidth; x++)
                            for (int y = houseY; y < houseY + houseHeight; y++)
                                layout[x, y] = 'H'; // H pour maison

                        // Placer une porte
                        switch (random.Next(4))
                        {
                            case 0: layout[houseX + houseWidth / 2, houseY] = ']'; break;
                            case 1: layout[houseX + houseWidth / 2, houseY + houseHeight - 1] = '['; break;
                            case 2: layout[houseX, houseY + houseHeight / 2] = '―'; break;
                            case 3: layout[houseX + houseWidth - 1, houseY + houseHeight / 2] = '―'; break;
                        }
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
            layout[startX + fortressWidth / 2, startY] = 'D';
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
                        HandleEncounter(allies, enemy, player, 2);

                        enemyMaps[i].COMBATSTART = true;
                    }
                }
            }
        }

        private void HandleEncounter(Allies allies, Enemy enemy, Player p, int combatType)
        {
            // Combat entre le joueur et l'ennemi
            fight.StartCombat(allies.entitiesContainer, false, p, combatType);
        }

        private void PlaceEnemiesRandomly(Map map, int positionX, int positionY)
        {
            

            for (int i = 0; i < 3; i++)
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
        public void CheckRandEnemy(Player player, Allies allies)
        {
            int randEnemy = random.Next(1, 19);
            if (randEnemy == player.LOCALX && !(player.WORLDX == 1 && player.WORLDY == 1))
            {
                fight.StartCombat(allies.entitiesContainer, true, player, 1); // Passer en paramètre le type de combat
            }
        }

        public void StartFortressBattle(Player player, Allies allies)
        {
            // Combat à la forteresse
            fight.StartCombat(allies.entitiesContainer, false, player, 3);
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

            if (entityContainer.alliesList != null)
            {
                // Affiche l'équipe des alliés
                Console.SetCursorPosition(inventoryX, inventoryY++);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\t\tEquipe");
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.SetCursorPosition(inventoryX, inventoryY++);

                for (int i = 0; i < entityContainer.alliesList.Count; i++)
                {
                    var ally = entityContainer.alliesList[i];
                    if (ally != null)
                    {
                        UpdateInfoAllies(entityContainer, "../../../Entities/entity.json");
                        Console.SetCursorPosition(inventoryX, inventoryY);

                        // Nom du personnage en couleur différente
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.Write(ally.name);
                        Console.ResetColor();

                        // HP en couleur différente
                        Console.Write(" - HP:");
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(ally.health);
                        Console.ResetColor();

                        // Stamina en couleur différente
                        Console.Write(" - Stamina:");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine(ally.stamina);
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

        public void DisplayInventoryAndTeam2(Player player, EntityContainer entityContainer, ref EntityAbstract allie)
        {
            List<string> options;
            int selectedIndex;

            options = new List<string> { $"Viande : {player.NBViande}", $"Alcool : {player.NBAlcool}", $"Or : {player.NBGold}", "Fermer" };
            selectedIndex = RunOptionsInventory(options, allie);

            switch (selectedIndex)
            {
                case 0:
                    if (player.NBViande > 0)
                    {
                        alliesNames = entityContainer.alliesList
                            .Select(a => a.name)
                            .ToList();

                        EntityAbstract newAllie = null;
                        do
                        {
                            selectedIndex = RunOptionsInventory(alliesNames, allie);
                            string selectedName = alliesNames[selectedIndex];
                            newAllie = entityContainer.alliesList.FirstOrDefault(a => a.name == selectedName);
                        } while (newAllie == null);

                        allie = newAllie;
                        allie.AddHealth(20);
                        player.RemoveViande(1);
                        AddAllyJson(allie, "../../../Entities/entity.json", 20, "Health");
                    }
                    break;
                case 1:
                    if (player.NBAlcool > 0)
                    {
                        alliesNames = entityContainer.alliesList
                            .Select(a => a.name)
                            .ToList();

                        EntityAbstract newAllie = null;
                        do
                        {
                            selectedIndex = RunOptionsInventory(alliesNames, allie);
                            string selectedName = alliesNames[selectedIndex];
                            newAllie = entityContainer.alliesList.FirstOrDefault(a => a.name == selectedName);
                        } while (newAllie == null);

                        allie = newAllie;
                        allie.AddStamina(20);
                        player.RemoveAlcool(1);
                        AddAllyJson(allie, "../../../Entities/entity.json", 20, "Stamina");
                    }
                    break;
                default:
                    break;
            }
        }

        public void DisplayInfoAllies(EntityContainer entityContainer)
        {
            List<string> options;
            int selectedIndex;
            
            options = new List<string> { "Fermer" };
            selectedIndex = RunOptionsInfo(options, entityContainer);

            switch (selectedIndex)
            {
                case 0:
                    break;
                default:
                    break;
            }
        }

        private void AddAllyJson(EntityAbstract entity, string path, int countType, string type)
        {
            EntityContainer entities;

            // Utilisez un bloc using pour libérer la ressource après la lecture du fichier
            using (StreamReader reader = File.OpenText(path))
            {
                string json = reader.ReadToEnd();
                entities = JsonConvert.DeserializeObject<EntityContainer>(json);
            }

            if (entity is Allies)
            {
                var targetAllies = entities.alliesList.FirstOrDefault(a => a.name.Equals(entity.name, StringComparison.OrdinalIgnoreCase));
                if (targetAllies != null)
                {
                    if (type == "Health")
                    {
                        targetAllies.health += countType;
                    }
                    else if (type == "Stamina")
                    {
                        targetAllies.stamina += countType;
                    }
                }
            }

            // Utilisez à nouveau un bloc using pour libérer la ressource après l'écriture dans le fichier
            using (StreamWriter writer = File.CreateText(path))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(writer, entities);
                writer.Close();
            }
        }


        public int RunOptionsInventory(List<string> options, EntityAbstract allie)
        {
            ConsoleKey keyPressed;
            int selectedIndex = 0;
            do
            {
                DisplayOptionsInventory(options, selectedIndex, allie);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

                if (keyPressed == ConsoleKey.UpArrow)
                {
                    selectedIndex = (selectedIndex > 0) ? selectedIndex - 1 : options.Count - 1;
                }
                else if (keyPressed == ConsoleKey.DownArrow)
                {
                    selectedIndex = (selectedIndex < options.Count - 1) ? selectedIndex + 1 : 0;
                }
            } while (keyPressed != ConsoleKey.Enter);

            return selectedIndex;
        }

        public int RunOptionsInfo(List<string> options, EntityContainer entities)
        {
            ConsoleKey keyPressed;
            int selectedIndex = 0;
            do
            {
                DisplayOptionsInfo(options, selectedIndex, entities);

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                keyPressed = keyInfo.Key;

            } while (keyPressed != ConsoleKey.Enter);

            return selectedIndex;
        }

        private void DisplayOptionsInventory(List<string> options, int selectedIndex, EntityAbstract allie)
        {
            Console.Clear();

            string inventory = @"

██╗███╗   ██╗██╗   ██╗███████╗███╗   ██╗████████╗ ██████╗ ██████╗ ██╗   ██╗
██║████╗  ██║██║   ██║██╔════╝████╗  ██║╚══██╔══╝██╔═══██╗██╔══██╗╚██╗ ██╔╝
██║██╔██╗ ██║██║   ██║█████╗  ██╔██╗ ██║   ██║   ██║   ██║██████╔╝ ╚████╔╝ 
██║██║╚██╗██║╚██╗ ██╔╝██╔══╝  ██║╚██╗██║   ██║   ██║   ██║██╔══██╗  ╚██╔╝  
██║██║ ╚████║ ╚████╔╝ ███████╗██║ ╚████║   ██║   ╚██████╔╝██║  ██║   ██║   
╚═╝╚═╝  ╚═══╝  ╚═══╝  ╚══════╝╚═╝  ╚═══╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝   ╚═╝   
                                                                           
                                                                           
                                                                           



            ";

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(inventory);
            Console.ResetColor();

            int inventoryX = 10;
            int inventoryY = 20;

            // Créez un cadre pour l'inventaire
            DrawBox(inventoryX - 2, inventoryY - 5, 30, 15);

            // Titre de l'inventaire
            Console.SetCursorPosition(inventoryX, inventoryY++);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t     Inventaire\n");
            for (int i = 0; i < options.Count; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine($"\t\t* {options[i]}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine($"\t\t  {options[i]}");
                }
            }
            Console.ResetColor();
        }

        private void DisplayOptionsInfo(List<string> options, int selectedIndex, EntityContainer entities)
        {
            Console.Clear();

            string informations = @"



██╗███╗   ██╗███████╗ ██████╗ ██████╗ ███╗   ███╗ █████╗ ████████╗██╗ ██████╗ ███╗   ██╗███████╗
██║████╗  ██║██╔════╝██╔═══██╗██╔══██╗████╗ ████║██╔══██╗╚══██╔══╝██║██╔═══██╗████╗  ██║██╔════╝
██║██╔██╗ ██║█████╗  ██║   ██║██████╔╝██╔████╔██║███████║   ██║   ██║██║   ██║██╔██╗ ██║███████╗
██║██║╚██╗██║██╔══╝  ██║   ██║██╔══██╗██║╚██╔╝██║██╔══██║   ██║   ██║██║   ██║██║╚██╗██║╚════██║
██║██║ ╚████║██║     ╚██████╔╝██║  ██║██║ ╚═╝ ██║██║  ██║   ██║   ██║╚██████╔╝██║ ╚████║███████║
╚═╝╚═╝  ╚═══╝╚═╝      ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚═╝  ╚═╝   ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝╚══════╝
                                                                                                
                                                                                                
                                                                                                



            ";

            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(informations);
            Console.ResetColor();

            entities = JsonConvert.DeserializeObject<EntityContainer>(File.ReadAllText("../../../Entities/entity.json"));
            int inventoryX = 10;
            int inventoryY = 15;

            // Titre de l'inventaire
            Console.SetCursorPosition(inventoryX, inventoryY++);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t     Informations\n");

            Console.WriteLine("Persoonage :\n");
            for (int i = 0; i < entities.alliesList.Count(); i++)
            {
                Console.WriteLine($"{i + 1}. {entities.alliesList[i].name} - Level : {entities.alliesList[i].level}");
                for (int j = 0; j < entities.alliesList[i].listCapacities.Count(); j++)
                {
                    if (entities.alliesList[i].level >= entities.alliesList[i].listCapacities[j].level)
                    {
                        Console.WriteLine($"Attaque débloqué : {entities.alliesList[i].listCapacities[j].name}");
                    }
                    else if (entities.alliesList[i].level < entities.alliesList[i].listCapacities[j].level)
                    {
                        Console.WriteLine($"Attaque à débloqué : {entities.alliesList[i].listCapacities[j].name} - Level requis : {entities.alliesList[i].listCapacities[j].level}");
                    }
                }
                Console.WriteLine("\n");
            }


            for (int i = 0; i < options.Count; i++)
            {
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.WriteLine($"\t\t* {options[i]}");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.WriteLine($"\t\t  {options[i]}");
                }
            }


            Console.ResetColor();
        }

        public void UpdateInfoAllies(EntityContainer entityContainer, string path)
        {
            // Utilisez un bloc using pour libérer la ressource après la lecture du fichier
            using (StreamReader reader = File.OpenText(path))
            {
                string json = reader.ReadToEnd();
                var entities = JsonConvert.DeserializeObject<EntityContainer>(json);

                foreach (var ally in entities.alliesList)
                {
                    var targetAlly = entityContainer.alliesList.FirstOrDefault(a => a.name.Equals(ally.name, StringComparison.OrdinalIgnoreCase));
                    if (targetAlly != null)
                    {
                        if (ally.health < 0)
                        {
                            ally.health = 0.0f;
                        }
                        else
                        {
                            targetAlly.health = ally.health;
                        }
                        targetAlly.stamina = ally.stamina;
                    }
                }
            }
        }

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
using MapGame;
using MapEntities;
using MenuPr;
using ShopDemo;
using Project.Quest;

namespace InGame
{
    class Game
    {
        Enemy enemy = new Enemy();
        Allies allies = new Allies();
        World world = new World();
        Player player = new Player(1, 1, mapRows / 2, mapColumns / 2);
        Shop shop = new Shop();
        const int mapRows = 20;
        const int mapColumns = 20;


        public void Start()
        {
            string title = "One Piece Game";
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.ResetColor();
            RunMainMenu();
        }

        private void RunMainMenu()
        {
            string prompt = @"
                                                                        


             ██████╗ ███╗   ██╗███████╗    ██████╗ ██╗███████╗ ██████╗███████╗
            ██╔═══██╗████╗  ██║██╔════╝    ██╔══██╗██║██╔════╝██╔════╝██╔════╝
            ██║   ██║██╔██╗ ██║█████╗      ██████╔╝██║█████╗  ██║     █████╗  
            ██║   ██║██║╚██╗██║██╔══╝      ██╔═══╝ ██║██╔══╝  ██║     ██╔══╝  
            ╚██████╔╝██║ ╚████║███████╗    ██║     ██║███████╗╚██████╗███████╗
             ╚═════╝ ╚═╝  ╚═══╝╚══════╝    ╚═╝     ╚═╝╚══════╝ ╚═════╝╚══════╝
                                                                  
                                                          
                                      


            ";
            string[] options = { "Play", "Credit", "Shop", "Exit" };
            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:
                    //NarrateStory();
                    PlayGame();
                    break;
                case 1:
                    Credits();
                    break;
                case 2:
                    Shopping();
                    break;
                case 3:
                    ExitGame();
                    break;
            }
        }

        private void DisplayArt()
        {
            string art = @"
                ███████╗████████╗ ██████╗ ██████╗ ██╗   ██╗    
                ██╔════╝╚══██╔══╝██╔═══██╗██╔══██╗╚██╗ ██╔╝    
                ███████╗   ██║   ██║   ██║██████╔╝ ╚████╔╝     
                ╚════██║   ██║   ██║   ██║██╔══██╗  ╚██╔╝      
                ███████║   ██║   ╚██████╔╝██║  ██║   ██║       
                ╚══════╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝   ╚═╝




            ";



            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(art);
            Console.ResetColor();

        }

        private void NarrateStory()
        {
            Console.Clear();
            Console.ResetColor();
            DisplayArt();

            string[] storyLines = {
                "\tSur une île mystérieuse aux confins du ", "Nouveau Monde, \n",
                "\tLuffy, Ace et Jimbey sont pris au piège, confrontés à un défi inédit.\n",
                "\tCette île, gouvernée par des ", "forces surnaturelles, \n",
                "\tcrée des effets miroirs déroutants, rendant toute évasion impossible.\n",
                "\tPour quitter cette île, nos héros doivent ", "devenir plus forts, \n",
                "\tgagner en compétences et ", "vaincre l'amiral de la Marine, \n",
                "\tle gardien de l'île.\n\n\n", "\t\tL'aventure commence au cœur d'un ",
                "village de marchands...\n"
            };

            ConsoleColor[] lineColors = {
                ConsoleColor.White, ConsoleColor.Cyan, ConsoleColor.White,
                ConsoleColor.White, ConsoleColor.Magenta, ConsoleColor.White,
                ConsoleColor.White, ConsoleColor.Green, ConsoleColor.White,
                ConsoleColor.Yellow, ConsoleColor.White, ConsoleColor.White,
                ConsoleColor.Blue
            };

            for (int i = 0; i < storyLines.Length; i++)
            {
                WriteAnimatedText(storyLines[i], lineColors[i], i % 2 == 0 ? 20 : 60);
            }

            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("\n\t\tAppuyez sur une touche pour commencer l'aventure...");
            Console.ResetColor();
            Console.ReadKey();
        }

        private void WriteAnimatedText(string text, ConsoleColor color, int delay)
        {
            Console.ForegroundColor = color;
            foreach (char c in text)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(delay);
            }
            Console.ResetColor();
        }


        private void PlayGame()
        {
            Console.WriteLine("\t\tLancement du Jeu");

            Enemy enemy = new Enemy();
            Allies allies = new Allies();
            Console.WriteLine("\t\tLancement En Cours");

            World world = new World();
            EntityContainer entities = new EntityContainer();
            Player player = new Player(1, 1, mapRows / 2, mapColumns / 2);
            QuestNPC questNPC = new QuestNPC(0, 0, "Some quest description", world.GetMapAt(player.WORLDX, player.WORLDY));
            List<QuestNPC> questNPCs = world.GetQuestNPCs();



            string path = "../../../Entities/entity.json";
            enemy.CreateEntity(path, entities);
            enemy.GetInfoEntity(path);
            allies.CreateEntity(path, entities);
            allies.GetInfoEntity(path);
            
            while (true)
            {
                Console.Clear();
                world.CheckForEncounter(player, allies, enemy);
                world.CheckRandEnemy(player, allies, enemy);

                Map currentMap = world.GetMapAt(player.WORLDX, player.WORLDY);
                currentMap.PrintMap();
                world.DisplayInventoryAndTeam(player, entities);
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                int newLocalX = player.LOCALX;
                int newLocalY = player.LOCALY;

                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        newLocalX--;
                        break;
                    case ConsoleKey.DownArrow:
                        newLocalX++;
                        break;
                    case ConsoleKey.LeftArrow:
                        newLocalY--;
                        break;
                    case ConsoleKey.RightArrow:
                        newLocalY++;
                        break;
                }

                // Interaction avec les PNJ
                foreach (var questNpc in questNPCs)
                {
                    if (questNpc.IsNear(player))
                    {
                        questNpc.Interact();
                        break; // Sortir de la boucle après avoir interagi avec le PNJ le plus proche
                    }
                }

                // Collecte de bois
                if (world.IsNextToWood(player))
                {
                    Console.WriteLine("\tAppuyez sur 'E' pour ramasser le bois");
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.E)
                    {
                        // Trouver le PNJ le plus proche pour la collecte de bois
                        foreach (var questNpc in questNPCs)
                        {
                            if (questNpc.IsNear(player))
                            {
                                questNpc.CollectWood(); 

                                break; 
                            }
                        }
                    }
                }




                // Gérer le changement de carte si le joueur atteint les bords de la carte actuelle
                if (newLocalX < 0 || newLocalX >= mapRows || newLocalY < 0 || newLocalY >= mapColumns)
                {
                    world.MovePlayerToNewMap(player);
                }
                else if (currentMap.CanMoveTo(newLocalX, newLocalY))
                {
                    // Déplacer le joueur localement
                    currentMap.MovePlayer(player.LOCALX, player.LOCALY, newLocalX, newLocalY);
                    player.LOCALX = newLocalX;
                    player.LOCALY = newLocalY;
                }

 
                // Vérifier si le joueur est à côté d'une porte
                if (world.IsPlayerNextToDoor(player))
                {

                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("\tAppuyez sur 'E' pour entrer");
                    Console.ResetColor();
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.E)
                    {
                        Shop.Run(player);
                    }
                }
                if (world.IsPlayerNextToFortressDoor(player))
                {
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("\t\t   ATTENTION : ZONE DANGEREUSE   ");

                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("\t  Appuyez sur 'E' pour entrer dans la forteresse");
                    Console.ResetColor();

                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.E)
                    {
                        // Déclencher un combat ou une fonction spéciale
                        world.StartFortressBattle(player, world);
                    }
                }
            }
        }
        private void ExitGame()
        {
            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("\t\tAppuyez sur une touche pour Quitter le Jeu");
            Console.ResetColor();
            Console.ReadKey(true);
            Environment.Exit(0);
        }

        private void Credits()
        {
            Console.Clear();
            string[] credits = {
                "\tQuentin LEFORESTIER",
                "\tLucie QUINTANA",
                "\tArthur BRU",
                "\tMathias REBECCA"
            };
            string art = @"

                 ██████╗██████╗ ███████╗██████╗ ██╗████████╗███████╗
                ██╔════╝██╔══██╗██╔════╝██╔══██╗██║╚══██╔══╝██╔════╝
                ██║     ██████╔╝█████╗  ██║  ██║██║   ██║   ███████╗
                ██║     ██╔══██╗██╔══╝  ██║  ██║██║   ██║   ╚════██║
                ╚██████╗██║  ██║███████╗██████╔╝██║   ██║   ███████║
                 ╚═════╝╚═╝  ╚═╝╚══════╝╚═════╝ ╚═╝   ╚═╝   ╚══════╝
                                                    



            ";



            Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Console.WriteLine(art);
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t\t\t\tGame Developed By: \n");
            Console.ResetColor();

            foreach (string line in credits)
            {
                foreach (char c in line)
                {
                    Console.Write(c);
                    System.Threading.Thread.Sleep(50); // Délai pour simuler l'effet de frappe
                }
                Console.WriteLine("\n");
                System.Threading.Thread.Sleep(300); // Délai entre les lignes
            }


            Console.BackgroundColor = ConsoleColor.DarkYellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("\t\tAppuyez sur une touche pour retourner au Menu");
            Console.ResetColor();

            Console.ReadKey(true);
            RunMainMenu();
        }



       private void Shopping()
        {
            //Shop.Run(player);
        }
    }

}

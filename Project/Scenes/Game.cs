using MapGame;
using MapEntities;
using MenuPr;
using ShopDemo;
namespace InGame
{
    class Game
    {
        const int mapRows = 20;
        const int mapColumns = 20;


        public void Start()
        {
            string title = "One Piece Game";
            RunMainMenu();
        }

        private void RunMainMenu()
        {
            string prompt = @"
 ██████  ███    ██ ███████     ██████  ██ ███████  ██████ ███████ 
██    ██ ████   ██ ██          ██   ██ ██ ██      ██      ██
██    ██ ██ ██  ██ █████       ██████  ██ █████   ██      █████
██    ██ ██  ██ ██ ██          ██      ██ ██      ██      ██
 ██████  ██   ████ ███████     ██      ██ ███████  ██████ ███████ 



";
            string[] options = { "Jouer", "Crédits","Shop", "Quitter" };
            Menu mainMenu = new Menu(prompt, options);
            int selectedIndex = mainMenu.Run();

            switch (selectedIndex)
            {
                case 0:
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


        private void PlayGame()
        {
            Enemy enemy = new Enemy();
            Allies allies = new Allies();
            World world = new World();
            Player player = new Player(1, 1, mapRows / 2, mapColumns / 2);

            string path = "../../../Entities/entity.json";
            enemy.CreateEntity(path);
            enemy.GetInfoEntity(path);
            allies.CreateEntity(path);
            allies.GetInfoEntity(path);

            while (true)
            {
                Console.Clear();
                world.CheckForEncounter(player, allies, enemy);
                world.CheckRandEnemy(player, allies, enemy);

                Map currentMap = world.GetMapAt(player.WORLDX, player.WORLDY);
                currentMap.PrintMap();
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

                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("\tAppuyez sur 'E' pour entrer");
                    Console.ResetColor();
                    var key = Console.ReadKey(true);
                    if (key.Key == ConsoleKey.E)
                    {
                        ShopDemo.Shop.Run();
                    }
                }
            }
        }
        private void ExitGame()
        {
            Console.BackgroundColor = ConsoleColor.DarkRed;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("\t\tAppuyer sur une touche pour Quitter le Jeu");
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

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\t\tGame Developed By:\n");
            Console.WriteLine(new string('=', 55));
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

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine("\t\tAppuyer sur une touche pour retourner au Menu");
            Console.ResetColor();

            Console.ReadKey(true);
            RunMainMenu();
        }

        private void Shopping()
        {
            Shop.Run();
        }
    }

}

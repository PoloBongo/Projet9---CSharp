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



Bienvenu chez les pirates ";
            string[] options = { "Play", "Credit","Shop", "Exit" };
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
                Console.WriteLine($"{player.NBViande}");
                world.CheckForEncounter(player, allies, enemy);
                world.CheckRandEnemy(player, allies, enemy);

                Map currentMap = world.GetMapAt(player.WORLDX, player.WORLDY);
                currentMap.PrintMap();
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                int newLocalX = player.LOCALY;
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
            }
        }
        private void ExitGame()
        {
            Console.WriteLine("\n Press any key to exit");
            Console.ReadKey(true);
            Environment.Exit(0);
        }

        private void Credits()
        {
            Console.Clear();
            Console.WriteLine("JSP QUOI ECRIRE");
            Console.WriteLine("\n Press any key to the menu");
            Console.ReadKey(true);
            RunMainMenu();
        }



       private void Shopping()
        {
            Shop.Run();
        }
    }

}

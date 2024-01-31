using MapGame;
using PlayerGame;
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
            Initialization init = new Initialization();
            Marine marine = new Marine();
            Jimbey jimbey = new Jimbey();
            Fight fight = new Fight();
            init.creationEntity(marine, jimbey);
            // marine.DisplayDetails();
            // jimbey.DisplayDetails();
            fight.startCombat(jimbey, marine);



            World world = new World();
            Player player = new Player(1, 1, mapRows / 2, mapColumns / 2);

            while (true)
            {
                Map currentMap = world.GetMapAt(player.WorldX, player.WorldY);
                currentMap.PrintMap();
                ConsoleKeyInfo keyInfo = Console.ReadKey();

                int newLocalX = player.LocalX;
                int newLocalY = player.LocalY;

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
                if (currentMap.CanMoveTo(newLocalX, newLocalY))
                {
                    // Déplacer le joueur localement
                    currentMap.MovePlayer(player.LocalX, player.LocalY, newLocalX, newLocalY);
                    player.LocalX = newLocalX;
                    player.LocalY = newLocalY;
                }
                else
                {
                    // Sinon, vérifier si le joueur doit changer de carte
                    world.MovePlayerToNewMap(player);
                }

                // Déplace le joueur localement ou vers une nouvelle carte si nécessaire
                if (newLocalX < 0 || newLocalX >= mapRows || newLocalY < 0 || newLocalY >= mapColumns)
                {
                    world.MovePlayerToNewMap(player);
                }
                else if (currentMap.CanMoveTo(newLocalX, newLocalY))
                {
                    currentMap.MovePlayer(player.LocalX, player.LocalY, newLocalX, newLocalY);
                    player.LocalX = newLocalX;
                    player.LocalY = newLocalY;
                }
                currentMap = world.GetMapAt(player.WorldX, player.WorldY);
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
            Shop myShop = new Shop();
            myShop.Run();
        }
    }

}

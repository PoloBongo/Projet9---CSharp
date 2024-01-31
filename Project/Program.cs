using MapGame;
using Newtonsoft.Json;
using PlayerGame;
using System;
using System.Reflection.PortableExecutable;


public class Program {
    static void Main()
    {
        Fight fight = new Fight();
        Enemy enemy = new Enemy();
        Allies allies = new Allies();

        const int mapRows = 20;
        const int mapColumns = 20;

        World world = new World();
        Player player = new Player(1, 1, mapRows / 2, mapColumns / 2);


        string path = "../../../Entities/entity.json";
        enemy.CreateEntity(path);
        enemy.GetInfoEntity(path);
        allies.CreateEntity(path);
        allies.GetInfoEntity(path);

        while (true)
        {
            /* Clear la console ici pour pouvoir print en plus de la map */
            Console.Clear();

            fight.startCombat(allies.alliesContainer.Allies1, enemy.enemyContainer.Enemy1);

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
}
using MapGame;
using PlayerGame;
using System;

class Program
{
    static void Main()
    {
        const int rows = 20;
        const int columns = 20;

        Map gameMap = new Map(rows, columns);
        Player player = new Player(0, 0);

        while (true)
        {
            gameMap.PrintMap();
            ConsoleKeyInfo keyInfo = Console.ReadKey();

            int newX = player.X;
            int newY = player.Y;

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    newX = (newX - 1 + rows) % rows;
                    break;
                case ConsoleKey.DownArrow:
                    newX = (newX + 1) % rows;
                    break;
                case ConsoleKey.LeftArrow:
                    newY = (newY - 1 + columns) % columns;
                    break;
                case ConsoleKey.RightArrow:
                    newY = (newY + 1) % columns;
                    break;
            }

            gameMap.MovePlayer(player.X, player.Y, newX, newY);

            player.X = newX;
            player.Y = newY;
        }
    }
}

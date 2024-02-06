
using MapEntities;
using MapGame;

namespace MapGame
{
    public class Map
    {
        private ConsoleColor grassColor = ConsoleColor.Green;
        private ConsoleColor waterColor = ConsoleColor.Blue;
        private ConsoleColor playerColor = ConsoleColor.Magenta;
        private ConsoleColor enemyColor = ConsoleColor.Red;
        private ConsoleColor doorColor = ConsoleColor.Yellow;

        public char[,] matrix;
        private int rows;
        private int columns;


        public Map(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            matrix = new char[rows, columns];
        }

        public void InitializeCustomMap(char[,] layout)
        {
            // S'assurer que layout a la bonne taille
            if (layout.GetLength(0) != rows || layout.GetLength(1) != columns)
            {
                throw new ArgumentException("La taille du layout ne correspond pas aux dimensions de la carte.");
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    matrix[i, j] = layout[i, j];
                }
            }
        }

        public void PrintMap()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    ConsoleColor currentColor;
                    if (matrix[i, j] == '@') // Joueur
                    {
                        currentColor = playerColor;
                    }
                    else if (matrix[i, j] == '~') // Eau
                    {
                        currentColor = waterColor;
                    }
                    else if (matrix[i, j] == '.') // Herbe
                    {
                        currentColor = grassColor;
                    }
                    else if (matrix[i, j] == 'O') // Ennemi
                    {
                        currentColor = enemyColor;
                    }
                    else if (matrix[i, j] == ']' || matrix[i, j] == '[') // Portes
                    {
                        currentColor = doorColor;
                    }
                    else
                    {
                        currentColor = ConsoleColor.White;
                    }

                    Console.ForegroundColor = currentColor;
                    Console.Write(matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
            Console.ResetColor();
        }

        public bool IsGrass(int x, int y)
        {
            // Vérifier si les indices sont dans les limites de la matrice
            if (x >= 0 && x < rows && y >= 0 && y < columns)
            {
                return matrix[x, y] == '.';
            }
            else
            {
                return false;
            }
        }

        public bool IsWater(int x, int y)
        {
            return matrix[x, y] == '~';
        }

        public bool IsSand(int x, int y)
        {
            return matrix[x, y] == 'S';
        }

        public bool IsPlayer(int x, int y)
        {
            return matrix[x, y] == '@';
        }

        public bool IsEnemy(int x, int y)
        {
            return matrix[x, y] == 'O';
        }

        public bool CanMoveTo(int x, int y)
        {
            return IsGrass(x, y);
        }

        public void PlacePlayer(int x, int y)
        {
            if (CanMoveTo(x, y) && !IsPlayer(x, y))
            {
                ClearPlayerPosition(x, y);
                matrix[x, y] = '@';
            }
        }


        public void PlaceEnemy(int x, int y)
        {
            if (CanMoveTo(x, y) && matrix[x, y] != '@')
            {
                matrix[x, y] = 'O';
            }
        }

        public void ClearPlayerPosition(int x, int y)
        {
            if (matrix[x, y] == '@')
            {
                // Remettre de l'eau, de l'herbe ou du sable selon le cas
                if (IsWater(x, y))
                {
                    matrix[x, y] = '~';
                }
                else if (IsSand(x, y))
                {
                    matrix[x, y] = 'S';
                }
                else
                {
                    matrix[x, y] = '.';
                }
            }
        }



        public void ClearEnemyPosition(int x, int y)
        {
            if (matrix[x, y] == 'O')
            {
                matrix[x, y] = IsWater(x, y) ? '~' : '.'; // Remettre de l'eau ou de l'herbe selon le cas
            }
        }

        public void MovePlayer(int oldX, int oldY, int newX, int newY)
        {
            // Vérifier si la nouvelle position est sur de l'herbe et n'est pas occupée par le joueur
            if (IsGrass(newX, newY) && !IsPlayer(newX, newY))
            {
                ClearPlayerPosition(oldX, oldY);
                PlacePlayer(newX, newY);
            }
            else
            {
                // Reste sur la position actuelle en cas de collision ou si la nouvelle position est de l'eau
                PlacePlayer(oldX, oldY);
            }
        }
    }
}
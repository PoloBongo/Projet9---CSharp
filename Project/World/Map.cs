
using Project.Quest;

namespace MapGame
{
    public class Map
    {
        private ConsoleColor grassColor = ConsoleColor.Green;
        private ConsoleColor waterColor = ConsoleColor.Blue;
        private ConsoleColor playerColor = ConsoleColor.Magenta;
        private ConsoleColor enemyColor = ConsoleColor.Red;

        public char[,] matrix;
        private int rows;
        private int columns;

        public List<QuestNPC> questNPCs = new List<QuestNPC>();

        public Map(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            matrix = new char[rows, columns];
            CreateQuestNPCs();
        }

        public void CreateQuestNPCs()
        {
            questNPCs.Add(new QuestNPC(10, 10, "Tuer trois ennemis"));
            questNPCs.Add(new QuestNPC(2, 2, "Ramasser 5 morceaux de bois"));
        }

        public void DrawNPCs()
        {
            foreach (var npc in questNPCs)
            {
                matrix[npc.PositionX, npc.PositionY] = '?';
            }
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
            DrawNPCs();

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
                    else if (matrix[i, j] == '?') // NPC de quête
                    {
                        currentColor = ConsoleColor.Yellow;
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
                matrix[x, y] = IsWater(x, y) ? '~' : '.'; // Remettre de l'eau ou de l'herbe selon le cas
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
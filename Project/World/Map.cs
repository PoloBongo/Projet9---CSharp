using Project.Quest;
using Wood;
using static Project.Quest.QuestNPC;

namespace MapGame
{
    public class Map
    {
        private ConsoleColor grassColor = ConsoleColor.Green;
        private ConsoleColor waterColor = ConsoleColor.Blue;
        private ConsoleColor playerColor = ConsoleColor.Magenta;
        private ConsoleColor enemyColor = ConsoleColor.Red;
        private ConsoleColor doorColor = ConsoleColor.DarkYellow;

        public char[,] matrix;
        private int rows;
        private int columns;

        public List<QuestNPC> questNPCs = new List<QuestNPC>();
        public List<WoodPiece> WoodPieces { get; private set; }

        public Map(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            matrix = new char[rows, columns];
            CreateQuestNPCs();
            WoodPieces = new List<WoodPiece>();
            PlaceWoodPieces();
        }



         public bool IsWood(int x, int y)
    {
        if (x >= 0 && x < rows && y >= 0 && y < columns)
        {
            return matrix[x, y] == '!'; 
        }
        return false;
    }




        public void ClearWoodPiecePosition(int x, int y)
        {
            if (matrix[x, y] == '!')
            {
                matrix[x, y] = '.'; 

                // Supprimer le morceau de bois de la liste WoodPieces
                WoodPieces.RemoveAll(woodPiece => woodPiece.PositionX == x && woodPiece.PositionY == y);
            }
        }




        private void PlaceWoodPieces()
        {
            Random random = new Random();
            int numberOfWoodPieces = 5; 
            int attempts = 0;
            int maxAttempts = 100; 

            for (int i = 0; i < numberOfWoodPieces; i++)
            {
                int x, y;
                do
                {
                    x = random.Next(rows);
                    y = random.Next(columns);
                    attempts++;
                    if (attempts > maxAttempts)
                    {
                        return; 
                    }
                }
                while (IsWater(x, y) || IsEnemy(x, y));

                WoodPieces.Add(new WoodPiece(x, y));


            }
        }




        public void CreateQuestNPCs()
        {
            questNPCs.Add(new QuestNPC(10, 10, "Tuer trois ennemis", this)); 
            questNPCs.Add(new QuestNPC(15, 15, "Ramasser 5 morceaux de bois", this));
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
            DrawWoodPieces();

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    ConsoleColor currentColor = ConsoleColor.White;
                    switch (matrix[i, j])
                    {
                        case '@': currentColor = playerColor; break;
                        case '~': currentColor = waterColor; break;
                        case '.': currentColor = grassColor; break;
                        case 'O': currentColor = enemyColor; break;
                        case 'H': currentColor = ConsoleColor.Gray; break; // Murs des maisons
                        case 'F': currentColor = ConsoleColor.DarkGray; break; // Murs de la forteresse
                        case 'D':
                        case ']':
                        case '[':
                        case '―': currentColor = ConsoleColor.DarkYellow; break; // Portes
                        case 'S': currentColor = ConsoleColor.Yellow; break; // Sable
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
        public bool IsDoor(int x, int y)
        {
            return matrix[x, y] == ']' || matrix[x, y] == '[' || matrix[x, y] == '―';
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

        public void DrawWoodPieces()
        {
            foreach (var woodPiece in WoodPieces)
            {
                matrix[woodPiece.PositionX, woodPiece.PositionY] = '!';
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

        public bool IsNextToDoor(int x, int y)
        {
            // Vérifier les cases autour de la position du joueur
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (x + i >= 0 && x + i < rows && y + j >= 0 && y + j < columns)
                    {
                        if (IsDoor(x + i, y + j))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }


        public bool IsNextToFortressDoor(int x, int y)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (x + i >= 0 && x + i < rows && y + j >= 0 && y + j < columns)
                    {
                        if (matrix[x + i, y + j] == 'D')
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

    }
}
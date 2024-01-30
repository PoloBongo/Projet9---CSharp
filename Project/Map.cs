using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapGame
{
    public class Map
    {
        private ConsoleColor grassColor = ConsoleColor.Green;
        private ConsoleColor waterColor = ConsoleColor.Blue;
        private ConsoleColor playerColor = ConsoleColor.Red;

        private char[,] matrix;
        private int rows;
        private int columns;

        public Map(int rows, int columns)
        {
            this.rows = rows;
            this.columns = columns;
            matrix = new char[rows, columns];
            InitializeMap();
        }

        private void InitializeMap()
        {
            // Eau partout
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    matrix[i, j] = '~';
                }
            }

            // Herbe en bas
            for (int i = 15; i < 20; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    matrix[i, j] = '.';
                }
            }
        }

        public void PrintMap()
        {
            Console.Clear();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    ConsoleColor currentColor;
                    if (matrix[i, j] == '@')
                    {
                        currentColor = playerColor;
                    }
                    else if (matrix[i, j] == '~')
                    {
                        currentColor = waterColor;
                    }
                    else if (matrix[i, j] == '.')
                    {
                        currentColor = grassColor;
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
            return matrix[x, y] == '.';
        }

        public bool IsWater(int x, int y)
        {
            return matrix[x, y] == '~';
        }

        public bool IsPlayer(int x, int y)
        {
            return matrix[x, y] == '@';
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

        public void ClearPlayerPosition(int x, int y)
        {
            matrix[x, y] = IsWater(x, y) ? '~' : '.';
        }

        public void MovePlayer(int oldX, int oldY, int newX, int newY)
        {
            // Vérifier si la nouvelle position est sur de l'herbe et n'est pas occupée par le joueur
            if (IsGrass(newX, newY) && !IsPlayer(newX, newY))
            {
                ClearPlayerPosition(oldX, oldY);
                PlacePlayer(newX, newY);
            }
        }

        public char GetTileAtPosition(int x, int y)
        {
            return matrix[x, y];
        }

        public int GetRows()
        {
            return rows;
        }

        public int GetColumns()
        {
            return columns;
        }
    }

}

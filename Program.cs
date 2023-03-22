using System;
using System.IO;

namespace Pacman
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;

            Random random = new Random();

            bool isPlaying = true;
            bool isAlive = true;

            int firstGhostX;
            int firstGhostY;
            int firstGhostDX = 0;
            int firstGhostDY = 1;
            int secondGhostX;
            int secondGhostY;
            int secondGhostDX = 0;
            int secondGhostDY = -1;
            int pacmanX;
            int pacmanY;
            int pacmanDX = 0;
            int pacmanDY = 0;
            int allDots = 0;
            int collectDots = 0;

            char[,] map = ReadMap("Map1", out pacmanX, out pacmanY, out firstGhostX, out firstGhostY, out secondGhostX, out secondGhostY, ref allDots);

            DrawMap(map);

            while (isPlaying)
            {

                Console.SetCursorPosition(0, 35);
                Console.Write($"Собрано {collectDots}/{allDots}");

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    ChangeDirection(key, ref pacmanDX, ref pacmanDY);
                }

                if (map[pacmanX + pacmanDX, pacmanY + pacmanDY] != '#')
                {
                    CollectDots(map, pacmanX, pacmanY, ref collectDots);

                    Move(map, '@', ref pacmanX, ref pacmanY, pacmanDX, pacmanDY);

                }

                if (map[firstGhostX + firstGhostDX, firstGhostY + firstGhostDY] != '#')
                {
                    Move(map, '$', ref firstGhostX, ref firstGhostY, firstGhostDX, firstGhostDY);
                }
                else
                {
                    ChangeDirection(random, ref firstGhostDX, ref firstGhostDY);
                }

                if (map[secondGhostX + secondGhostDX, secondGhostY + secondGhostDY] != '#')
                {
                    Move(map, '&', ref secondGhostX, ref secondGhostY, secondGhostDX, secondGhostDY);
                }
                else
                {
                    ChangeDirection(random, ref secondGhostDX, ref secondGhostDY);
                }

                if ((firstGhostX == pacmanX && firstGhostY == pacmanY) || (secondGhostX == pacmanX && secondGhostY == pacmanY))
                {
                    isAlive = false;
                }

                System.Threading.Thread.Sleep(150);

                if (collectDots == allDots || !isAlive)
                {
                    isPlaying = false;
                }
            }

            Console.SetCursorPosition(0, 36);

            if (collectDots == allDots)
            {
                Console.WriteLine("Вы победили!");
            }
            else if (!isAlive)
            {
                Console.WriteLine("Вы проиграли, Вас съели!");
            }

            Console.ReadKey();
        }

        static void Move(char[,] map, char symbol, ref int X, ref int Y, int DX, int DY)
        {
            Console.SetCursorPosition(Y, X);
            Console.Write(map[X, Y]);

            X += DX;
            Y += DY;

            Console.SetCursorPosition(Y, X);
            Console.Write(symbol);
        }

        static void CollectDots(char[,] map, int pacmanX, int pacmanY, ref int collectDots)
        {
            if (map[pacmanX, pacmanY] == '.')
            {
                collectDots++;
                map[pacmanX, pacmanY] = ' ';
            }
        }

        static void ChangeDirection(ConsoleKeyInfo key, ref int DX, ref int DY)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    DX = -1;
                    DY = 0;
                    break;

                case ConsoleKey.DownArrow:
                    DX = 1;
                    DY = 0;
                    break;

                case ConsoleKey.LeftArrow:
                    DX = 0;
                    DY = -1;
                    break;

                case ConsoleKey.RightArrow:
                    DX = 0;
                    DY = 1;
                    break;
            }
        }
        static void ChangeDirection(Random random, ref int DX, ref int DY)
        {
            int GhostDir = random.Next(1, 5);

            switch (GhostDir)
            {
                case 1:
                    DX = -1;
                    DY = 0;
                    break;

                case 2:
                    DX = 1;
                    DY = 0;
                    break;

                case 3:
                    DX = 0;
                    DY = -1;
                    break;

                case 4:
                    DX = 0;
                    DY = 1;
                    break;
            }
        }

        static void DrawMap(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i, j]);
                }

                Console.WriteLine();
            }
        }

        static char[,] ReadMap(string mapNape, out int pacmanX, out int pacmanY, out int firstGhostX, out int firstGhostY, out int secondGhostX, out int secondGhostY, ref int allDots)
        {
            pacmanX = 0;
            pacmanY = 0;
            firstGhostX = 0;
            firstGhostY = 0;
            secondGhostX = 0;
            secondGhostY = 0;

            string[] newFile = File.ReadAllLines($"Maps/{mapNape}.txt");
            char[,] map = new char[newFile.Length, newFile[0].Length];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = newFile[i][j];

                    if ((map[i, j]) == '@')
                    {
                        pacmanX = i;
                        pacmanY = j;
                        map[i, j] = '.';
                    }
                    else if ((map[i, j]) == '$')
                    {
                        firstGhostX = i;
                        firstGhostY = j;
                        map[i, j] = '.';
                    }
                    else if ((map[i, j]) == '&')
                    {
                        secondGhostX = i;
                        secondGhostY = j;
                        map[i, j] = '.';
                    }
                    else if (map[i, j] == ' ')
                    {
                        map[i, j] = '.';
                        allDots++;
                    }
                }
            }

            return map;
        }
    }
}
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
            int pacmanDirectionX = 0;
            int pacmanDirectionY = 0;
            int allDots = 0;
            int collectDots = 0;

            char obstacle = '#';
            char player = '@';
            char reward = '.';
            char[,] map = ReadMap("Map1", out int pacmanPositionX, out int pacmanPositionY, ref allDots, player, reward);

            DrawMap(map);

            while (isPlaying)
            {
                ShowDotsInfo(collectDots, allDots);

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    MovePacman(key, out pacmanDirectionX, out pacmanDirectionY);
                }

                if (map[pacmanPositionX + pacmanDirectionX, pacmanPositionY + pacmanDirectionY] != obstacle)
                {
                    collectDots = CollectDots(map, pacmanPositionX, pacmanPositionY, collectDots, reward);

                    Move(map, player, ref pacmanPositionX, ref pacmanPositionY, pacmanDirectionX, pacmanDirectionY);

                }

                if (collectDots == allDots)
                {
                    isPlaying = false;
                }

                System.Threading.Thread.Sleep(150);
            }

            ShowGameResult(collectDots, allDots, isAlive);
        }

        static void ShowDotsInfo(int collectDots, int allDots)
        {
            Console.SetCursorPosition(30, 0);
            Console.Write($"Собрано {collectDots}/{allDots}");
        }

        static void ShowGameResult(int collectDots, int allDots, bool isAlive)
        {
            Console.SetCursorPosition(30, 1);

            if (collectDots == allDots)
            {
                Console.WriteLine("Вы победили!");
            }
            else if (isAlive == false)
            {
                Console.WriteLine("Вы проиграли, Вас съели!");
            }

            Console.ReadKey();
        }

        static void Move(char[,] map, char symbol, ref int numberСolumn, ref int numberLine, int directionX, int directionY)
        {
            Console.SetCursorPosition(numberLine, numberСolumn);
            Console.Write(map[numberСolumn, numberLine]);

            numberСolumn += directionX;
            numberLine += directionY;

            Console.SetCursorPosition(numberLine, numberСolumn);
            Console.Write(symbol);
        }

        static int CollectDots(char[,] map, int pacmanPositionX, int pacmanPositionY, int collectDots, char reward)
        {
            if (map[pacmanPositionX, pacmanPositionY] == reward)
            {
                collectDots++;
                map[pacmanPositionX, pacmanPositionY] = ' ';
            }

            return collectDots;
        }

        static void MovePacman(ConsoleKeyInfo key, out int directionX, out int directionY)
        {
            const ConsoleKey MoveUpCommand = ConsoleKey.UpArrow;
            const ConsoleKey MoveDownCommand = ConsoleKey.DownArrow;
            const ConsoleKey MoveLeftCommand = ConsoleKey.LeftArrow;
            const ConsoleKey MoveRightCommand = ConsoleKey.RightArrow;

            directionX = 0;
            directionY = 0;

            switch (key.Key)
            {
                case MoveUpCommand:
                    directionX = -1;
                    break;

                case MoveDownCommand:
                    directionX = 1;
                    break;

                case MoveLeftCommand:
                    directionY = -1;
                    break;

                case MoveRightCommand:
                    directionY = 1;
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

        static char[,] ReadMap(string mapNape, out int pacmanPositionX, out int pacmanPositionY, ref int allDots, char player, char reward)
        {
            pacmanPositionX = 0;
            pacmanPositionY = 0;

            string[] newFile = File.ReadAllLines($"Maps/{mapNape}.txt");
            char[,] map = new char[newFile.Length, newFile[0].Length];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = newFile[i][j];

                    if ((map[i, j]) == player)
                    {
                        pacmanPositionX = i;
                        pacmanPositionY = j;
                        allDots = DrawReward(map, i, j, allDots, reward);
                    }
                    else if (map[i, j] == ' ')
                    {
                        allDots = DrawReward(map, i, j, allDots, reward);
                    }
                }
            }

            return map;
        }

        static int DrawReward(char[,] map, int coordinateX, int coordinateY, int allDots, char reward)
        {
            map[coordinateX, coordinateY] = reward;
            allDots++;
            return allDots;
        }
    }
}
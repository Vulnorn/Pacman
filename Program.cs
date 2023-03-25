using System;
using System.ComponentModel;
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
            int firstGhostDirectionX = 0;
            int firstGhostDirectionY = 1;
            int secondGhostX;
            int secondGhostY;
            int secondGhostDirectionX = 0;
            int secondGhostDirectionY = -1;
            int pacmanX;
            int pacmanY;
            int pacmanDirectionX = 0;
            int pacmanDirectionY = 0;
            int allDots = 0;
            int collectDots = 0;
            char obstacle = '#';
            char player = '@';
            char firstOpponent = '$';
            char secondOpponent = '&';
            char reward = '.';
            char[,] map = ReadMap("Map1", out pacmanX, out pacmanY, out firstGhostX, out firstGhostY, out secondGhostX, out secondGhostY, ref allDots, player, firstOpponent, secondOpponent, reward);

            DrawMap(map);

            while (isPlaying)
            {

                Console.SetCursorPosition(0, 35);
                Console.Write($"Собрано {collectDots}/{allDots}");

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    ChangeDirection(key, ref pacmanDirectionX, ref pacmanDirectionY);
                }

                if (map[pacmanX + pacmanDirectionX, pacmanY + pacmanDirectionY] != obstacle)
                {
                    CollectDots(map, pacmanX, pacmanY, ref collectDots, reward);

                    Move(map, player, ref pacmanX, ref pacmanY, pacmanDirectionX, pacmanDirectionY);

                }

                if (map[firstGhostX + firstGhostDirectionX, firstGhostY + firstGhostDirectionY] != obstacle)
                {
                    Move(map, firstOpponent, ref firstGhostX, ref firstGhostY, firstGhostDirectionX, firstGhostDirectionY);
                }
                else
                {
                    ChangeDirection(random, ref firstGhostDirectionX, ref firstGhostDirectionY);
                }

                if (map[secondGhostX + secondGhostDirectionX, secondGhostY + secondGhostDirectionY] != obstacle)
                {
                    Move(map, secondOpponent, ref secondGhostX, ref secondGhostY, secondGhostDirectionX, secondGhostDirectionY);
                }
                else
                {
                    ChangeDirection(random, ref secondGhostDirectionX, ref secondGhostDirectionY);
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

        static void Move(char[,] map, char symbol, ref int  numberСolumn, ref int numberLine, int directionX, int directionY)
        {
            Console.SetCursorPosition(numberLine, numberСolumn);
            Console.Write(map[numberСolumn, numberLine]);

            numberСolumn += directionX;
            numberLine += directionY;

            Console.SetCursorPosition(numberLine, numberСolumn);
            Console.Write(symbol);
        }

        static void CollectDots(char[,] map, int pacmanX, int pacmanY, ref int collectDots,char reward)
        {
            if (map[pacmanX, pacmanY] == reward)
            {
                collectDots++;
                map[pacmanX, pacmanY] = ' ';
            }
        }

        static void ChangeDirection(ConsoleKeyInfo key, ref int directionX, ref int directionY)
        {
            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    directionX = -1;
                    directionY = 0;
                    break;

                case ConsoleKey.DownArrow:
                    directionX = 1;
                    directionY = 0;
                    break;

                case ConsoleKey.LeftArrow:
                    directionX = 0;
                    directionY = -1;
                    break;

                case ConsoleKey.RightArrow:
                    directionX = 0;
                    directionY = 1;
                    break;
            }
        }
        static void ChangeDirection(Random random, ref int directionX, ref int directionY)
        {
            int GhostDir = random.Next(1, 5);

            switch (GhostDir)
            {
                case 1:
                    directionX = -1;
                    directionY = 0;
                    break;

                case 2:
                    directionX = 1;
                    directionY = 0;
                    break;

                case 3:
                    directionX = 0;
                    directionY = -1;
                    break;

                case 4:
                    directionX = 0;
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

        static char[,] ReadMap(string mapNape, out int pacmanX, out int pacmanY, out int firstGhostX, out int firstGhostY, out int secondGhostX, out int secondGhostY, ref int allDots, char player, char firstOpponent, char secondOpponent,char reward)
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

                    if ((map[i, j]) == player)
                    {
                        pacmanX = i;
                        pacmanY = j;
                        map[i, j] = reward;
                    }
                    else if ((map[i, j]) == firstOpponent)
                    {
                        firstGhostX = i;
                        firstGhostY = j;
                        map[i, j] = reward;
                    }
                    else if ((map[i, j]) == secondOpponent)
                    {
                        secondGhostX = i;
                        secondGhostY = j;
                        map[i, j] = reward;
                    }
                    else if (map[i, j] == ' ')
                    {
                        map[i, j] = reward;
                        allDots++;
                    }
                }
            }

            return map;
        }
    }
}
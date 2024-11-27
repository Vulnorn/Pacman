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

            int firstGhostPositionX;
            int firstGhostPositionY;
            int firstGhostDirectionX = 0;
            int firstGhostDirectionY = 1;
            int secondGhostPositionX;
            int secondGhostPositionY;
            int secondGhostDirectionX = 0;
            int secondGhostDirectionY = -1;
            int pacmanPositionX;
            int pacmanPositionY;
            int pacmanDirectionX = 0;
            int pacmanDirectionY = 0;
            int allDots = 0;
            int collectDots = 0;

            char obstacle = '#';
            char player = '@';
            char firstOpponent = '$';
            char secondOpponent = '&';
            char reward = '.';
            char[,] map = ReadMap("Map1", out pacmanPositionX, out pacmanPositionY, out firstGhostPositionX, out firstGhostPositionY, out secondGhostPositionX, out secondGhostPositionY, ref allDots, player, firstOpponent, secondOpponent, reward);

            DrawMap(map);

            while (isPlaying)
            {
                Console.SetCursorPosition(30,0);
                Console.Write($"Собрано {collectDots}/{allDots}");

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    MovePacman(key, out pacmanDirectionX, out pacmanDirectionY);
                }

                if (map[pacmanPositionX + pacmanDirectionX, pacmanPositionY + pacmanDirectionY] != obstacle)
                {
                    CollectDots(map, pacmanPositionX, pacmanPositionY, collectDots, reward);

                    Move(map, player, ref pacmanPositionX, ref pacmanPositionY, pacmanDirectionX, pacmanDirectionY);

                }

                MoveGhost(random, map, ref firstGhostPositionX, ref firstGhostDirectionX, ref firstGhostPositionY, ref firstGhostDirectionY, firstOpponent, obstacle);
                MoveGhost(random, map, ref secondGhostPositionX, ref secondGhostDirectionX, ref secondGhostPositionY, ref secondGhostDirectionY, secondOpponent, obstacle);

                if ((firstGhostPositionX == pacmanPositionX && firstGhostPositionY == pacmanPositionY) || (secondGhostPositionX == pacmanPositionX && secondGhostPositionY == pacmanPositionY))
                {
                    isAlive = false;
                }

                System.Threading.Thread.Sleep(150);

                if (collectDots == allDots || isAlive == false)
                {
                    isPlaying = false;
                }
            }

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

        static void MoveGhost(Random random, char[,] map, ref int ghostPositionX, ref int ghostDirectionX, ref int ghostPositionY, ref int ghostDirectionY, char opponent, char obstacle)
        {
            if (map[ghostPositionX + ghostDirectionX, ghostPositionY + ghostDirectionY] != obstacle)
            {
                Move(map, opponent, ref ghostPositionX, ref ghostPositionY, ghostDirectionX, ghostDirectionY);
            }
            else
            {
                ChangeDirection(random, out ghostDirectionX, out ghostDirectionY);
            }
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

        static void ChangeDirection(Random random, out int directionX, out int directionY)
        {
            const int MoveLeftCommand = 1;
            const int MoveRightCommand = 2;
            const int MoveDownCommand = 3;
            const int MoveUpCommand = 4;

            directionX = 0;
            directionY=0;

            int GhostDirection = random.Next(MoveLeftCommand, MoveUpCommand + 1);

            switch (GhostDirection)
            {
                case MoveLeftCommand:
                     directionX = -1;
                    directionY = 0;
                    break;

                case MoveRightCommand:
                    directionX = 1;
                    directionY = 0;
                    break;

                case MoveDownCommand:
                    directionX = 0;
                    directionY = -1;
                    break;

                case MoveUpCommand:
                    directionX = 0;
                    directionY = 1;
                    break;
            }

            return;
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

        static char[,] ReadMap(string mapNape, out int pacmanPositionX, out int pacmanPositionY, out int firstGhostPositionX, out int firstGhostPositionY, out int secondGhostPositionX, out int secondGhostPositionY, ref int allDots, char player, char firstOpponent, char secondOpponent, char reward)
        {
            pacmanPositionX = 0;
            pacmanPositionY = 0;
            firstGhostPositionX = 0;
            firstGhostPositionY = 0;
            secondGhostPositionX = 0;
            secondGhostPositionY = 0;

            string[] newFile = File.ReadAllLines($"Maps/{mapNape}.txt");
            char[,] map = new char[newFile.Length, newFile[0].Length];

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = newFile[i][j];
                    
                    if ((map[i, j]) == player)
                    {
                        DrouElementMaps(ref map, i, j, ref allDots, reward, out pacmanPositionX, out pacmanPositionY);
                    }
                    else if ((map[i, j]) == firstOpponent)
                    {
                        DrouElementMaps(ref map, i, j, ref allDots, reward, out firstGhostPositionX, out firstGhostPositionY);
                    }
                    else if ((map[i, j]) == secondOpponent)
                    {
                        DrouElementMaps(ref map, i, j,  ref allDots, reward,out secondGhostPositionX, out secondGhostPositionY);
                    }
                    else if (map[i, j] == ' ')
                    {
                        DrouReward(ref map, i, j, ref allDots, reward);
                    }
                }
            }

            return map;
        }

        static void DrouElementMaps(ref char[,] map,int coordinateX, int coordinateY, ref int allDots, char reward,out int positionX, out int positionY)
        {
            positionX = coordinateX;
            positionY = coordinateY;
            DrouReward(ref map, coordinateX, coordinateY, ref allDots, reward);
        }

        static void DrouReward(ref char[,] map, int coordinateX, int coordinateY, ref int allDots, char reward)
        {
            map[coordinateX, coordinateY] = reward;
            allDots++;
        }
    }
}
using System;
using System.Diagnostics;
using System.Threading;

class Program
{
    static Stopwatch stopwatch = new Stopwatch();
    static int selectedOption = 0;
    static string[] menuOptions = { "New Game", "Exit" };
    static int level = 1;

    static void Main(string[] args)
    {
        Console.CursorVisible = false;

        while (true)
        {
            Console.Clear();
            DrawMenu();

            ConsoleKeyInfo keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.UpArrow)
            {
                selectedOption = (selectedOption - 1 + menuOptions.Length) % menuOptions.Length;
            }
            else if (keyInfo.Key == ConsoleKey.DownArrow)
            {
                selectedOption = (selectedOption + 1) % menuOptions.Length;
            }
            else if (keyInfo.Key == ConsoleKey.Enter)
            {
                if (selectedOption == 0)
                {
                    StartGame();
                }
                else if (selectedOption == 1)
                {
                    Environment.Exit(0);
                }
            }
        }
    }

    static void DrawMenu()
    {
        int centerX = Console.WindowWidth / 2;
        int centerY = Console.WindowHeight / 2;

        Console.SetCursorPosition(centerX, centerY - (menuOptions.Length / 2));
        Console.WriteLine("Select an option:");
        for (int i = 0; i < menuOptions.Length; i++)
        {
            if (i == selectedOption)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Blue;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;
            }
            Console.SetCursorPosition(centerX - (menuOptions[i].Length / 2), Console.CursorTop + 1);
            Console.WriteLine(menuOptions[i]);
        }
    }

    static void StartGame()
    {
        stopwatch.Restart();
        Thread timerThread = new Thread(UpdateTime);
        timerThread.Start();

        char[,] maze =
        {
            {'#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'},
            {'#', ' ', ' ', ' ', '#', ' ', ' ', ' ', ' ', ' ', '#'},
            {'#', ' ', '#', ' ', '#', ' ', '#', '#', '#', '#', '#'},
            {'#', ' ', '#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#'},
            {'#', ' ', '#', '#', '#', '#', '#', '#', '#', ' ', '#'},
            {'#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#', ' ', '#'},
            {'#', '#', '#', '#', '#', '#', '#', '#', '#', ' ', '#'},
            {'#', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '#'},
            {'#', '#', '#', '#', '#', '#', '#', '#', '#', '#', '#'}
        };

        int playerX = 1;
        int playerY = 1;
        int exitX = 9;
        int exitY = 7;

        while (true)
        {
            Console.Clear();
            DrawHeader();
            DrawMaze(maze, playerX, playerY, exitX, exitY);

            ConsoleKeyInfo keyInfo = Console.ReadKey(true);
            HandleInput(keyInfo, maze, ref playerX, ref playerY, exitX, exitY);

            if (playerX == exitX && playerY == exitY)
            {
                stopwatch.Stop();
                Console.SetCursorPosition(0, maze.GetLength(0) + 3);
                Console.WriteLine($"Congratulations! You completed Level {level} in {stopwatch.Elapsed:mm\\:ss}.");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                level++;
                break;
            }
        }
    }

    static void DrawHeader()
    {
        int centerX = Console.WindowWidth / 2;
        Console.SetCursorPosition(centerX - 5, 0);
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"Level {level}");

        Console.SetCursorPosition(0, 2);
        Console.WriteLine($"Time: {stopwatch.Elapsed:mm\\:ss}");
    }

    static void DrawMaze(char[,] maze, int playerX, int playerY, int exitX, int exitY)
    {
        int mazeWidth = maze.GetLength(1);
        int mazeHeight = maze.GetLength(0);

        int centerX = (Console.WindowWidth - mazeWidth) / 2;
        int centerY = (Console.WindowHeight - mazeHeight) / 2;

        for (int y = 0; y < mazeHeight; y++)
        {
            Console.SetCursorPosition(centerX, centerY + y);
            for (int x = 0; x < mazeWidth; x++)
            {
                if (x == playerX && y == playerY)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("@");
                }
                else if (x == exitX && y == exitY)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("E");
                }
                else if (maze[y, x] == '#')
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write("#");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.Write(" ");
                }
            }
        }
    }

    static void HandleInput(ConsoleKeyInfo keyInfo, char[,] maze, ref int playerX, ref int playerY, int exitX, int exitY)
    {
        switch (keyInfo.Key)
        {
            case ConsoleKey.UpArrow:
                if (maze[playerY - 1, playerX] != '#')
                {
                    playerY--;
                }
                break;
            case ConsoleKey.DownArrow:
                if (maze[playerY + 1, playerX] != '#')
                {
                    playerY++;
                }
                break;
            case ConsoleKey.LeftArrow:
                if (maze[playerY, playerX - 1] != '#')
                {
                    playerX--;
                }
                break;
            case ConsoleKey.RightArrow:
                if (maze[playerY, playerX + 1] != '#')
                {
                    playerX++;
                }
                break;
        }
    }

    static void UpdateTime()
    {
        while (true)
        {
            Thread.Sleep(1000);
            DrawHeader();
        }
    }
}

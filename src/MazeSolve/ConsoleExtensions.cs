public static class ConsoleExtensions
{
    public static void DisplayUnsolved(this Maze maze)
    {
        Console.Clear();
        for (int i = 0; i < maze.MazeHeight; i++)
        {
            for (int j = 0; j < maze.MazeWidth; j++)
            {
                if (maze.Grid[i, j].IsClosedWall())
                {
                    Console.Write("▓▓▓");
                }
                else
                {
                    Console.Write("   ");
                }
            }
            Console.WriteLine();
        }
    }

    public static void DisplayPoint(this Maze maze, PathPoint point)
    {
        for (int i = 0; i < maze.MazeHeight; i++)
        {
            for (int j = 0; j < maze.MazeWidth; j++)
            {
                if (maze.Grid[i, j].IsClosedWall())
                {
                    Console.Write("▓▓▓");
                }
                else
                {
                    var isInPath = point.X == i && point.Y == j;
                    var displaySymbol = isInPath ? " □ " : "   ";
                    Console.Write(displaySymbol);
                }
            }
            Console.WriteLine();
        }
    }

    public static void DisplaySolving(this Maze maze, MazePath path)
    {
        Console.Clear();
        Console.CursorVisible = false;
        foreach (var point in path.Points)
        {
            Console.SetCursorPosition(0, 0);
            maze.DisplayPoint(point);
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
        }
        Console.CursorVisible = true;
    }

    public static void DisplayFrameByFrame(this Maze maze, MazePath path)
    {
        Console.Clear();
        Console.CursorVisible = false;
        foreach (var point in path.Points)
        {
            Console.SetCursorPosition(0, 0);
            maze.DisplayPoint(point);
            Console.WriteLine($"({point.X}, {point.Y}) {(point.IsCell ? "cell" : "wall")} - press any key for the next frame...");
            Console.ReadKey();
        }
        Console.CursorVisible = true;
    }

    public static void DisplayPathHighlight(this Maze maze, MazePath path)
    {
        for (int i = 0; i < maze.MazeHeight; i++)
        {
            for (int j = 0; j < maze.MazeWidth; j++)
            {
                if (maze.Grid[i, j].IsClosedWall())
                {
                    Console.Write("▓▓▓");
                }
                else
                {
                    var isInPath = path.Points.Any(point => point.X == i && point.Y == j);
                    var displaySymbol = isInPath ? " □ " : "   ";
                    Console.Write(displaySymbol);
                }
            }
            Console.WriteLine();
        }
    }
}
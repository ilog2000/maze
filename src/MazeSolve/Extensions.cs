public static class Extensions {
    public static bool IsEven(this int value) {
        return value % 2 == 0;
    }

    public static List<string> ToPairs(this string text) {
        var pairs = new List<string>();
        for (int i = 0; i < text.Length; i += 2)
            pairs.Add(text.Substring(i, 2));
        return pairs;
    }

    public static bool IsClosedWall(this MazeItem item) {
        return item is Wall && !((Wall)item).Open;
    }

    public static bool MatchEnd(this Maze maze, MazeItem item) {
        return item.MazeX == maze.MazeEndX && item.MazeY == maze.MazeEndY;
    }

    public static int DistanceToEnd(this Maze maze, MazeItem item) {
        var dx = Math.Abs(maze.MazeEndX - item.MazeX);
        var dy = Math.Abs(maze.MazeEndY - item.MazeY);
        return dx + dy;
    }

    public static void DisplayUnsolved(this Maze maze) {
        Console.OutputEncoding = System.Text.Encoding.Unicode;
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

    public static void DisplayNode(this Maze maze, PathNode node) {
        Console.OutputEncoding = System.Text.Encoding.Unicode;

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
                    var isInPath = node.X == i && node.Y == j;
                    var displaySymbol = isInPath ? " □ " : "   ";
                    Console.Write(displaySymbol);
                }
            }
            Console.WriteLine();
        }
    }

    public static void DisplaySolving(this Maze maze, MazePath path) {
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        Console.Clear();
        Console.CursorVisible = false;
        foreach (var node in path.Nodes)
        {
            Console.SetCursorPosition(0, 0);
            maze.DisplayNode(node);
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
        }
        Console.CursorVisible = true;
    }

    public static void DisplayFrameByFrame(this Maze maze, MazePath path) {
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        Console.Clear();
        Console.CursorVisible = false;
        foreach (var node in path.Nodes)
        {
            Console.SetCursorPosition(0, 0);
            maze.DisplayNode(node);
            Console.WriteLine($"({node.X}, {node.Y}) {(node.IsCell ? "cell" : "wall")} - press any key for the next frame...");
            Console.ReadKey();
        }
        Console.CursorVisible = true;
    }

    public static void DisplayPathHighlight(this Maze maze, MazePath path) {
        Console.OutputEncoding = System.Text.Encoding.Unicode;

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
                    var isInPath = path.Nodes.Any(node => node.X == i && node.Y == j);
                    var displaySymbol = isInPath ? " □ " : "   ";
                    Console.Write(displaySymbol);
                }
            }
            Console.WriteLine();
        }
    }
}
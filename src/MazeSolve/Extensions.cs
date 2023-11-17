public static class Extensions
{
    public static bool IsEven(this int value)
    {
        return value % 2 == 0;
    }

    public static List<string> ToPairs(this string text)
    {
        var pairs = new List<string>();
        for (int i = 0; i < text.Length; i += 2)
            pairs.Add(text.Substring(i, 2));
        return pairs;
    }

    public static bool IsClosedWall(this MazeItem item)
    {
        return item is Wall && !((Wall)item).Open;
    }

    public static bool MatchEnd(this Maze maze, MazeItem item)
    {
        return item.MazeX == maze.MazeEndX && item.MazeY == maze.MazeEndY;
    }

    public static int DistanceToEnd(this Maze maze, MazeItem item)
    {
        var dx = Math.Abs(maze.MazeEndX - item.MazeX);
        var dy = Math.Abs(maze.MazeEndY - item.MazeY);
        return dx + dy;
    }

    public static bool IsInPath(this MazeItem item, MazePath path)
    {
        return path.Points.Any(p => p.X == item.MazeX && p.Y == item.MazeY);
    }
}
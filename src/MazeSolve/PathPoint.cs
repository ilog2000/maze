public class PathPoint
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public bool IsCell { get; set; }

    public PathPoint(MazeItem item)
    {
        X = item.MazeX;
        Y = item.MazeY;
        IsCell = item is Cell;
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}

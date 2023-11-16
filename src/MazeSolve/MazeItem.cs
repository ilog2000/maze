
public class MazeItem
{
    public int MazeX, MazeY;
    public bool Visited = false;


    public MazeItem(int x, int y)
    {
        MazeX = x;
        MazeY = y;
    }

    public override string ToString()
    {
        return $"({MazeX}, {MazeY})";
    }
}
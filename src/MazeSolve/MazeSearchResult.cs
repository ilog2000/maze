public class MazeSearchResult
{
    public bool solved { get; set; }
    public long ElapsedMilliseconds { get; set; }
    public MazePath Path { get; set; }
}
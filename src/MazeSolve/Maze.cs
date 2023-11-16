using System.Diagnostics;

public class Maze
{
    private int cellHeight, cellWidth;
    private int mazeHeight, mazeWidth;
    private MazeItem[,] grid;

    public int CellHeight { get { return cellHeight; } }
    public int CellWidth { get { return cellWidth; } }
    public int MazeHeight { get { return mazeHeight; } }
    public int MazeWidth { get { return mazeWidth; } }
    public MazeItem[,] Grid { get { return grid; } }
    public int MazeStartX = 0;
    public int MazeStartY = 1;
    public int MazeEndX { get { return mazeHeight - 1; } }
    public int MazeEndY { get { return mazeWidth - 2; } }

    public Maze(int cellHeight, int cellWidth)
    {
        this.cellHeight = cellHeight;
        this.cellWidth = cellWidth;
        this.mazeHeight = cellHeight * 2 + 1;
        this.mazeWidth = cellWidth * 2 + 1;

        grid = new MazeItem[mazeHeight, mazeWidth];

        for (int i = 0; i < mazeHeight; i++)
            for (int j = 0; j < mazeWidth; j++)
                if (i.IsEven())
                {
                    grid[i, j] = new Wall(i, j);
                }
                else
                {
                    if (j.IsEven())
                    {
                        grid[i, j] = new Wall(i, j);
                    }
                    else
                    {
                        grid[i, j] = new Cell(i, j);
                    }
                }
    }

    public static Maze Parse(string text)
    {
        if (string.IsNullOrEmpty(text))
        {
            throw new Exception("Invalid maze text");
        }

        string[] lines = text.Split(Environment.NewLine);
        var mazeHeight = lines.Length;
        var mazeWidth = lines[0].Length / 2; // 2 chars per cell

        if (mazeHeight.IsEven() || mazeWidth.IsEven() || lines[0].Length % 2 != 0)
        {
            throw new Exception("Invalid maze dimensions");
        }

        var cellHeight = (mazeHeight - 1) / 2;
        var cellWidth = (mazeWidth - 1) / 2;

        var maze = new Maze(cellHeight, cellWidth);
        var grid = maze.Grid;

        for (int i = 0; i < mazeHeight; i++)
        {
            var pairs = lines[i].ToPairs();
            for (int j = 0; j < mazeWidth; j++)
            {
                if (pairs[j] == "[]")
                {
                    grid[i, j] = new Wall(i, j);
                }
                else
                {
                    if (i.IsEven() || j.IsEven())
                    {
                        grid[i, j] = new Wall(i, j) { Open = true };
                    }
                    else
                    {
                        grid[i, j] = new Cell(i, j);
                    }
                }
            }
        }
        return maze;
    }

    public MazeSearchResult SolveDFS()
    {
        var stopwatch = Stopwatch.StartNew();
        var solved = false;
        var path = new MazePath();

        var stack = new Stack<MazeItem>();
        var current = grid[MazeStartX, MazeStartY];
        current.Visited = true;
        stack.Push(current);

        while (stack.Count > 0)
        {
            current = stack.Pop();
            path.Nodes.Add(new PathNode(current));
            if (this.MatchEnd(current))
            {
                solved = true;
                break;
            }
            var neighbors = GetUnvisitedNeighbors(current);
            if (neighbors.Count > 0)
            {
                stack.Push(current);
                // Original:
                // var next = neighbors[new Random().Next(neighbors.Count)];
                // Optimization - try to select a node that is the most close to the end:
                var next = neighbors.OrderBy(node => this.DistanceToEnd(node)).First();
                next.Visited = true;
                stack.Push(next);
            }
        }

        stopwatch.Stop();
        return new MazeSearchResult
        {
            solved = solved,
            ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
            Path = path
        };
    }

    public MazeSearchResult SolveBFS()
    {
        var stopwatch = Stopwatch.StartNew();
        var solved = false;
        var path = new MazePath();

        var queue = new Queue<MazeItem>();
        var current = grid[MazeStartX, MazeStartY];
        current.Visited = true;
        queue.Enqueue(current);

        while (queue.Count > 0)
        {
            current = queue.Dequeue();
            path.Nodes.Add(new PathNode(current));
            if (this.MatchEnd(current))
            {
                solved = true;
                break;
            }
            var neighbors = GetUnvisitedNeighbors(current);
            if (neighbors.Count > 0)
            {
                foreach (var neighbor in neighbors)
                {
                    neighbor.Visited = true;
                    queue.Enqueue(neighbor);
                }
            }
        }

        stopwatch.Stop();
        return new MazeSearchResult
        {
            solved = solved,
            ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
            Path = path
        };
    }

    public MazeSearchResult SolveDijkstra()
    {
        var stopwatch = Stopwatch.StartNew();
        var solved = false;
        var path = new MazePath();

        var queue = new PriorityQueue<MazeItem, int>();
        var current = grid[MazeStartX, MazeStartY];
        current.Visited = true;
        queue.Enqueue(current, 0);

        while (queue.Count > 0)
        {
            current = queue.Dequeue();
            path.Nodes.Add(new PathNode(current));
            if (this.MatchEnd(current))
            {
                solved = true;
                break;
            }
            var neighbors = GetUnvisitedNeighbors(current);
            if (neighbors.Count > 0)
            {
                foreach (var neighbor in neighbors)
                {
                    neighbor.Visited = true;
                    queue.Enqueue(neighbor, this.DistanceToEnd(neighbor));
                }
            }
        }

        stopwatch.Stop();
        return new MazeSearchResult
        {
            solved = solved,
            ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
            Path = path
        };
    }

    public MazeSearchResult SolveAStar()
    {
        var stopwatch = Stopwatch.StartNew();
        var solved = false;
        var path = new MazePath();

        var openSet = new HashSet<MazeItem>();
        var queue = new PriorityQueue<MazeItem, int>();
        var gScore = new Dictionary<MazeItem, int>();
        var fScore = new Dictionary<MazeItem, int>();

        var start = grid[MazeStartX, MazeStartY];
        var end = grid[MazeEndX, MazeEndY];

        gScore[start] = 0;
        fScore[start] = this.DistanceToEnd(start);
        queue.Enqueue(start, fScore[start]);
        openSet.Add(start);

        while (openSet.Count > 0)
        {
            var current = queue.Dequeue();
            openSet.Remove(current);
            path.Nodes.Add(new PathNode(current));
            if (current == end)
            {
                solved = true;
                break;
            }

            foreach (var neighbor in GetUnvisitedNeighbors(current))
            {
                var tentativeGScore = gScore[current] + 1;
                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + this.DistanceToEnd(neighbor);
                    if (!openSet.Contains(neighbor))
                    {
                        queue.Enqueue(neighbor, fScore[neighbor]);
                        openSet.Add(neighbor);
                    }
                }
            }
        }

        stopwatch.Stop();
        return new MazeSearchResult
        {
            solved = solved,
            ElapsedMilliseconds = stopwatch.ElapsedMilliseconds,
            Path = path
        };
    }

    private List<MazeItem> GetUnvisitedNeighbors(MazeItem current)
    {
        var neighbors = new List<MazeItem>();

        var x = current.MazeX;
        var y = current.MazeY;

        if (x > 0 && !grid[x - 1, y].IsClosedWall() && !grid[x - 1, y].Visited)
        {
            neighbors.Add(grid[x - 1, y]);
        }
        if (x < mazeHeight - 1 && !grid[x + 1, y].IsClosedWall() && !grid[x + 1, y].Visited)
        {
            neighbors.Add(grid[x + 1, y]);
        }
        if (y > 0 && !grid[x, y - 1].IsClosedWall() && !grid[x, y - 1].Visited)
        {
            neighbors.Add(grid[x, y - 1]);
        }
        if (y < mazeWidth - 1 && !grid[x, y + 1].IsClosedWall() && !grid[x, y + 1].Visited)
        {
            neighbors.Add(grid[x, y + 1]);
        }

        return neighbors;
    }
}

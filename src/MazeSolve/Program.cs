ConsoleUI.Init();

var file = Environment.GetCommandLineArgs().Skip(1).FirstOrDefault();
if (string.IsNullOrEmpty(file))
{
    file = ConsoleUI.AskForString("Enter sample file path");
}

if (!File.Exists(file))
{
    Console.WriteLine("Could not find sample file. Nothing to solve, exiting...");
    Environment.Exit(1);
}

var text = File.ReadAllText(file);
var maze = Maze.Parse(text);

if (ConsoleUI.AskYN("Display unsolved maze?"))
{
    maze.DisplayUnsolved();
}

MazeSearchResult result = null;

if (ConsoleUI.AskYN("Solve using DFS?"))
{
    result = maze.SolveDFS();
    maze.DisplayPathHighlight(result.Path);
    Console.WriteLine($"Solved: {result.Path.Solved}, steps: {result.Path.Points.Count}, elapsed: {result.ElapsedMilliseconds} ms");
}
else if (ConsoleUI.AskYN("Solve using BFS?"))
{
    result = maze.SolveBFS();
    maze.DisplayPathHighlight(result.Path);
    Console.WriteLine($"Solved: {result.Path.Solved}, steps: {result.Path.Points.Count}, elapsed: {result.ElapsedMilliseconds} ms");
}
else if (ConsoleUI.AskYN("Solve using Dijkstra?"))
{
    result = maze.SolveDijkstra();
    maze.DisplayPathHighlight(result.Path);
    Console.WriteLine($"Solved: {result.Path.Solved}, steps: {result.Path.Points.Count}, elapsed: {result.ElapsedMilliseconds} ms");
}
else if (ConsoleUI.AskYN("Solve using A*?"))
{
    result = maze.SolveAStar();
    maze.DisplayPathHighlight(result.Path);
    Console.WriteLine($"Solved: {result.Path.Solved}, steps: {result.Path.Points.Count}, elapsed: {result.ElapsedMilliseconds} ms");
}

if (result != null && ConsoleUI.AskYN("Display solving process?"))
{
    maze.DisplaySolving(result.Path);
}
else if (result != null && ConsoleUI.AskYN("Display frame by frame?"))
{
    maze.DisplayFrameByFrame(result.Path);
}

if (ConsoleUI.AskYN("Try to find alternative solutions?"))
{
    var paths = new List<MazePath>();
    var newMaze = Maze.Parse(text);
    var nextResult = newMaze.TrySolveAltDFS(paths);
    int MAX_ATTEMPTS = 10;
    var attempt = 0;

    while (attempt < MAX_ATTEMPTS)
    {
        attempt++;
        if (!paths.Any(p => p.Equals(nextResult.Path)))
        {
            paths.Add(nextResult.Path);
        }
        newMaze = Maze.Parse(text);
        nextResult = newMaze.TrySolveAltDFS(paths);
    }
    if (paths.Count == 0)
    {
        Console.WriteLine("No alternative paths found.");
    }
    else
    {
        var orderedPaths = paths.OrderBy(p => p.Points.Count).ToList();
        foreach (var path in orderedPaths)
        {
            newMaze.DisplayPathHighlight(path);
            Console.WriteLine($"Solved: {path.Solved}, steps: {path.Points.Count}");
            Console.WriteLine();
        }
    }
}

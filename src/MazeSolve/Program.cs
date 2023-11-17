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

MazePath mazePath = null;

if (ConsoleUI.AskYN("Solve using DFS?"))
{
    mazePath = maze.SolveDFS();
    maze.DisplayPathHighlight(mazePath);
    mazePath.DisplayPathInfo();
}
else if (ConsoleUI.AskYN("Solve using BFS?"))
{
    mazePath = maze.SolveBFS();
    maze.DisplayPathHighlight(mazePath);
    mazePath.DisplayPathInfo();
}
else if (ConsoleUI.AskYN("Solve using Dijkstra?"))
{
    mazePath = maze.SolveDijkstra();
    maze.DisplayPathHighlight(mazePath);
    mazePath.DisplayPathInfo();
}
else if (ConsoleUI.AskYN("Solve using A*?"))
{
    mazePath = maze.SolveAStar();
    maze.DisplayPathHighlight(mazePath);
    mazePath.DisplayPathInfo();
}

if (mazePath != null && ConsoleUI.AskYN("Display solving process?"))
{
    maze.DisplaySolving(mazePath);
}
else if (mazePath != null && ConsoleUI.AskYN("Display frame by frame?"))
{
    maze.DisplayFrameByFrame(mazePath);
}

if (ConsoleUI.AskYN("Try to find alternative solutions?"))
{
    var paths = new List<MazePath>();
    var newMaze = Maze.Parse(text);
    var nextPath = newMaze.TrySolveAltDFS(paths);
    int MAX_ATTEMPTS = 10;
    var attempt = 0;

    while (attempt < MAX_ATTEMPTS)
    {
        attempt++;
        if (!paths.Any(p => p.Equals(nextPath)))
        {
            paths.Add(nextPath);
        }
        newMaze = Maze.Parse(text);
        nextPath = newMaze.TrySolveAltDFS(paths);
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
            path.DisplayPathInfo();
            Console.WriteLine();
        }
    }
}

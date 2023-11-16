var askForString = (Func<string, string>)((string enquiry) =>
{
    Console.Write($"{enquiry}: ");
    return Console.ReadLine();
});

var askYN = (Func<string, bool>)((string question) =>
{
    Console.Write($"{question} (y/n): ");
    var key = Console.ReadKey();
    Console.WriteLine();
    return key.KeyChar == 'y' || key.KeyChar == 'Y';
});

var file = Environment.GetCommandLineArgs().Skip(1).FirstOrDefault();
if (string.IsNullOrEmpty(file))
{
    file = askForString("Enter sample file path");
}

if (!File.Exists(file))
{   
    Console.WriteLine("Could not find sample file. Nothing to solve, exiting...");
    Environment.Exit(1);
}

var text = File.ReadAllText(file);

var maze = Maze.Parse(text);
if (askYN("Display unsolved maze?"))
{
    maze.DisplayUnsolved();
}

MazeSearchResult result = null;
if (askYN("Solve using DFS?"))
{
    result = maze.SolveDFS();
    maze.DisplayPathHighlight(result.Path);
    Console.WriteLine($"Solved: {result.solved}, steps: {result.Path.Nodes.Count}, elapsed: {result.ElapsedMilliseconds} ms");
}
else if (askYN("Solve using BFS?"))
{
    result = maze.SolveBFS();
    maze.DisplayPathHighlight(result.Path);
    Console.WriteLine($"Solved: {result.solved}, steps: {result.Path.Nodes.Count}, elapsed: {result.ElapsedMilliseconds} ms");
}
else if (askYN("Solve using Dijkstra?"))
{
    result = maze.SolveDijkstra();
    maze.DisplayPathHighlight(result.Path);
    Console.WriteLine($"Solved: {result.solved}, steps: {result.Path.Nodes.Count}, elapsed: {result.ElapsedMilliseconds} ms");
}
else if (askYN("Solve using A*?"))
{
    result = maze.SolveAStar();
    maze.DisplayPathHighlight(result.Path);
    Console.WriteLine($"Solved: {result.solved}, steps: {result.Path.Nodes.Count}, elapsed: {result.ElapsedMilliseconds} ms");
}

if (result != null && askYN("Display solving process?"))
{
    maze.DisplaySolving(result.Path);
}
else if (result != null && askYN("Display frame by frame?"))
{
    maze.DisplayFrameByFrame(result.Path);
}

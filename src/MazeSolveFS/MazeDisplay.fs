[<AutoOpen>]
module MazeDisplay

open System
open MazeTypes

let displayUnsolved (maze: Maze) =
  Console.Clear()
  for i in 0 .. maze.Height - 1 do
    for j in 0 .. maze.Width - 1 do
      if maze.Grid[i, j].isClosedWall then
        Console.Write("▓▓▓")
      else
        Console.Write("   ")
    Console.WriteLine()

let displayItem (maze: Maze) (item: MazeItem)  =
  Console.Clear()
  for i in 0 .. maze.Height - 1 do
    for j in 0 .. maze.Width - 1 do
      let current = maze.Grid[i, j]
      if current.isClosedWall then
        Console.Write("▓▓▓")
      else
        let displaySymbol = if current = item then " □ "  else "   "
        Console.Write(displaySymbol)
    Console.WriteLine()

let displayPathHighlighted (maze: Maze) (path: MazePath) =
  Console.Clear()
  for i in 0 .. maze.Height - 1 do
    for j in 0 .. maze.Width - 1 do
      let current = maze.Grid[i, j]
      if current.isClosedWall then
        Console.Write("▓▓▓")
      else
        let matched = path.isInPath current
        let displaySymbol = if matched then " □ "  else "   "
        Console.Write(displaySymbol)
    Console.WriteLine()

let displaySolving (maze: Maze) (path: MazePath) =
  Console.Clear()
  Console.CursorVisible <- false
  for i in 0 .. path.Items.Length - 1 do
    let item = path.Items[i]
    displayItem maze item
    System.Threading.Thread.Sleep(100)
  Console.CursorVisible <- true

let displayFrameByFrame (maze: Maze) (path: MazePath) =
  Console.Clear()
  Console.CursorVisible <- false
  for i in 0 .. path.Items.Length - 1 do
    let item = path.Items[i]
    displayItem maze item
    Console.WriteLine($"({item.X}, {item.Y}) - press any key for the next frame...")
    Console.ReadKey() |> ignore
  Console.CursorVisible <- true

let displayPathInfo (path: MazePath) =
  let elapsed =
    if path.ElapsedMilliseconds.IsSome then
      $"{path.ElapsedMilliseconds.Value} ms"
    else
      "not measured"
  Console.WriteLine($"Solved: {path.Solved}, steps: {path.Items |> List.length}, elapsed: {elapsed}")

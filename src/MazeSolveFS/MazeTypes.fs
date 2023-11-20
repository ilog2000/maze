namespace MazeTypes

open System.Diagnostics

type Cell = { X: int; Y: int }
type Wall = { X: int; Y: int; Open: bool }

type MazeItem =
  | Cell of Cell
  | Wall of Wall
  with
    member this.X =
      match this with
      | Cell cell -> cell.X
      | Wall wall -> wall.X
    member this.Y =
      match this with
      | Cell cell -> cell.Y
      | Wall wall -> wall.Y
    member this.isClosedWall =
      match this with
      | Wall wall -> not wall.Open
      | _ -> false

type MazePath =
  { Items: MazeItem list
    Solved: bool
    ElapsedMilliseconds: double option }
  with
    member this.isInPath (item: MazeItem) =
      this.Items |> List.exists (fun x -> item = x)

type Maze =
  { Height: int
    Width: int
    mutable Grid: MazeItem[,] }
  with
    member this.StartX = 0
    member this.StartY = 1
    member this.EndX = this.Height - 1
    member this.EndY = this.Width - 2
    member this.Start = this.Grid[this.StartX, this.StartY]
    member this.End = this.Grid[this.EndX, this.EndY]

    member this.distanceToEnd (item: MazeItem) =
      let x = abs (this.EndX - item.X)
      let y = abs (this.EndY - item.Y)
      x + y

    member this.getUnvisitedNeighbours(item: MazeItem) (visited: MazeItem list) : MazeItem list =
      let mutable neighbours = List.empty<MazeItem>
      let grid = this.Grid
      let x = item.X
      let y = item.Y
      if x > 0 && not grid[x - 1, y].isClosedWall && not (visited |> List.contains grid[x - 1, y]) then
        neighbours <- neighbours @ [grid[x - 1, y]]
      if x < this.Height - 1 && not grid[x + 1, y].isClosedWall && not (visited |> List.contains grid[x + 1, y]) then
        neighbours <- neighbours @ [grid[x + 1, y]]
      if y > 0 && not grid[x, y - 1].isClosedWall && not (visited |> List.contains grid[x, y - 1]) then
        neighbours <- neighbours @ [grid[x, y - 1]]
      if y < this.Width - 1 && not grid[x, y + 1].isClosedWall && not (visited |> List.contains grid[x, y + 1]) then
        neighbours <- neighbours @ [grid[x, y + 1]]
      neighbours

    member this.solveDFS (): MazePath =
      let stopwatch = Stopwatch.StartNew()

      // define recursive function:
      // * visited is an accumulator that keeps track of the visited items
      // * return type is the same as the accumulator type
      // * recursion exit condition is when there are no more neighbours to explore (neighbours = [])
      // * the function is tail-recursive
      let rec processNeighbours (neighbours: MazeItem list) (visited: MazeItem list): MazeItem list =
        match neighbours with
        | [] -> visited
        | current :: rest ->
          let newVisited = visited @ [current]
          if current = this.End then
            processNeighbours [] newVisited
          else if visited |> List.contains current then
            // continue with the rest
            processNeighbours rest visited
          else
            let unvisitedNeighbors =
              this.getUnvisitedNeighbours current visited
              |> List.sortBy (fun item -> this.distanceToEnd item)
            processNeighbours (unvisitedNeighbors @ rest) newVisited

      let start = this.Grid.[this.StartX, this.StartY]
      let visited = [start]
      let neighbours = this.getUnvisitedNeighbours start visited
      let path = processNeighbours neighbours visited

      stopwatch.Stop()
      { Items = path
        Solved = path |> List.exists (fun item -> item = this.End)
        ElapsedMilliseconds = Some stopwatch.Elapsed.TotalMilliseconds }

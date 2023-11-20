namespace MazeTypes

// Required to have the same name as the type Maze in MazeTypes.fs
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Maze =

  open System

  let parse (text: string): Maze =
    if String.IsNullOrEmpty(text) then
      failwith "Invalid maze text"

    let lines = text.Split(Environment.NewLine)
    let mazeHeight = lines.Length
    let mazeWidth = lines[0].Length / 2 // 2 chars per cell

    if (isEven mazeHeight) || (isEven mazeWidth) || (lines[0].Length % 2 <> 0) then
      failwith "Invalid maze dimensions"

    let grid = Array2D.zeroCreate<MazeItem> mazeHeight mazeWidth

    for i in 0 .. mazeHeight - 1 do
      let pairs = lines[i] |> toPairs

      for j in 0 .. mazeWidth - 1 do
        if pairs[j] = "[]" then
          grid[i, j] <- Wall { X = i; Y = j; Open = false}
        else
          if (isEven i) || (isEven j) then
            grid[i, j] <- Wall { X = i; Y = j; Open = true }
          else
            grid[i, j] <- Cell { X = i; Y = j}

    { Height = mazeHeight; Width = mazeWidth; Grid = grid }

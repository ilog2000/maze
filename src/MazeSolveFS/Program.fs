open System
open System.IO
open MazeTypes

// Initialize console to support Unicode
ConsoleUI.init()

let args = Environment.GetCommandLineArgs()
let mutable file = args[1]

if String.IsNullOrEmpty(file) then
  file <- ConsoleUI.askForString "Enter sample file path"

if not <| File.Exists(file) then
  printfn "Could not find sample file. Nothing to solve, exiting..."
  Environment.Exit(1)

let text = File.ReadAllText(file)
let maze = Maze.parse(text)

let path = maze.solveDFS()
displayPathHighlighted maze path
displayPathInfo path

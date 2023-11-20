module ConsoleUI

open System

let init () =
  // Set the console encoding to Unicode
  Console.OutputEncoding <- System.Text.Encoding.Unicode

let askForString (enquiry: string) : string =
  printf "%s: " enquiry
  Console.ReadLine()

let askYN (question: string) : bool =
  printf "%s (y/n) " question
  let key = Console.ReadKey()
  printfn ""
  key.Key = ConsoleKey.Y

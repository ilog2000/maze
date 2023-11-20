[<AutoOpen>]
module Utils

open System

let isEven x = (x % 2) = 0

// "112233" -> ["11"; "22"; "33"]
let toPairs (text: string) : string list =
  let rec loop (text: string) (pairs: string list) =
    if String.IsNullOrEmpty(text) then
      pairs
    else
      let pair = text.Substring(0, 2)
      let rest = text.Substring(2)
      loop rest (pairs @ [pair])
  loop text []

// "112233" -> ["33"; "22"; "11"]
let toPairsReversed (text: string) : string list =
  let rec loop (text: string) (pairs: string list) =
    if String.IsNullOrEmpty(text) then
      pairs
    else
      let pair = text.Substring(0, 2)
      let rest = text.Substring(2)
      loop rest (pair :: pairs)
  loop text []

// "112233" -> ["11"; "22"; "33"]
let toPairsAlt (text: string) : string list =
  let mutable pairs = List.empty<string>
  let mutable i = 0
  while i < text.Length do
    let pair = text.Substring(i, 2)
    i <- i + 2
    pairs <- pairs @ [pair]
  pairs

// "112233" -> ["33"; "22"; "11"]
let toPairsReversedAlt (text: string) : string list =
  let mutable pairs = List.empty<string>
  let mutable i = 0
  while i < text.Length do
    let pair = text.Substring(i, 2)
    i <- i + 2
    pairs <- pair :: pairs
  pairs

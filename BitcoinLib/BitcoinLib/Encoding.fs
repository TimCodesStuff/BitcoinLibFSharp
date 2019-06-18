module Encoding

open System
open System.Linq

let HexStringToByteArray (hex : string) = 
    Enumerable.Range(0, hex.Length)
    |> Enumerable.ToArray
    |> Array.filter (fun x -> (x % 2) = 0)
    |> Array.map (fun x -> Convert.ToByte(hex.Substring(x, 2), 16))

let ByteArrayToHexString (includeDashes : bool) (byteArray : byte[]) =
    match includeDashes with
    | true -> BitConverter.ToString(byteArray).ToLower()
    | false -> BitConverter.ToString(byteArray).Replace("-", "").ToLower()
module Encoding

open System
open System.Linq
open System.Numerics

let HexStringToByteArray (hex : string) = 
    Enumerable.Range(0, hex.Length)
    |> Enumerable.ToArray
    |> Array.filter (fun x -> (x % 2) = 0)
    |> Array.map (fun x -> Convert.ToByte(hex.Substring(x, 2), 16))

let ByteArrayToHexString (includeDashes : bool) (byteArray : byte[]) =
    match includeDashes with
    | true -> BitConverter.ToString(byteArray).ToLower()
    | false -> BitConverter.ToString(byteArray).Replace("-", "").ToLower()

let ByteArrayToBigInt (array : byte[]) =
    let rec buildBigInt (bigInt : BigInteger) (arr : byte[]) =
        if arr.Length = 0 then
            bigInt
        else
            let bi = (bigInt * (bigint 256)) + (BigInteger(int(arr.[0])))
            let arrayTail = arr.[1..]
            buildBigInt bi arrayTail

    buildBigInt (bigint 0) array

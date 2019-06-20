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

// Base 58 contains no 0, O, l, or I.
let private Base58CharSet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
let Base58Encode (array : byte[]) =
    let arrayAsBigInt = ByteArrayToBigInt array

    let rec encode (number : bigint) (base58Encoded : string) =
        if number = bigint(0) then
            base58Encoded
        else
            let charSetIndex = number % bigint(58)
            let newChar = Base58CharSet.[int(index)]
            let encodedString = (string)newChar + base58Encoded
            let num = number / bigint(58)
            encode num encodedString

    encode arrayAsBigInt ""


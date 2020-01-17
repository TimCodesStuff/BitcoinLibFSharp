module Encoding

open System
open System.Linq
open System.Numerics

let private hexCharSet = "0123456789ABCDEF"

let (|Hex|_|) (str: string) =
    str.ToUpper()
    |> Seq.exists (fun c -> not (Set.ofSeq(hexCharSet).Contains c))
    |> function
    | true -> None // Found non-hex chars in string
    | false -> Some () // All chars are hex

let HexStringToByteArray (hex : string) =
    match hex with
    | Hex ->
        Enumerable.Range(0, hex.Length)
        |> Enumerable.ToArray
        |> Array.filter (fun x -> (x % 2) = 0)
        |> Array.map (fun x -> Convert.ToByte(hex.Substring(x, 2), 16))
        |> Ok
    | _ -> Error (sprintf "Incorrectly formatted hex string: '%s'" hex)

let ByteArrayToHexString (byteArray : byte[]) =
    try Ok (BitConverter.ToString(byteArray).Replace("-", "").ToLower())
    with | e -> Error (e.ToString())

let ByteArrayToHexStringWithDashes (byteArray : byte[]) =
    try Ok (BitConverter.ToString(byteArray).ToLower())
    with | e -> Error (e.ToString())

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
let private base58CharSet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz"
let Base58Encode (array : byte[]) =
    let arrayAsBigInt = ByteArrayToBigInt array

    // Base58 Encoded values can have leading zeros.
    // These need to be saved before converting to a BigInt.
    let rec countLeadingZeros (byteArr : byte seq) (zeroCount : int) =
        let firstByte =  byteArr |> Seq.head
        let tail = byteArr |> Seq.tail
        if firstByte <> byte(0) then
            zeroCount
        else
            let newCount = zeroCount + 1 
            countLeadingZeros tail newCount

    let rec encode (number : bigint) (base58Encoded : string) =
        if number = bigint(0) then
            base58Encoded
        else
            let charSetIndex = number % bigint(58)
            let newChar = base58CharSet.[int(charSetIndex)]
            let encodedString = (string)newChar + base58Encoded
            let num = number / bigint(58)
            encode num encodedString

    let base58EncodedTail = encode arrayAsBigInt ""
    let leadingZeroCount = countLeadingZeros (array |> Array.toSeq) 0
    let base58EncodedZeros = String.replicate leadingZeroCount "1"
    base58EncodedZeros + base58EncodedTail

// This function will lose the leading zeros after processing any leading 1's.
let Base58StringToBigInt (base58String : string) =
    let rec buildBigIntFromString (base58str : string) (bigInt : BigInteger) = 
        if base58str.Length = 0 then
            bigInt
        else
            let firstChar = Seq.head base58str
            let tailChars = Seq.tail base58str |> String.Concat
            let firstCharVal = base58CharSet.IndexOf firstChar
            let tempBigInt = (bigInt * bigint(58)) + bigint(firstCharVal)
            buildBigIntFromString tailChars tempBigInt

    buildBigIntFromString base58String (bigint(0))

let Base58Decode (base58Str : string) =
    let rec convertLeadingOnesToZeros (str : string) (zeros : byte[]) =
        let firstChar =  str |> Seq.head
        let tail = str |> Seq.tail |> String.Concat
        if firstChar <> '1' then
            zeros
        else
            let newArray =  Array.append [|byte(0)|] zeros
            convertLeadingOnesToZeros tail newArray
    
    let byteArrayWithoutZeros = 
        (Base58StringToBigInt base58Str).ToByteArray().Reverse() // Reverse to Big Endian
        |> Seq.skipWhile (fun i -> i = byte(0)) // Strip sign byte
        |> Seq.toArray
    let leadingZeros = convertLeadingOnesToZeros base58Str [||]
    
    Array.append leadingZeros byteArrayWithoutZeros
    
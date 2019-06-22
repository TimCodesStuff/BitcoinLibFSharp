module Encoding

open Crypto
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

    // Base58 Encoded values can have leading zeros.
    // These need to be saved before converting to a BigInt.
    let rec countLeadingZeros (byteArr : byte seq) (zeroCount : int) =
        let firstByte =  byteArr |> Seq.head
        let tail = byteArr |> Seq.tail
        if not (firstByte = byte(0)) then
            zeroCount
        else
            let newCount = zeroCount + 1 
            countLeadingZeros tail newCount

    let rec encode (number : bigint) (base58Encoded : string) =
        if number = bigint(0) then
            base58Encoded
        else
            let charSetIndex = number % bigint(58)
            let newChar = Base58CharSet.[int(charSetIndex)]
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
            let firstCharVal = Base58CharSet.IndexOf firstChar
            let tempBigInt = (bigInt * bigint(58)) + bigint(firstCharVal)
            buildBigIntFromString tailChars tempBigInt

    buildBigIntFromString base58String (bigint(0))

let Base58Decode (base58Str : string) =
    let rec convertLeadingOnesToZeros (str : string) (zeros : byte[]) =
        let firstChar =  str |> Seq.head
        let tail = str |> Seq.tail |> String.Concat
        if not (firstChar = '1') then
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

let WifToHex (wif : string) =
    let hex = wif |> Base58Decode |> ByteArrayToHexString false
    // Remove first byte (network byte) and last 4 bytes (checksum)
    hex.Substring(2, 64)

let HexToWif (isMainNetwork : bool) (isCompressedPublicKey : bool) (hex : string) =
    let hexWithNetworkByte = if isMainNetwork then "80" + hex else "EF" + hex
    let fullHex = if isCompressedPublicKey then hexWithNetworkByte + "01" else hexWithNetworkByte
    let doubleSha = fullHex |> HexStringToByteArray |> Crypto.Sha256 |> Crypto.Sha256
    let checksum = (ByteArrayToHexString false doubleSha).Substring(0, 8);
    let hexWithChecksum = fullHex + checksum
    hexWithChecksum |> HexStringToByteArray |> Base58Encode
    
module Crypto

open System.Security.Cryptography
open System

let Sha256 (array : byte[]) =
    (new SHA256Managed()).ComputeHash(array)

let RipeMD160 (array : byte[]) =
    (new RIPEMD160Managed()).ComputeHash(array)

let GenerateRand256BitKey () =
    let key : byte[] = Array.zeroCreate 32
    (RandomNumberGenerator.Create()).GetBytes(key)
    key

let GenerateRandECDSACompliant256BitKey () =
    let upperHexLimit = "fffffffffffffffffffffffffffffffebaaedce6af48a03bbfd25e8cd0364140"
    let rng = RandomNumberGenerator.Create()
    let key : byte[] = Array.zeroCreate 32

    let rec getKeyInBounds() =
        rng.GetBytes(key)
        let hexKey = (Encoding.ByteArrayToHexString false key).ToLower()
        if (String.Compare(hexKey, upperHexLimit, true) >= 0) then
            getKeyInBounds()
        else
            key

    getKeyInBounds()
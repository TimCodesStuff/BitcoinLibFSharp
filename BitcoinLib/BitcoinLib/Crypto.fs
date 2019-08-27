module Crypto

open Result
open System.Linq
open System.Security.Cryptography

let Sha256 (array : byte[]) =
    try Ok ((new SHA256Managed()).ComputeHash(array))
    with
    | e -> Error (e.ToString())

let RipeMD160 (array : byte[]) =
    try
        Ok ((new RIPEMD160Managed()).ComputeHash(array))
    with
    | e -> Error (e.ToString())

// Used in many places to generate checksums.
let DoubleSha256 (address : byte[]) =
    result {
        let! firstSha = Sha256(address)
        return! Sha256(firstSha)
    }

let GenerateChecksum (input : byte[]) =
    result {
        let! doubleSha = input |> DoubleSha256
        return (doubleSha).Take(4).ToArray()
    }
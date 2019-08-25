﻿module Crypto

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
//TODO: Move the take4 out of this function, it's misleading.
let DoubleSha256 (address : byte[]) =
    result {
        let! firstSha = Sha256(address)
        let! secondSha = Sha256(firstSha)
        return (secondSha).Take(4).ToArray()
    }    
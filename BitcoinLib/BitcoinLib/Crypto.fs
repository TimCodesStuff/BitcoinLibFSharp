module Crypto

open System.Security.Cryptography

let Sha256 (array : byte[]) =
    (new SHA256Managed()).ComputeHash(array)

let RipeMD160 (array : byte[]) =
    try
        Ok ((new RIPEMD160Managed()).ComputeHash(array))
    with
    | e -> Error (e.ToString())
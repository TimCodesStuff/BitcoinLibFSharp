module Encoding

open System

let ByteArrayToHexString (includeDashes : bool) (byteArray : byte[]) =
    match includeDashes with
    | true -> BitConverter.ToString(byteArray).ToLower()
    | false -> BitConverter.ToString(byteArray).Replace("-", "").ToLower()
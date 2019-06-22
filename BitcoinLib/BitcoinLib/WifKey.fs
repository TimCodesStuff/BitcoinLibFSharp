module WifKey

open Encoding

type WifKey = {
    NetworkHex: string;
    PublicKey: string;
    Checksum: string; }

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

let DecomposeWifKey (isCompressedPublicKey : bool) (wif : string) =
    let hex = wif |> Base58Decode |> ByteArrayToHexString false
    if isCompressedPublicKey then
        {NetworkHex = hex.Substring(0,2); PublicKey = hex.Substring(2, 64); Checksum = hex.Substring(68, 8);}
    else
        {NetworkHex = hex.Substring(0,2); PublicKey = hex.Substring(2, 62); Checksum = hex.Substring(66, 8);}

let IsWifChecksumValid (isCompressedPublicKey : bool) (wif : string) =
    let wifAddress = DecomposeWifKey isCompressedPublicKey wif
    let byteArray = 
        if isCompressedPublicKey then
            wifAddress.NetworkHex + wifAddress.PublicKey + "01"
        else
            wifAddress.NetworkHex + wifAddress.PublicKey
        |> HexStringToByteArray
    let doubleSha = byteArray |> Crypto.Sha256 |> Crypto.Sha256
    let checksum = (doubleSha |> ByteArrayToHexString false).Substring(0,8)
    checksum = wifAddress.Checksum
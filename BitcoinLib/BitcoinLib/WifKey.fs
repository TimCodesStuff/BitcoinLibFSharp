module WifKey

open Encoding

type WifKey = {
    NetworkHex: string;
    PublicKey: string;
    Checksum: string; }

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
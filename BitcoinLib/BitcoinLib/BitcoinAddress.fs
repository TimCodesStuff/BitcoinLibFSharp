module BitcoinAddress

open BitcoinAddressRecord
open Org.BouncyCastle.Crypto.Parameters
open Org.BouncyCastle.Math
open Org.BouncyCastle.Asn1.Sec
open Result
open System
open System.Security.Cryptography

let GenerateRand256BitKey () =
    let key : byte[] = Array.zeroCreate 32
    (RandomNumberGenerator.Create()).GetBytes(key)
    key

let GenerateRandECDSACompliant256BitKey () =
    let upperHexLimit = "fffffffffffffffffffffffffffffffebaaedce6af48a03bbfd25e8cd0364140"
    let rng = RandomNumberGenerator.Create()
    let key : byte[] = Array.zeroCreate 32

    let rec getKeyInBounds() =
        result {
            rng.GetBytes(key)
            let! hexKey = (Encoding.ByteArrayToHexString key)
            if (String.Compare(hexKey.ToLower(), upperHexLimit, true) >= 0) then
                return! getKeyInBounds()
            else
                return key
        }
        
    getKeyInBounds()

let GenerateFullPublicKey (publicKeyX : byte[]) (publicKeyY : byte[]) =
    Array.append (Array.append [| byte(0x04); |] publicKeyX) publicKeyY

let GenerateCompressedPublicKey (publicKeyX : byte[]) (publicKeyY : byte[]) =
    if (publicKeyY.Length < 32) then
        Error "Error generating compressed public key: Public Key Y value must be at least 32 bytes long."
    elif int(publicKeyY.[31]) % 2 = 0 then
        Ok (Array.append [| byte(0x02) |] publicKeyX)
    else
        Ok (Array.append [| byte(0x03) |] publicKeyX)

let GetSecp256k1PublicKey (privateKey : byte[]) =
    let curve = SecNamedCurves.GetByName("secp256k1")
    let domain = new ECDomainParameters(curve.Curve, curve.G, curve.N, curve.H)
    let d = new BigInteger(+1, privateKey)
    let publicKey = new ECPublicKeyParameters(domain.G.Multiply(d), domain)
    (publicKey.Q.XCoord.GetEncoded(), publicKey.Q.YCoord.GetEncoded())

// There are two networks, the main network and the test network.
let GetNetworkByteValue (isMainNetwork : bool) =
    if isMainNetwork then byte(0x00) else  byte(0x6f)

let private GenerateBitcoinAddressRecord (isMainNetwork : bool) (privateKey : byte[]) =
    result {
        let (x,y) = GetSecp256k1PublicKey privateKey
        let! compressedPublicKey = GenerateCompressedPublicKey x y
        let! shaHashedPublicKey = Crypto.Sha256 compressedPublicKey
        let! shaRipeHashedPublicKey = Crypto.RipeMD160 shaHashedPublicKey
    
        let payToPublicKeyAddress = Array.append [|(GetNetworkByteValue isMainNetwork)|] shaRipeHashedPublicKey
        let! payToPublicKeyAddressChecksum = payToPublicKeyAddress |> Crypto.GenerateChecksum
        let payToPublicKeyAddressWithChecksum = Array.append payToPublicKeyAddress payToPublicKeyAddressChecksum

        let payToScriptHashAddressByte = if isMainNetwork then byte(0x05) else byte(0xc4)
        let payToScriptHashAddressInit = Array.append [|GetNetworkByteValue isMainNetwork; byte(0x14)|] shaRipeHashedPublicKey
        let! payToScriptSha256 = payToScriptHashAddressInit |> Crypto.Sha256
        let! payToScriptHash160 =  payToScriptSha256 |> Crypto.RipeMD160
        let payToScriptHashWithoutChecksum = Array.append [| payToScriptHashAddressByte |] payToScriptHash160
        let! payToScriptHashChecksum = payToScriptHashWithoutChecksum |> Crypto.GenerateChecksum
        let payToScriptHashWithChecksum = Array.append payToScriptHashWithoutChecksum payToScriptHashChecksum

        let metadata = {
            privateKey = privateKey;
            publicKeyX = x;
            publicKeyY = y;
            publicKeyCompressed = compressedPublicKey;
            publicKeyFull = GenerateFullPublicKey x y;
            publicKeySha256 = shaHashedPublicKey;
            publicKeySha256Ripe = shaRipeHashedPublicKey;
            isMainNetwork = isMainNetwork;

            // pay to public key hash
            p2pkh_publicKeyWithNetworkByte = payToPublicKeyAddress;
            p2pkh_checksum = payToPublicKeyAddressChecksum;
            p2pkh_addressWithChecksum = payToPublicKeyAddressWithChecksum;

            // pay to script hash
            p2sh_init = payToScriptHashAddressInit;
            p2sh_addressWithoutChecksum = payToScriptHashWithoutChecksum;
            p2sh_checksum = payToScriptHashChecksum;
            p2sh_addressWithChecksum = payToScriptHashWithChecksum;
        }
        let! privateKeyHex = privateKey |> Encoding.ByteArrayToHexString
        let! privateKeyWif = WifKey.HexToWif isMainNetwork true privateKeyHex
        let! publicKeyFull = metadata.publicKeyFull |> Encoding.ByteArrayToHexString
        let! publicKeyCompressed = compressedPublicKey |> Encoding.ByteArrayToHexString
        return {
            Metadata = metadata;
            PublicKeyFull = publicKeyFull;
            PublicKeyCompressed =  publicKeyCompressed;
            PrivateKeyHex = privateKeyHex;
            PrivateKeyWIF = privateKeyWif;
            P2PKHAddress = Encoding.Base58Encode payToPublicKeyAddressWithChecksum;
            P2SHAddress = Encoding.Base58Encode payToScriptHashWithChecksum;
        }
    }

let GenerateBitcoinAddressRecordFromPrivateKeyHex (isMainNetwork : bool) (hex : string) : Result<BitcoinAddressRecord, string> =
    result {
        let! privateKey = Encoding.HexStringToByteArray hex
        return! GenerateBitcoinAddressRecord isMainNetwork privateKey
    }

let GenerateBitcoinAddressRecordFromPrivateKeyWIF (isMainNetwork : bool) (wif : string) : Result<BitcoinAddressRecord, string> =
    result {
        let! hex = WifKey.WifToHex wif
        let! privateKey = Encoding.HexStringToByteArray hex
        return! GenerateBitcoinAddressRecord isMainNetwork privateKey
    }

let GenerateNewRandomBitcoinAddressRecord (isMainNetwork : bool) : Result<BitcoinAddressRecord, string> =
    result {
        let! newKey = GenerateRandECDSACompliant256BitKey()
        return! GenerateBitcoinAddressRecord isMainNetwork newKey
    }
    
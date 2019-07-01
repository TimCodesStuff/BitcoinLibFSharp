module BitcoinAddressRecord

type BitcoinAddressRecord = {
    privateKey : byte[];
    publicKeyX : byte[];
    publicKeyY : byte[];
    publicKeyCompressed : byte[];
    publicKeySha256 : byte[];
    publicKeySha256Ripe : byte[];
    isMainNetwork : bool;

    // pay to public key hash
    p2pkh_publicKeyWithNetworkByte : byte[];
    p2pkh_checksum : byte[];
    p2pkh_addressWithChecksum : byte[];

    // pay to script hash
    p2sh_init : byte[];
    p2sh_addressWithoutChecksum : byte[];
    p2sh_checksum : byte[];
    p2sh_addressWithChecksum : byte[];

    // main entities
    PublicKeyFull : byte[];
    PrivateKeyHex :  string;
    PrivateKeyWIF :  string;
    P2PKHAddress :  string;
    P2SHAddress :  string;
    }
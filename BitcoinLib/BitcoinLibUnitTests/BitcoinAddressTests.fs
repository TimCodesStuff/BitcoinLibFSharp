module BitcoinAddressTests

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type BitcoinAddressTests () =
    
    [<TestMethod>]
    member this.TestGenerateBitcoinAddressRecordFromHex () =
        let address = BitcoinAddress.GenerateBitcoinAddressRecordFromPrivateKeyHex true "18e14a7b6a307f426a94f8114701e7c8e774e7f9a47e2c2035db29a206321725"
        
        let expectedWif = "Kx45GeUBSMPReYQwgXiKhG9FzNXrnCeutJp4yjTd5kKxCitadm3C"
        Assert.AreEqual(expectedWif, address.PrivateKeyWIF)

        let expectedPubKeyX = "50863ad64a87ae8a2fe83c1af1a8403cb53f53e486d8511dad8a04887e5b2352"
        Assert.AreEqual(expectedPubKeyX, (Encoding.ByteArrayToHexString false address.publicKeyX))
        let expectedPubKeyY = "2cd470243453a299fa9e77237716103abc11a1df38855ed6f2ee187e9c582ba6"
        Assert.AreEqual(expectedPubKeyY, (Encoding.ByteArrayToHexString false address.publicKeyY))
        let expectedFullKey = "0450863ad64a87ae8a2fe83c1af1a8403cb53f53e486d8511dad8a04887e5b23522cd470243453a299fa9e77237716103abc11a1df38855ed6f2ee187e9c582ba6"
        Assert.AreEqual(expectedFullKey, (Encoding.ByteArrayToHexString false address.PublicKeyFull))
        let expectedCompressedKey = "0250863ad64a87ae8a2fe83c1af1a8403cb53f53e486d8511dad8a04887e5b2352"
        Assert.AreEqual(expectedCompressedKey, (Encoding.ByteArrayToHexString false address.publicKeyCompressed))
        let expectedCompressedKeySha = "0b7c28c9b7290c98d7438e70b3d3f7c848fbd7d1dc194ff83f4f7cc9b1378e98"
        Assert.AreEqual(expectedCompressedKeySha, (Encoding.ByteArrayToHexString false address.publicKeySha256))
        let expectedCompressedKeyShaRipe160 = "f54a5851e9372b87810a8e60cdd2e7cfd80b6e31"
        Assert.AreEqual(expectedCompressedKeyShaRipe160, (Encoding.ByteArrayToHexString false address.publicKeySha256Ripe))
        Assert.AreEqual(true, address.isMainNetwork)

        let expectedPayToPublicKeyHashWNetwork = "00f54a5851e9372b87810a8e60cdd2e7cfd80b6e31"
        Assert.AreEqual(expectedPayToPublicKeyHashWNetwork, (Encoding.ByteArrayToHexString false address.p2pkh_publicKeyWithNetworkByte))
        let expectedPayToPublicKeyHashNetworkChecksum = "c7f18fe8"
        Assert.AreEqual(expectedPayToPublicKeyHashNetworkChecksum, (Encoding.ByteArrayToHexString false address.p2pkh_checksum))
        let expectedPayToPublicKeyHashWNetworkChecksum = "00f54a5851e9372b87810a8e60cdd2e7cfd80b6e31c7f18fe8"
        Assert.AreEqual(expectedPayToPublicKeyHashWNetworkChecksum, (Encoding.ByteArrayToHexString false address.p2pkh_addressWithChecksum))
        let expectedPayToPublicKeyHashAddress = "1PMycacnJaSqwwJqjawXBErnLsZ7RkXUAs"
        Assert.AreEqual(expectedPayToPublicKeyHashAddress, address.P2PKHAddress)

        let expectedPayToScriptHashWNetwork = "05f54a5851e9372b87810a8e60cdd2e7cfd80b6e31"
        Assert.AreEqual(expectedPayToScriptHashWNetwork, (Encoding.ByteArrayToHexString false address.p2sh_publicKeyWithNetworkByte))
        let expectedPayToScriptHashNetworkChecksum = "09f2ae6a"
        Assert.AreEqual(expectedPayToScriptHashNetworkChecksum, (Encoding.ByteArrayToHexString false address.p2sh_checksum))
        let expectedPayToScriptHashWNetworkChecksum = "05f54a5851e9372b87810a8e60cdd2e7cfd80b6e3109f2ae6a"
        Assert.AreEqual(expectedPayToScriptHashWNetworkChecksum, (Encoding.ByteArrayToHexString false address.p2sh_addressWithChecksum))
        let expectedPayToScriptHash = "3Q3zY87DrUmE371Grgc7bsDiVPqpu4mN1f"
        Assert.AreEqual(expectedPayToScriptHash, address.P2SHAddress)
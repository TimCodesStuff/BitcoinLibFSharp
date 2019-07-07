module BitcoinAddressTests

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type BitcoinAddressTests () =
    
    // Private key and address data come from: https://en.bitcoin.it/wiki/Technical_background_of_version_1_Bitcoin_addresses
    [<TestMethod>]
    member this.TestGenerateBitcoinAddressRecordFromHex () =
        let address = BitcoinAddress.GenerateBitcoinAddressRecordFromPrivateKeyHex true "18e14a7b6a307f426a94f8114701e7c8e774e7f9a47e2c2035db29a206321725"
        
        let expectedWif = "Kx45GeUBSMPReYQwgXiKhG9FzNXrnCeutJp4yjTd5kKxCitadm3C"
        Assert.AreEqual(expectedWif, address.PrivateKeyWIF)

        let expectedPubKeyX = "50863ad64a87ae8a2fe83c1af1a8403cb53f53e486d8511dad8a04887e5b2352"
        Assert.AreEqual(expectedPubKeyX, (Encoding.ByteArrayToHexString false address.Metadata.publicKeyX))
        let expectedPubKeyY = "2cd470243453a299fa9e77237716103abc11a1df38855ed6f2ee187e9c582ba6"
        Assert.AreEqual(expectedPubKeyY, (Encoding.ByteArrayToHexString false address.Metadata.publicKeyY))
        let expectedFullKey = "0450863ad64a87ae8a2fe83c1af1a8403cb53f53e486d8511dad8a04887e5b23522cd470243453a299fa9e77237716103abc11a1df38855ed6f2ee187e9c582ba6"
        Assert.AreEqual(expectedFullKey, address.PublicKeyFull)
        let expectedCompressedKey = "0250863ad64a87ae8a2fe83c1af1a8403cb53f53e486d8511dad8a04887e5b2352"
        Assert.AreEqual(expectedCompressedKey, address.PublicKeyCompressed)
        let expectedCompressedKeySha = "0b7c28c9b7290c98d7438e70b3d3f7c848fbd7d1dc194ff83f4f7cc9b1378e98"
        Assert.AreEqual(expectedCompressedKeySha, (Encoding.ByteArrayToHexString false address.Metadata.publicKeySha256))
        let expectedCompressedKeyShaRipe160 = "f54a5851e9372b87810a8e60cdd2e7cfd80b6e31"
        Assert.AreEqual(expectedCompressedKeyShaRipe160, (Encoding.ByteArrayToHexString false address.Metadata.publicKeySha256Ripe))
        Assert.AreEqual(true, address.Metadata.isMainNetwork)

        let expectedPayToPublicKeyHashWNetwork = "00f54a5851e9372b87810a8e60cdd2e7cfd80b6e31"
        Assert.AreEqual(expectedPayToPublicKeyHashWNetwork, (Encoding.ByteArrayToHexString false address.Metadata.p2pkh_publicKeyWithNetworkByte))
        let expectedPayToPublicKeyHashNetworkChecksum = "c7f18fe8"
        Assert.AreEqual(expectedPayToPublicKeyHashNetworkChecksum, (Encoding.ByteArrayToHexString false address.Metadata.p2pkh_checksum))
        let expectedPayToPublicKeyHashWNetworkChecksum = "00f54a5851e9372b87810a8e60cdd2e7cfd80b6e31c7f18fe8"
        Assert.AreEqual(expectedPayToPublicKeyHashWNetworkChecksum, (Encoding.ByteArrayToHexString false address.Metadata.p2pkh_addressWithChecksum))
        let expectedPayToPublicKeyHashAddress = "1PMycacnJaSqwwJqjawXBErnLsZ7RkXUAs"
        Assert.AreEqual(expectedPayToPublicKeyHashAddress, address.P2PKHAddress)

        let expectedPayToScriptHashInitialValue = "0014f54a5851e9372b87810a8e60cdd2e7cfd80b6e31"
        Assert.AreEqual(expectedPayToScriptHashInitialValue, (Encoding.ByteArrayToHexString false address.Metadata.p2sh_init))
        let expectedPayToScripthashWithoutChecksum = "0570b4065d87d8ba1bbf82e82227a0d04a36a57c80"
        Assert.AreEqual(expectedPayToScripthashWithoutChecksum, (Encoding.ByteArrayToHexString false address.Metadata.p2sh_addressWithoutChecksum))
        let expectedPayToScriptHashNetworkChecksum = "e45d708e"
        Assert.AreEqual(expectedPayToScriptHashNetworkChecksum, (Encoding.ByteArrayToHexString false address.Metadata.p2sh_checksum))
        let expectedPayToScriptHashWNetworkChecksum = "0570b4065d87d8ba1bbf82e82227a0d04a36a57c80e45d708e"
        Assert.AreEqual(expectedPayToScriptHashWNetworkChecksum, (Encoding.ByteArrayToHexString false address.Metadata.p2sh_addressWithChecksum))
        let expectedPayToScriptHash = "3BxwGNjvG4CP14tAZodgYyZ7UTjruYDyAM"
        Assert.AreEqual(expectedPayToScriptHash, address.P2SHAddress)

    // Private key and address data come from: https://en.bitcoin.it/wiki/Technical_background_of_version_1_Bitcoin_addresses
    [<TestMethod>]
    member this.TestGenerateBitcoinAddressRecordFromWif () =
        let address = BitcoinAddress.GenerateBitcoinAddressRecordFromPrivateKeyWIF true "Kx45GeUBSMPReYQwgXiKhG9FzNXrnCeutJp4yjTd5kKxCitadm3C"
        
        let expectedHex = "18e14a7b6a307f426a94f8114701e7c8e774e7f9a47e2c2035db29a206321725"
        Assert.AreEqual(expectedHex, address.PrivateKeyHex)

        let expectedPubKeyX = "50863ad64a87ae8a2fe83c1af1a8403cb53f53e486d8511dad8a04887e5b2352"
        Assert.AreEqual(expectedPubKeyX, (Encoding.ByteArrayToHexString false address.Metadata.publicKeyX))
        let expectedPubKeyY = "2cd470243453a299fa9e77237716103abc11a1df38855ed6f2ee187e9c582ba6"
        Assert.AreEqual(expectedPubKeyY, (Encoding.ByteArrayToHexString false address.Metadata.publicKeyY))
        let expectedFullKey = "0450863ad64a87ae8a2fe83c1af1a8403cb53f53e486d8511dad8a04887e5b23522cd470243453a299fa9e77237716103abc11a1df38855ed6f2ee187e9c582ba6"
        Assert.AreEqual(expectedFullKey, address.PublicKeyFull)
        let expectedCompressedKey = "0250863ad64a87ae8a2fe83c1af1a8403cb53f53e486d8511dad8a04887e5b2352"
        Assert.AreEqual(expectedCompressedKey, address.PublicKeyCompressed)
        let expectedCompressedKeySha = "0b7c28c9b7290c98d7438e70b3d3f7c848fbd7d1dc194ff83f4f7cc9b1378e98"
        Assert.AreEqual(expectedCompressedKeySha, (Encoding.ByteArrayToHexString false address.Metadata.publicKeySha256))
        let expectedCompressedKeyShaRipe160 = "f54a5851e9372b87810a8e60cdd2e7cfd80b6e31"
        Assert.AreEqual(expectedCompressedKeyShaRipe160, (Encoding.ByteArrayToHexString false address.Metadata.publicKeySha256Ripe))
        Assert.AreEqual(true, address.Metadata.isMainNetwork)

        let expectedPayToPublicKeyHashWNetwork = "00f54a5851e9372b87810a8e60cdd2e7cfd80b6e31"
        Assert.AreEqual(expectedPayToPublicKeyHashWNetwork, (Encoding.ByteArrayToHexString false address.Metadata.p2pkh_publicKeyWithNetworkByte))
        let expectedPayToPublicKeyHashNetworkChecksum = "c7f18fe8"
        Assert.AreEqual(expectedPayToPublicKeyHashNetworkChecksum, (Encoding.ByteArrayToHexString false address.Metadata.p2pkh_checksum))
        let expectedPayToPublicKeyHashWNetworkChecksum = "00f54a5851e9372b87810a8e60cdd2e7cfd80b6e31c7f18fe8"
        Assert.AreEqual(expectedPayToPublicKeyHashWNetworkChecksum, (Encoding.ByteArrayToHexString false address.Metadata.p2pkh_addressWithChecksum))
        let expectedPayToPublicKeyHashAddress = "1PMycacnJaSqwwJqjawXBErnLsZ7RkXUAs"
        Assert.AreEqual(expectedPayToPublicKeyHashAddress, address.P2PKHAddress)

        let expectedPayToScriptHashInitialValue = "0014f54a5851e9372b87810a8e60cdd2e7cfd80b6e31"
        Assert.AreEqual(expectedPayToScriptHashInitialValue, (Encoding.ByteArrayToHexString false address.Metadata.p2sh_init))
        let expectedPayToScripthashWithoutChecksum = "0570b4065d87d8ba1bbf82e82227a0d04a36a57c80"
        Assert.AreEqual(expectedPayToScripthashWithoutChecksum, (Encoding.ByteArrayToHexString false address.Metadata.p2sh_addressWithoutChecksum))
        let expectedPayToScriptHashNetworkChecksum = "e45d708e"
        Assert.AreEqual(expectedPayToScriptHashNetworkChecksum, (Encoding.ByteArrayToHexString false address.Metadata.p2sh_checksum))
        let expectedPayToScriptHashWNetworkChecksum = "0570b4065d87d8ba1bbf82e82227a0d04a36a57c80e45d708e"
        Assert.AreEqual(expectedPayToScriptHashWNetworkChecksum, (Encoding.ByteArrayToHexString false address.Metadata.p2sh_addressWithChecksum))
        let expectedPayToScriptHash = "3BxwGNjvG4CP14tAZodgYyZ7UTjruYDyAM"
        Assert.AreEqual(expectedPayToScriptHash, address.P2SHAddress)

    // Testing address generation based on https://www.freecodecamp.org/news/how-to-create-a-bitcoin-wallet-address-from-a-private-key-eca3ddd9c05f/
    [<TestMethod>]
     member this.TestP2PKHAddressGeneration () =
        let address = BitcoinAddress.GenerateBitcoinAddressRecordFromPrivateKeyHex true "60cf347dbc59d31c1358c8e5cf5e45b822ab85b79cb32a9f3d98184779a9efc2"

        Assert.AreEqual("1e7bcc70c72770dbb72fea022e8a6d07f814d2ebe4de9ae3f7af75bf706902a7", (Encoding.ByteArrayToHexString false address.Metadata.publicKeyX))
        Assert.AreEqual("b73ff919898c836396a6b0c96812c3213b99372050853bd1678da0ead14487d7", (Encoding.ByteArrayToHexString false address.Metadata.publicKeyY))
        Assert.AreEqual("041e7bcc70c72770dbb72fea022e8a6d07f814d2ebe4de9ae3f7af75bf706902a7b73ff919898c836396a6b0c96812c3213b99372050853bd1678da0ead14487d7", address.PublicKeyFull)
        Assert.AreEqual("031e7bcc70c72770dbb72fea022e8a6d07f814d2ebe4de9ae3f7af75bf706902a7", address.PublicKeyCompressed)
        Assert.AreEqual("453233600a96384bb8d73d400984117ac84d7e8b", (Encoding.ByteArrayToHexString false address.Metadata.publicKeySha256Ripe))
        Assert.AreEqual("512f43c4", (Encoding.ByteArrayToHexString false address.Metadata.p2pkh_checksum))
        Assert.AreEqual("17JsmEygbbEUEpvt4PFtYaTeSqfb9ki1F1", address.P2PKHAddress)


    // Testing Pay To Script Hash address generation based on https://medium.com/coinmonks/how-to-generate-a-bitcoin-address-step-by-step-9d7fcbf1ad0b
    [<TestMethod>]
     member this.TestP2SHAddressGeneration () =
        let address = BitcoinAddress.GenerateBitcoinAddressRecordFromPrivateKeyHex true "a966eb6058f8ec9f47074a2faadd3dab42e2c60ed05bc34d39d6c0e1d32b8bdf"
        Assert.AreEqual("023cba1f4d12d1ce0bced725373769b2262c6daa97be6a0588cfec8ce1a5f0bd09", address.PublicKeyCompressed)
        Assert.AreEqual("051d521dcf4983772b3c1e6ef937103ebdfaa1ad77", (Encoding.ByteArrayToHexString false address.Metadata.p2sh_addressWithoutChecksum))
        Assert.AreEqual("c41d25f2", (Encoding.ByteArrayToHexString false address.Metadata.p2sh_checksum))
        Assert.AreEqual("051d521dcf4983772b3c1e6ef937103ebdfaa1ad77c41d25f2", (Encoding.ByteArrayToHexString false address.Metadata.p2sh_addressWithChecksum))
        Assert.AreEqual("34N3tf5m5rdNhW5zpTXNEJucHviFEa8KEq", address.P2SHAddress)
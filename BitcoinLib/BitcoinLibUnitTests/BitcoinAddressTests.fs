module BitcoinAddressTests

open Microsoft.VisualStudio.TestTools.UnitTesting
open Result

[<TestClass>]
type BitcoinAddressTests () =
    
    // Private key and address data come from: https://en.bitcoin.it/wiki/Technical_background_of_version_1_Bitcoin_addresses
    [<TestMethod>]
    member this.TestGenerateBitcoinAddressRecordFromHex () =
        result {
            let! address = BitcoinAddress.GenerateBitcoinAddressRecordFromPrivateKeyHex true "18e14a7b6a307f426a94f8114701e7c8e774e7f9a47e2c2035db29a206321725"
            let expectedWif = "Kx45GeUBSMPReYQwgXiKhG9FzNXrnCeutJp4yjTd5kKxCitadm3C"
            Assert.AreEqual(expectedWif, address.PrivateKeyWIF)
            
            let expectedPubKeyX = "50863ad64a87ae8a2fe83c1af1a8403cb53f53e486d8511dad8a04887e5b2352"
            let! actualPubKeyX = Encoding.ByteArrayToHexString address.Metadata.publicKeyX
            Assert.AreEqual(expectedPubKeyX, actualPubKeyX)
            let expectedPubKeyY = "2cd470243453a299fa9e77237716103abc11a1df38855ed6f2ee187e9c582ba6"
            let! actualPubKeyY = Encoding.ByteArrayToHexString address.Metadata.publicKeyY
            Assert.AreEqual(expectedPubKeyY, actualPubKeyY)
            let expectedFullKey = "0450863ad64a87ae8a2fe83c1af1a8403cb53f53e486d8511dad8a04887e5b23522cd470243453a299fa9e77237716103abc11a1df38855ed6f2ee187e9c582ba6"
            Assert.AreEqual(expectedFullKey, address.PublicKeyFull)
            let expectedCompressedKey = "0250863ad64a87ae8a2fe83c1af1a8403cb53f53e486d8511dad8a04887e5b2352"
            Assert.AreEqual(expectedCompressedKey, address.PublicKeyCompressed)
            let expectedCompressedKeySha = "0b7c28c9b7290c98d7438e70b3d3f7c848fbd7d1dc194ff83f4f7cc9b1378e98"
            let! actualCompressedKeySha = Encoding.ByteArrayToHexString address.Metadata.publicKeySha256
            Assert.AreEqual(expectedCompressedKeySha, actualCompressedKeySha)
            let expectedCompressedKeyShaRipe160 = "f54a5851e9372b87810a8e60cdd2e7cfd80b6e31"
            let! actualCompressedKeyShaRipe160 = Encoding.ByteArrayToHexString address.Metadata.publicKeySha256Ripe
            Assert.AreEqual(expectedCompressedKeyShaRipe160, actualCompressedKeyShaRipe160)
            Assert.AreEqual(true, address.Metadata.isMainNetwork)

            let expectedPayToPublicKeyHashWNetwork = "00f54a5851e9372b87810a8e60cdd2e7cfd80b6e31"
            let! actualPayToPublicKeyHashWNetwork = Encoding.ByteArrayToHexString address.Metadata.p2pkh_publicKeyWithNetworkByte
            Assert.AreEqual(expectedPayToPublicKeyHashWNetwork, actualPayToPublicKeyHashWNetwork)
            let expectedPayToPublicKeyHashNetworkChecksum = "c7f18fe8"
            let! actualPayToPublicKeyHashNetworkChecksum = Encoding.ByteArrayToHexString address.Metadata.p2pkh_checksum
            Assert.AreEqual(expectedPayToPublicKeyHashNetworkChecksum, actualPayToPublicKeyHashNetworkChecksum)
            let expectedPayToPublicKeyHashWNetworkChecksum = "00f54a5851e9372b87810a8e60cdd2e7cfd80b6e31c7f18fe8"
            let! actualPayToPublicKeyHashWNetworkChecksum = Encoding.ByteArrayToHexString address.Metadata.p2pkh_addressWithChecksum
            Assert.AreEqual(expectedPayToPublicKeyHashWNetworkChecksum, actualPayToPublicKeyHashWNetworkChecksum)
            let expectedPayToPublicKeyHashAddress = "1PMycacnJaSqwwJqjawXBErnLsZ7RkXUAs"
            Assert.AreEqual(expectedPayToPublicKeyHashAddress, address.P2PKHAddress)

            let expectedPayToScriptHashInitialValue = "0014f54a5851e9372b87810a8e60cdd2e7cfd80b6e31"
            let! actualPayToScriptHashInitialValue = Encoding.ByteArrayToHexString address.Metadata.p2sh_init
            Assert.AreEqual(expectedPayToScriptHashInitialValue, actualPayToScriptHashInitialValue)
            let expectedPayToScripthashWithoutChecksum = "0570b4065d87d8ba1bbf82e82227a0d04a36a57c80"
            let! actualPayToScripthashWithoutChecksum = Encoding.ByteArrayToHexString address.Metadata.p2sh_addressWithoutChecksum
            Assert.AreEqual(expectedPayToScripthashWithoutChecksum, actualPayToScripthashWithoutChecksum)
            let expectedPayToScriptHashNetworkChecksum = "e45d708e"
            let! actualPayToScriptHashNetworkChecksum = Encoding.ByteArrayToHexString address.Metadata.p2sh_checksum
            Assert.AreEqual(expectedPayToScriptHashNetworkChecksum, actualPayToScriptHashNetworkChecksum)
            let expectedPayToScriptHashWNetworkChecksum = "0570b4065d87d8ba1bbf82e82227a0d04a36a57c80e45d708e"
            let! actualPayToScriptHashWNetworkChecksum = Encoding.ByteArrayToHexString address.Metadata.p2sh_addressWithChecksum
            Assert.AreEqual(expectedPayToScriptHashWNetworkChecksum, actualPayToScriptHashWNetworkChecksum)
            let expectedPayToScriptHash = "3BxwGNjvG4CP14tAZodgYyZ7UTjruYDyAM"
            Assert.AreEqual(expectedPayToScriptHash, address.P2SHAddress)
        } |> TestHelper.FailOnError

    // Private key and address data come from: https://en.bitcoin.it/wiki/Technical_background_of_version_1_Bitcoin_addresses
    [<TestMethod>]
    member this.TestGenerateBitcoinAddressRecordFromWif () =
        result {
            let! address = BitcoinAddress.GenerateBitcoinAddressRecordFromPrivateKeyWIF true "Kx45GeUBSMPReYQwgXiKhG9FzNXrnCeutJp4yjTd5kKxCitadm3C"    
            let expectedHex = "18e14a7b6a307f426a94f8114701e7c8e774e7f9a47e2c2035db29a206321725"
            Assert.AreEqual(expectedHex, address.PrivateKeyHex)

            let expectedPubKeyX = "50863ad64a87ae8a2fe83c1af1a8403cb53f53e486d8511dad8a04887e5b2352"
            let! actualPubKeyX = Encoding.ByteArrayToHexString address.Metadata.publicKeyX
            Assert.AreEqual(expectedPubKeyX, actualPubKeyX)
            let expectedPubKeyY = "2cd470243453a299fa9e77237716103abc11a1df38855ed6f2ee187e9c582ba6"
            let! actualPubKeyY = Encoding.ByteArrayToHexString address.Metadata.publicKeyY
            Assert.AreEqual(expectedPubKeyY, actualPubKeyY)
            let expectedFullKey = "0450863ad64a87ae8a2fe83c1af1a8403cb53f53e486d8511dad8a04887e5b23522cd470243453a299fa9e77237716103abc11a1df38855ed6f2ee187e9c582ba6"
            Assert.AreEqual(expectedFullKey, address.PublicKeyFull)
            let expectedCompressedKey = "0250863ad64a87ae8a2fe83c1af1a8403cb53f53e486d8511dad8a04887e5b2352"
            Assert.AreEqual(expectedCompressedKey, address.PublicKeyCompressed)
            let expectedCompressedKeySha = "0b7c28c9b7290c98d7438e70b3d3f7c848fbd7d1dc194ff83f4f7cc9b1378e98"
            let! actualCompressedKeySha = Encoding.ByteArrayToHexString address.Metadata.publicKeySha256
            Assert.AreEqual(expectedCompressedKeySha, actualCompressedKeySha)
            let expectedCompressedKeyShaRipe160 = "f54a5851e9372b87810a8e60cdd2e7cfd80b6e31"
            let! actualCompressedKeyShaRipe160 = Encoding.ByteArrayToHexString address.Metadata.publicKeySha256Ripe
            Assert.AreEqual(expectedCompressedKeyShaRipe160, actualCompressedKeyShaRipe160)
            Assert.AreEqual(true, address.Metadata.isMainNetwork)

            let expectedPayToPublicKeyHashWNetwork = "00f54a5851e9372b87810a8e60cdd2e7cfd80b6e31"
            let! actualPayToPublicKeyHashWNetwork = Encoding.ByteArrayToHexString address.Metadata.p2pkh_publicKeyWithNetworkByte
            Assert.AreEqual(expectedPayToPublicKeyHashWNetwork, actualPayToPublicKeyHashWNetwork)
            let expectedPayToPublicKeyHashNetworkChecksum = "c7f18fe8"
            let! actualPayToPublicKeyHashNetworkChecksum = Encoding.ByteArrayToHexString address.Metadata.p2pkh_checksum
            Assert.AreEqual(expectedPayToPublicKeyHashNetworkChecksum, actualPayToPublicKeyHashNetworkChecksum)
            let expectedPayToPublicKeyHashWNetworkChecksum = "00f54a5851e9372b87810a8e60cdd2e7cfd80b6e31c7f18fe8"
            let! actualPayToPublicKeyHashWNetworkChecksum = Encoding.ByteArrayToHexString address.Metadata.p2pkh_addressWithChecksum
            Assert.AreEqual(expectedPayToPublicKeyHashWNetworkChecksum, actualPayToPublicKeyHashWNetworkChecksum)
            let expectedPayToPublicKeyHashAddress = "1PMycacnJaSqwwJqjawXBErnLsZ7RkXUAs"
            Assert.AreEqual(expectedPayToPublicKeyHashAddress, address.P2PKHAddress)

            let expectedPayToScriptHashInitialValue = "0014f54a5851e9372b87810a8e60cdd2e7cfd80b6e31"
            let! actualPayToScriptHashInitialValue = Encoding.ByteArrayToHexString address.Metadata.p2sh_init
            Assert.AreEqual(expectedPayToScriptHashInitialValue, actualPayToScriptHashInitialValue)
            let expectedPayToScripthashWithoutChecksum = "0570b4065d87d8ba1bbf82e82227a0d04a36a57c80"
            let! actualPayToScripthashWithoutChecksum = Encoding.ByteArrayToHexString address.Metadata.p2sh_addressWithoutChecksum
            Assert.AreEqual(expectedPayToScripthashWithoutChecksum, actualPayToScripthashWithoutChecksum)
            let expectedPayToScriptHashNetworkChecksum = "e45d708e"
            let! actualPayToScriptHashNetworkChecksum = Encoding.ByteArrayToHexString address.Metadata.p2sh_checksum
            Assert.AreEqual(expectedPayToScriptHashNetworkChecksum, actualPayToScriptHashNetworkChecksum)
            let expectedPayToScriptHashWNetworkChecksum = "0570b4065d87d8ba1bbf82e82227a0d04a36a57c80e45d708e"
            let! actualPayToScriptHashWNetworkChecksum = Encoding.ByteArrayToHexString address.Metadata.p2sh_addressWithChecksum
            Assert.AreEqual(expectedPayToScriptHashWNetworkChecksum, actualPayToScriptHashWNetworkChecksum)
            let expectedPayToScriptHash = "3BxwGNjvG4CP14tAZodgYyZ7UTjruYDyAM"
            Assert.AreEqual(expectedPayToScriptHash, address.P2SHAddress)
        } |> TestHelper.FailOnError

    // Testing address generation based on https://www.freecodecamp.org/news/how-to-create-a-bitcoin-wallet-address-from-a-private-key-eca3ddd9c05f/
    [<TestMethod>]
     member this.TestP2PKHAddressGeneration () =
        result {
            let! address = BitcoinAddress.GenerateBitcoinAddressRecordFromPrivateKeyHex true "60cf347dbc59d31c1358c8e5cf5e45b822ab85b79cb32a9f3d98184779a9efc2"
            let! pubKey = Encoding.ByteArrayToHexString address.Metadata.publicKeyX
            Assert.AreEqual("1e7bcc70c72770dbb72fea022e8a6d07f814d2ebe4de9ae3f7af75bf706902a7", pubKey)
            let! pubKeyX = Encoding.ByteArrayToHexString address.Metadata.publicKeyY
            Assert.AreEqual("b73ff919898c836396a6b0c96812c3213b99372050853bd1678da0ead14487d7", pubKeyX)
            Assert.AreEqual("041e7bcc70c72770dbb72fea022e8a6d07f814d2ebe4de9ae3f7af75bf706902a7b73ff919898c836396a6b0c96812c3213b99372050853bd1678da0ead14487d7", address.PublicKeyFull)
            Assert.AreEqual("031e7bcc70c72770dbb72fea022e8a6d07f814d2ebe4de9ae3f7af75bf706902a7", address.PublicKeyCompressed)
            let! sha256Ripe = Encoding.ByteArrayToHexString address.Metadata.publicKeySha256Ripe
            Assert.AreEqual("453233600a96384bb8d73d400984117ac84d7e8b", sha256Ripe)
            let! checksum = Encoding.ByteArrayToHexString address.Metadata.p2pkh_checksum
            Assert.AreEqual("512f43c4", checksum)
            Assert.AreEqual("17JsmEygbbEUEpvt4PFtYaTeSqfb9ki1F1", address.P2PKHAddress)
        } |> TestHelper.FailOnError

    // Testing Pay To Script Hash address generation based on https://medium.com/coinmonks/how-to-generate-a-bitcoin-address-step-by-step-9d7fcbf1ad0b
    [<TestMethod>]
     member this.TestP2SHAddressGeneration () =
        result {
            let! address = BitcoinAddress.GenerateBitcoinAddressRecordFromPrivateKeyHex true "a966eb6058f8ec9f47074a2faadd3dab42e2c60ed05bc34d39d6c0e1d32b8bdf"
            Assert.AreEqual("023cba1f4d12d1ce0bced725373769b2262c6daa97be6a0588cfec8ce1a5f0bd09", address.PublicKeyCompressed)
            let! addressWithoutChecksum = Encoding.ByteArrayToHexString address.Metadata.p2sh_addressWithoutChecksum
            Assert.AreEqual("051d521dcf4983772b3c1e6ef937103ebdfaa1ad77", addressWithoutChecksum)
            let! checksum = Encoding.ByteArrayToHexString address.Metadata.p2sh_checksum
            Assert.AreEqual("c41d25f2", checksum)
            let! addressWithChecksum = Encoding.ByteArrayToHexString address.Metadata.p2sh_addressWithChecksum
            Assert.AreEqual("051d521dcf4983772b3c1e6ef937103ebdfaa1ad77c41d25f2", addressWithChecksum)
            Assert.AreEqual("34N3tf5m5rdNhW5zpTXNEJucHviFEa8KEq", address.P2SHAddress)
        } |> TestHelper.FailOnError
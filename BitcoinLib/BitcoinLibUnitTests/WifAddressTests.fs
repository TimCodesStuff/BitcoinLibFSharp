module WifAddressTests

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type WifAddressTests () =
    
    [<TestMethod>]
    member this.TestDecomposeWifAddress () =
        let wifKey = "Kx45GeUBSMPReYQwgXiKhG9FzNXrnCeutJp4yjTd5kKxCitadm3C"
        let decomposedKey = WifKey.DecomposeWifKey true wifKey
        
        // Main Network byte is 0x80
        Assert.AreEqual(decomposedKey.NetworkHex, "80")
        Assert.AreEqual(decomposedKey.PublicKey, "18e14a7b6a307f426a94f8114701e7c8e774e7f9a47e2c2035db29a206321725")
        Assert.AreEqual(decomposedKey.Checksum, "281938ff")

    [<TestMethod>]
    member this.TestValidWifChecksum () =
        let keyWithGoodChecksum = "Kx45GeUBSMPReYQwgXiKhG9FzNXrnCeutJp4yjTd5kKxCitadm3C"
        Assert.IsTrue(WifKey.IsWifChecksumValid true keyWithGoodChecksum)
    
    [<TestMethod>]
    member this.TestInvalidWifChecksum () =
        // Changed last character from "C" to "D" to make checksum invalid.
        let keyWithBadChecksum = "Kx45GeUBSMPReYQwgXiKhG9FzNXrnCeutJp4yjTd5kKxCitadm3D"
        Assert.IsFalse(WifKey.IsWifChecksumValid true keyWithBadChecksum)
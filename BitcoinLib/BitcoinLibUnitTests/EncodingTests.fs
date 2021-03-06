﻿module EncodingTests

open Microsoft.VisualStudio.TestTools.UnitTesting
open Result

[<TestClass>]
type EncodingTests () =
    let comparisonFunc = Array.compareWith(fun x y -> if x = y then 0 else 1)

    [<TestMethod>]
    member this.TestKnownGoodHex () =
        let knownGoodHex = "0123456789ABCDEF"
        Encoding.HexStringToByteArray knownGoodHex
        |> function
        | Ok _ -> ()
        | Error m -> Assert.Fail m

    [<TestMethod>]
    member this.TestKnownBadHex () =
        let knownBadHex = "123456789ABCDEFG"
        Encoding.HexStringToByteArray knownBadHex
        |> function
        | Ok _ -> Assert.Fail "Hex strings should not be accepted with invalid characters."
        | Error _ -> ()

    [<TestMethod>]
    member this.TestHexStringToByteArray () =
        let expected = [| byte(24); byte(225); byte(74); byte(123); byte(106); byte(48); byte(127); byte(66); byte(106); byte(148); byte(248); byte(17); byte(71); byte(1); byte(231); byte(200); byte(231); byte(116); byte(231); byte(249); byte(164); byte(126); byte(44); byte(32); byte(53); byte(219); byte(41); byte(162); byte(6); byte(50); byte(23); byte(37); |]
        result {
            let! actual = Encoding.HexStringToByteArray "18e14a7b6a307f426a94f8114701e7c8e774e7f9a47e2c2035db29a206321725"
            return comparisonFunc expected actual
        } |> function
        | Ok result -> Assert.AreEqual(result, 0)
        | Error m -> Assert.Fail m
        
        
    [<TestMethod>]
    member this.TestByteArrayToHexString () =
        result {
            let expected = "18e14a7b6a307f426a94f8114701e7c8e774e7f9a47e2c2035db29a206321725"
            let byteArray = [| byte(24); byte(225); byte(74); byte(123); byte(106); byte(48); byte(127); byte(66); byte(106); byte(148); byte(248); byte(17); byte(71); byte(1); byte(231); byte(200); byte(231); byte(116); byte(231); byte(249); byte(164); byte(126); byte(44); byte(32); byte(53); byte(219); byte(41); byte(162); byte(6); byte(50); byte(23); byte(37); |]
            let! actual = Encoding.ByteArrayToHexString byteArray
            Assert.AreEqual(expected, actual)
        } |> TestHelper.FailOnError

    [<TestMethod>]
    member this.TestByteArrayToHexStringWithDashes () =
        result {
            let expected = "18-e1-4a-7b-6a-30-7f-42-6a-94-f8-11-47-01-e7-c8-e7-74-e7-f9-a4-7e-2c-20-35-db-29-a2-06-32-17-25"
            let byteArray = [| byte(24); byte(225); byte(74); byte(123); byte(106); byte(48); byte(127); byte(66); byte(106); byte(148); byte(248); byte(17); byte(71); byte(1); byte(231); byte(200); byte(231); byte(116); byte(231); byte(249); byte(164); byte(126); byte(44); byte(32); byte(53); byte(219); byte(41); byte(162); byte(6); byte(50); byte(23); byte(37); |]
            let! actual = Encoding.ByteArrayToHexStringWithDashes byteArray
            Assert.AreEqual(expected, actual)
        } |> TestHelper.FailOnError

    [<TestMethod>]
    member this.TestHexToByteToHex () =
        let expected = "18e14a7b6a307f426a94f8114701e7c8e774e7f9a47e2c2035db29a206321725"
        result {
            let! hexAsByte = expected |> Encoding.HexStringToByteArray
            return! hexAsByte |> Encoding.ByteArrayToHexString
        } |> function
        | Result.Ok actual -> Assert.AreEqual(expected, actual)
        | Result.Error m -> Assert.Fail m
        

    [<TestMethod>]
    member this.TestByteArrayToBigInt () =
        let expected = bigint 417417851
        let actual = Encoding.ByteArrayToBigInt [| byte(24); byte(225); byte(74); byte(123); |]
        Assert.AreEqual(expected, actual)

    [<TestMethod>]
    member this.TestBase58Encode () =
        let expected = "dtNnW"
        let actual = Encoding.Base58Encode [| byte(24); byte(225); byte(74); byte(123); |]
        Assert.AreEqual(expected, actual)

    [<TestMethod>]
    member this.TestBase58EncodeWithLeadingZeros () =
        let expected = "11dtNnW"
        let actual = Encoding.Base58Encode [| byte(0); byte(0); byte(24); byte(225); byte(74); byte(123); |]
        Assert.AreEqual(expected, actual)

    [<TestMethod>]
    member this.TestHexToBase58Encode () =
        let expected = "2g82vgrZTviKG5sN1g2VM7FHgHTm16ej4gmr8ECMzab6"
        result {
            let! actual = "18e14a7b6a307f426a94f8114701e7c8e774e7f9a47e2c2035db29a206321725" |> Encoding.HexStringToByteArray
            return Encoding.Base58Encode actual
        } |> function
        | Ok actual -> Assert.AreEqual(expected, actual)
        | Error m -> Assert.Fail m

    [<TestMethod>]
    member this.TestBase58Decode () =
        let expected = [| byte(24); byte(225); byte(74); byte(123); |]
        let actual = Encoding.Base58Decode "dtNnW"
        let result = comparisonFunc expected actual
        Assert.AreEqual(result, 0)

    [<TestMethod>]
    member this.TestBase58DecodeWithLeadingZeros () =
        let expected = [| byte(0); byte(0); byte(24); byte(225); byte(74); byte(123); |]
        let actual = Encoding.Base58Decode "11dtNnW"
        let result = comparisonFunc expected actual
        Assert.AreEqual(result, 0)
module EncodingTests

open BitcoinLib
open Microsoft.VisualStudio.TestTools.UnitTesting
open System


[<TestClass>]
type EncodingTests () =

    [<TestMethod>]
    member this.TestHexStringToByteArray () =
        let comparisonFunc = Array.compareWith(fun x y -> if x = y then 1 else 0)

        let expected = [| byte(24); byte(225); byte(74); byte(123); byte(106); byte(48); byte(127); byte(66); byte(106); byte(148); byte(248); byte(17); byte(71); byte(1); byte(231); byte(200); byte(231); byte(116); byte(231); byte(249); byte(164); byte(126); byte(44); byte(32); byte(53); byte(219); byte(41); byte(162); byte(6); byte(50); byte(23); byte(37); |]
        let actual = Encoding.HexStringToByteArray "18e14a7b6a307f426a94f8114701e7c8e774e7f9a47e2c2035db29a206321725"
        let result = comparisonFunc expected actual
        Assert.AreEqual(result, 1)
        
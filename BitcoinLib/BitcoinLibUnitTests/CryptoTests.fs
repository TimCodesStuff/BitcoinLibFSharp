module CryptoTests

open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type CryptoTests () =
    
    [<TestMethod>]
    member this.TestBadInputRipeMd () =
        Crypto.RipeMD160(null)
        |> function
        | Result.Ok _ -> Assert.Fail "Expected error when calling Crypto.RipeMD160(null)."
        | Result.Error _ -> ()
        
    [<TestMethod>]
    member this.TestGoodInputRipeMd () =
        Crypto.RipeMD160([|byte(171); byte(205)|])
        |> TestHelper.FailOnError
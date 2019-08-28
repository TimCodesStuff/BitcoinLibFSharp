module TestHelper

open Microsoft.VisualStudio.TestTools.UnitTesting

let FailOnError (result : Result<'T, string>) =
    result |> function
            | Error m -> Assert.Fail m
            | Ok _ -> ()
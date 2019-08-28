module Result

type ResultBuilder() =
        member this.Return a = Ok a
        member this.Bind (m, f) = Result.bind f m
        member this.ReturnFrom m = m
        member this.Zero () = Ok ()

let result = ResultBuilder()
namespace FSharp.JLinq.Urls

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

type UrljsonTupleTest(output: ITestOutputHelper) =
        
    [<Fact>]
    member this.``serialize array``() =
        let x = (1,"x")
        let y = Urljson.serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "(1*'x')"

    [<Fact>]
    member this.``deserialize array``() =
        let x = "(1*'x')"
        let y = Urljson.deserialize<int*string> x
        //output.WriteLine(Render.stringify y)
        Should.equal y (1,"x")


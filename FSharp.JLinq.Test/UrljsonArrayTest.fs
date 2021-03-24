namespace FSharp.JLinq.Urls

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

type UrljsonArrayTest(output: ITestOutputHelper) =

    [<Fact>]
    member this.``serialize array``() =
        let x = [|1;2;3|]
        let y = Urljson.serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "(1*2*3)"

    [<Fact>]
    member this.``deserialize array``() =
        let x = "(1*2*3)"
        let y = Urljson.deserialize<int[]> x
        //output.WriteLine(Render.stringify y)
        Should.equal y [|1;2;3|]


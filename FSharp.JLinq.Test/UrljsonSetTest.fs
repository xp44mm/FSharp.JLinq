namespace FSharp.JLinq.Urls

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

type UrljsonSetTest(output: ITestOutputHelper) =

    [<Fact>]
    member this.``serialize set``() =
        let x = set [1;2;3]
        let y = Urljson.serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "(1*2*3)"

    [<Fact>]
    member this.``deserialize set``() =
        let x = "(1*2*3)"
        let y = Urljson.deserialize<Set<int>> x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| set [1;2;3]


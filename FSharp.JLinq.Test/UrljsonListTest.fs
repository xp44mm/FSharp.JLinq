namespace FSharp.JLinq.Urls

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

type UrljsonListTest(output: ITestOutputHelper) =
    
    [<Fact>]
    member this.``serialize list``() =
        let x = [1;2;3]
        let y = Urljson.serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "(1*2*3)"

    [<Fact>]
    member this.``deserialize list``() =
        let x = "(1*2*3)"
        let y = Urljson.deserialize<List<int>> x
        //output.WriteLine(Render.stringify y)
        Should.equal y [1;2;3]


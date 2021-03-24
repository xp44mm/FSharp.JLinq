namespace FSharp.JLinq.Urls

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

type UrljsonMapTest(output: ITestOutputHelper) =

    [<Fact>]
    member this.``serialize map``() =
        let x = Map.ofList [1,"1";2,"2"]
        let y = Urljson.serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "((1*'1')*(2*'2'))"

    [<Fact>]
    member this.``deserialize map``() =
        let x = "((1*'1')*(2*'2'))"
        let y = Urljson.deserialize<Map<int,string>> x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| Map.ofList [1,"1";2,"2"]


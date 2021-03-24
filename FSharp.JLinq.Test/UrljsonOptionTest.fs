namespace FSharp.JLinq.Urls

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit

type UrljsonOptionTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``serialize none``() =
        let x = None
        let y = Urljson.serialize x
        Should.equal y "null"

    [<Fact>]
    member this.``deserialize none``() =
        let x = "null"
        let y = Urljson.deserialize<_ option> x
        //output.WriteLine(Render.stringify y)
        Should.equal y None

    [<Fact>]
    member this.``serialize some``() =
        let x = Some 1
        let y = Urljson.serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "1"

    [<Fact>]
    member this.``deserialize some``() =
        let x = "1"
        let y = Urljson.deserialize<int option> x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| Some 1



namespace FSharp.JLinq.Urls

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit

type UionExample =
| Zero
| OnlyOne of int
| Pair of int * string

type UrljsonUnionTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``serialize zero union case``() =
        let x = Zero
        let y = Urljson.serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "(Zero!null)"

    [<Fact>]
    member this.``deserialize zero union case``() =
        let x = "(Zero!null)"
        let y = Urljson.deserialize<UionExample> x
        //output.WriteLine(Render.stringify y)
        Should.equal y Zero

    [<Fact>]
    member this.``serialize only-one union case``() =
        let x = OnlyOne 1
        let y = Urljson.serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "(OnlyOne!1)"

    [<Fact>]
    member this.``deserialize only-one union case``() =
        let x = "(OnlyOne!1)"
        let y = Urljson.deserialize<UionExample> x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| OnlyOne 1

    [<Fact>]
    member this.``serialize many params union case``() =
        let x = Pair(1,"")
        let y = Urljson.serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "(Pair!(1*''))"

    [<Fact>]
    member this.``deserialize many params union case``() =
        let x = "(Pair!(1*''))"
        let y = Urljson.deserialize<UionExample> x
        Should.equal y <| Pair(1,"")


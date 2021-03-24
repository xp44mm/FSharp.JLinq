namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit
open Newtonsoft.Json.Linq

type UionExample =
| Zero
| OnlyOne of int
| Pair of int * string

type UnionTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``serialize zero union case``() =
        let x = Zero
        let y = ObjectConverter.serialize x
        //output.WriteLine(Render.stringify y)
        should.equal y """{"Zero":null}"""

    [<Fact>]
    member this.``deserialize zero union case``() =
        let x = """{"Zero":null}"""
        let y = ObjectConverter.deserialize<UionExample> x
        //output.WriteLine(Render.stringify y)
        should.equal y Zero

    [<Fact>]
    member this.``serialize only-one union case``() =
        let x = OnlyOne 1
        let y = ObjectConverter.serialize x
        //output.WriteLine(Render.stringify y)
        should.equal y """{"OnlyOne":1}"""

    [<Fact>]
    member this.``deserialize only-one union case``() =
        let x = """{"OnlyOne":1}"""
        let y = ObjectConverter.deserialize<UionExample> x
        //output.WriteLine(Render.stringify y)
        should.equal y <| OnlyOne 1


    [<Fact>]
    member this.``serialize many params union case``() =
        let x = Pair(1,"")
        let y = ObjectConverter.serialize x
        //output.WriteLine(Render.stringify y)
        should.equal y """{"Pair":[1,""]}"""

    [<Fact>]
    member this.``deserialize many params union case``() =
        let x = """{"Pair":[1,""]}"""
        let y = ObjectConverter.deserialize<UionExample> x
        should.equal y <| Pair(1,"")


    [<Fact>]
    member this.``read zero union case``() =
        let x = Zero
        let y = ObjectConverter.read x :?> JObject
        //output.WriteLine(Render.stringify y)
        should.equal y <| JObject [JProperty("Zero",JValue(null:obj))]

    [<Fact>]
    member this.``write zero union case``() =
        let x = JObject [JProperty("Zero",JValue(null:obj))]
        let y = ObjectConverter.write<UionExample> x
        //output.WriteLine(Render.stringify y)
        should.equal y Zero

    [<Fact>]
    member this.``read only-one union case``() =
        let x = OnlyOne 1
        let y = ObjectConverter.read x :?> JObject
        //output.WriteLine(Render.stringify y)
        should.equal y <| JObject [JProperty("OnlyOne",JValue 1)]

    [<Fact>]
    member this.``write only-one union case``() =
        let x = JObject [JProperty("OnlyOne",JValue 1)]
        let y = ObjectConverter.write<UionExample> x
        //output.WriteLine(Render.stringify y)
        should.equal y <| OnlyOne 1

    [<Fact>]
    member this.``read many params union case``() =
        let x = Pair(1,"")
        let y = ObjectConverter.read x :?> JObject
        //output.WriteLine(Render.stringify y)
        should.equal y <| JObject [JProperty("Pair",JArray [JValue 1;JValue ""])]

    [<Fact>]
    member this.``write many params union case``() =
        let x = JObject [JProperty("Pair",JArray [JValue 1;JValue ""])]
        let y = ObjectConverter.write<UionExample> x
        should.equal y <| Pair(1,"")

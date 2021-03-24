namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open Newtonsoft.Json.Linq

type TupleTest(output: ITestOutputHelper) =
        
    [<Fact>]
    member this.``serialize array``() =
        let x = (1,"x")
        let y = ObjectConverter.serialize x
        //output.WriteLine(Render.stringify y)
        should.equal y """[1,"x"]"""

    [<Fact>]
    member this.``deserialize array``() =
        let x = """[1,"x"]"""
        let y = ObjectConverter.deserialize<int*string> x
        //output.WriteLine(Render.stringify y)
        should.equal y (1,"x")

    [<Fact>]
    member this.``read array``() =
        let x = (1,"x")
        let y = ObjectConverter.read x :?> JArray
        //output.WriteLine(Render.stringify y)
        should.equal y <| JArray [JValue 1;JValue "x"]

    [<Fact>]
    member this.``write array``() =
        let x = JArray [JValue 1;JValue "x"]
        let y = ObjectConverter.write<int*string> x
        //output.WriteLine(Render.stringify y)
        should.equal y (1,"x")

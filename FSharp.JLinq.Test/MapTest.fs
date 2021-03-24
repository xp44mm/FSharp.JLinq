namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open Newtonsoft.Json.Linq

type MapTest(output: ITestOutputHelper) =

    [<Fact>]
    member this.``serialize map``() =
        let x = Map [1,"1";2,"2"]
        let y = ObjectConverter.serialize x
        //output.WriteLine(Render.stringify y)
        should.equal y """[[1,"1"],[2,"2"]]"""

    [<Fact>]
    member this.``deserialize map``() =
        let x = """[[1,"1"],[2,"2"]]"""
        let y = ObjectConverter.deserialize<Map<int,string>> x
        //output.WriteLine(Render.stringify y)
        should.equal y <| Map [1,"1";2,"2"]


    [<Fact>]
    member this.``read map``() =
        let x = Map [1,"1";2,"2"]
        let y = ObjectConverter.read x :?> JArray
        //output.WriteLine(Render.stringify y)
        should.equal y 
        <| JArray [JArray [JValue 1;JValue "1"];JArray [JValue 2;JValue "2"]]

    [<Fact>]
    member this.``write map``() =
        let x = JArray [JArray [JValue 1.0;JValue "1"];JArray [JValue 2.0;JValue "2"]]
        let y = ObjectConverter.write<Map<int,string>> x
        //output.WriteLine(Render.stringify y)
        should.equal y (Map [1,"1";2,"2"])



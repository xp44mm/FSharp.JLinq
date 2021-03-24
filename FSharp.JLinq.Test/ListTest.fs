namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open Newtonsoft.Json.Linq

type ListTest(output: ITestOutputHelper) =
    
    [<Fact>]
    member this.``serialize list``() =
        let x = [1;2;3]
        let y = ObjectConverter.serialize x
        //output.WriteLine(Render.stringify y)
        should.equal y "[1,2,3]"

    [<Fact>]
    member this.``deserialize list``() =
        let x = "[1,2,3]"
        let y = ObjectConverter.deserialize<List<int>> x
        //output.WriteLine(Render.stringify y)
        should.equal y [1;2;3]


    [<Fact>]
    member this.``read list``() =
        let x = [1;2;3]
        let y = ObjectConverter.read x :?> JArray
        //output.WriteLine(Render.stringify y)
        should.equal y <| JArray [JValue 1;JValue 2;JValue 3]

    [<Fact>]
    member this.``write list``() =
        let x = JArray [JValue 1;JValue 2;JValue 3]
        let y = ObjectConverter.write<List<int>> x
        //output.WriteLine(Render.stringify y)
        should.equal y [1;2;3]

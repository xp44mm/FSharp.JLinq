namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open Newtonsoft.Json.Linq

type SetTest(output: ITestOutputHelper) =

    [<Fact>]
    member this.``serialize set``() =
        let x = set [1;2;3]
        let y = ObjectConverter.serialize x
        //output.WriteLine(Render.stringify y)
        should.equal y "[1,2,3]"

    [<Fact>]
    member this.``deserialize set``() =
        let x = "[1,2,3]"
        let y = ObjectConverter.deserialize<Set<int>> x
        //output.WriteLine(Render.stringify y)
        should.equal y <| set[1;2;3]

    [<Fact>]
    member this.``read set``() =
        let x = set [1;2;3]
        let y = ObjectConverter.read x :?> JArray
        //output.WriteLine(Render.stringify y)
        should.equal y <| JArray [JValue 1;JValue 2;JValue 3]

    [<Fact>]
    member this.``write set``() =
        let x = JArray [JValue 1;JValue 2;JValue 3]
        let y = ObjectConverter.write<Set<int>> x
        //output.WriteLine(Render.stringify y)
        should.equal y <| set [1;2;3]

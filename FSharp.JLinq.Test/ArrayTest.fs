namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open Newtonsoft.Json.Linq

type ArrayTest(output: ITestOutputHelper) =

    [<Fact>]
    member this.``serialize array``() =
        let x = [|1;2;3|]
        let y = ObjectConverter.serialize x
        //output.WriteLine(Render.stringify y)
        should.equal y "[1,2,3]"

    [<Fact>]
    member this.``deserialize array``() =
        let x = "[1,2,3]"
        let y = ObjectConverter.deserialize<int[]> x
        //output.WriteLine(Render.stringify y)
        should.equal y [|1;2;3|]

    [<Fact>]
    member this.``read array``() =
        let x = [|1;2;3|]
        let y = ObjectConverter.read x :?> JArray

        //output.WriteLine(Render.stringify y)
        should.equal y <| JArray[|JValue 1;JValue 2;JValue 3|]

    [<Fact>]
    member this.``write array``() =
        let x = (JArray [|JValue 1.0;JValue 2.0;JValue 3.0|]:>JToken)
        let y = ObjectConverter.write<int[]> x
        //output.WriteLine(Render.stringify y)
        should.equal y [|1;2;3|]




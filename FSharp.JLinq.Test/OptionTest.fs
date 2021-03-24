namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open Newtonsoft.Json.Linq

type OptionTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``serialize none``() =
        let x = None
        let y = ObjectConverter.serialize x
        should.equal y "null"

    [<Fact>]
    member this.``deserialize none``() =
        let x = "null"
        let y = ObjectConverter.deserialize<_ option> x
        //output.WriteLine(Render.stringify y)
        should.equal y None

    [<Fact>]
    member this.``serialize some``() =
        let x = Some 1
        let y = ObjectConverter.serialize x
        //output.WriteLine(Render.stringify y)
        should.equal y "1"

    [<Fact>]
    member this.``deserialize some``() =
        let x = "1"
        let y = ObjectConverter.deserialize<int option> x
        //output.WriteLine(Render.stringify y)
        should.equal y <| Some 1

    [<Fact>]
    member this.``read none``() =
        let x = None
        let y = ObjectConverter.read<int option> x :?> JValue
        should.equal y <| JValue(null:obj)

    [<Fact>]
    member this.``write none``() =
        let x = JValue(null:obj)
        let y = ObjectConverter.write<int option> x

        //output.WriteLine(Render.stringify y)
        should.equal y None

    [<Fact>]
    member this.``read some``() =
        let x = Some 1
        let y = ObjectConverter.read x :?> JValue
        //output.WriteLine(Render.stringify y)
        should.equal y <| JValue 1

    [<Fact>]
    member this.``write some``() =
        let x = JValue 1
        let y = ObjectConverter.write<int option> x
        //output.WriteLine(Render.stringify y)
        should.equal y <| Some 1





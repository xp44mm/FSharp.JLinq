namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit
open Newtonsoft.Json.Linq

type DBNullConverterTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``serialize DBNull``() =
        let x = DBNull.Value
        let y = ObjectConverter.serialize x
        //output.WriteLine(Render.stringify y)
        should.equal y "null"

    [<Fact>]
    member this.``deserialize DBNull``() =
        let x = "null"
        let y = ObjectConverter.deserialize<DBNull> x
        //output.WriteLine(Render.stringify y)
        should.equal y <| DBNull.Value

    [<Fact>]
    member this.``read DBNull``() =
        let x = DBNull.Value
        let y = ObjectConverter.read x
        //output.WriteLine(Render.stringify y)
        should.equal y <| (JValue(null:obj):>JToken)

    [<Fact>]
    member this.``write DBNull``() =
        let x = JValue(null:obj)
        let y = ObjectConverter.write<DBNull> x
        //output.WriteLine(Render.stringify y)
        should.equal y <| DBNull.Value


namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit
open Newtonsoft.Json.Linq

type DateTimeOffsetConverterTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``serialize DateTimeOffset``() =
        let x = DateTimeOffset(2020,3,31,9,2,18,0,TimeSpan(0,8,0,0,0))
        let y = ObjectConverter.serialize x
        //output.WriteLine(Render.stringify y)
        should.equal y "\"2020-03-31T09:02:18+08:00\""

    [<Fact>]
    member this.``deserialize DateTimeOffset``() =
        let x = "\"2020-03-31T09:02:18+08:00\""
        let y = ObjectConverter.deserialize<DateTimeOffset> x
        //output.WriteLine(Render.stringify y)
        should.equal y <| DateTimeOffset(2020,3,31,9,2,18,0,TimeSpan(0,8,0,0,0))

    [<Fact>]
    member this.``read DateTimeOffset``() =
        let x = DateTimeOffset(2021,2,11,9,2,18,0,TimeSpan(0,8,0,0,0))
        let y = ObjectConverter.read x :?> JValue
        //output.WriteLine(Render.stringify y)
        should.equal y <| JValue x

    [<Fact>]
    member this.``write DateTimeOffset``() =
        let x = DateTimeOffset(2021,2,11,9,2,18,0,TimeSpan(0,8,0,0,0))
        let y = ObjectConverter.write<DateTimeOffset> (JValue x)

        //output.WriteLine(Render.stringify y)
        should.equal y x


namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit
open Newtonsoft.Json.Linq

type TimeSpanConverterTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``serialize DateTimeOffset``() =
        let x = TimeSpan(2, 14, 18)
        let y = ObjectConverter.serialize x
        //output.WriteLine(Render.stringify y)
        should.equal y <| "\"02:14:18\""

    [<Fact>]
    member this.``deserialize DateTimeOffset``() =
        let x = "\"02:14:18\""
        let y = ObjectConverter.deserialize<TimeSpan> x
        //output.WriteLine(Render.stringify y)
        should.equal y <| TimeSpan(2, 14, 18)


    [<Fact>]
    member this.``read DateTimeOffset``() =
        let x = TimeSpan(2, 14, 18)
        let y = ObjectConverter.read x :?> JValue
        //output.WriteLine(Render.stringify y)
        should.equal y <| JValue x

    [<Fact>]
    member this.``DateTimeOffset instantiate``() =
        let x = JValue "02:14:18"
        let y = ObjectConverter.write<TimeSpan> x
        //output.WriteLine(Render.stringify y)
        should.equal y <| TimeSpan(2, 14, 18)


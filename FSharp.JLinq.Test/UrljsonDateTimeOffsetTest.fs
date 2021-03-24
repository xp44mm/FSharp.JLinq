namespace FSharp.JLinq.Urls

open Xunit
open Xunit.Abstractions
open System
open FSharp.xUnit

type UrljsonDateTimeOffsetTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``serialize DateTimeOffset``() =
        let x = DateTimeOffset(2021,2,11,9,2,18,0,TimeSpan(0,8,0,0,0))
        let y = Urljson.serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| "'2021/2/11 9:02:18 +08:00'"

    [<Fact>]
    member this.``deserialize DateTimeOffset``() =
        let x = "'2021/2/11 9:02:18 +08:00'"
        let y = Urljson.deserialize<DateTimeOffset> x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| DateTimeOffset(2021,2,11,9,2,18,0,TimeSpan(0,8,0,0,0))


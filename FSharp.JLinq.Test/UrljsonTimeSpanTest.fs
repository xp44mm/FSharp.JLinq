namespace FSharp.JLinq.Urls

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit

type UrljsonTimeSpanTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``serialize DateTimeOffset``() =
        let x = TimeSpan(2, 14, 18)
        let y = Urljson.serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| "'02:14:18'"

    [<Fact>]
    member this.``deserialize DateTimeOffset``() =
        let x = "'02:14:18'"
        let y = Urljson.deserialize<TimeSpan> x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| TimeSpan(2, 14, 18)


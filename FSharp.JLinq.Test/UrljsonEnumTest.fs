namespace FSharp.JLinq.Urls

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

open System
open System.Reflection
open System.Text.RegularExpressions

type UrljsonEnumTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``serialize enum``() =
        let x = DateTimeKind.Local
        let y = Urljson.serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "'Local'"

    [<Fact>]
    member this.``deserialize enum``() =
        let x = "'Utc'"
        let y = Urljson.deserialize<DateTimeKind> x
        //output.WriteLine(Render.stringify y)
        Should.equal y DateTimeKind.Utc

    [<Fact>]
    member this.``serialize flags``() =
        let x = BindingFlags.Public ||| BindingFlags.NonPublic
        let y = Urljson.serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "('Public'*'NonPublic')"

    [<Fact>]
    member this.``deserialize flags``() =
        let x = "('Public'*'NonPublic')"
        let y = Urljson.deserialize<BindingFlags> x
        //output.WriteLine(Render.stringify y)
        Should.equal y (BindingFlags.Public ||| BindingFlags.NonPublic)

    [<Fact>]
    member this.``serialize zero flags``() =
        let x = RegexOptions.None
        let y = Urljson.serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "('None')"

    [<Fact>]
    member this.``deserialize zero flags``() =
        let x = "('None')"
        let y = Urljson.deserialize<RegexOptions> x
        //output.WriteLine(Render.stringify y)
        Should.equal y RegexOptions.None


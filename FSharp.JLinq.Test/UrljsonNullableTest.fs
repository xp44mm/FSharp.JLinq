namespace FSharp.JLinq.Urls

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit


type UrljsonNullableTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``serialize nullable``() =
        let x = Nullable 3
        let y = Urljson.serialize x
        Should.equal y "3"

    [<Fact>]
    member this.``serialize nullable null``() =
        let x = Nullable ()
        let y = Urljson.serialize x
        Should.equal y "null"

    [<Fact>]
    member this.``deserialize nullable``() =
        let x = "3" 
        let y = Urljson.deserialize<Nullable<int>> x
        Should.equal y <| Nullable 3

    [<Fact>]
    member this.``deserialize nullable null``() =
        let x = "null"
        let y = Urljson.deserialize<Nullable<_>> x
        Should.equal y <| Nullable ()


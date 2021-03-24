namespace FSharp.JLinq.Urls

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit

type UrljsonDBNullTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``serialize DBNull``() =
        let x = DBNull.Value
        let y = Urljson.serialize x
        //output.WriteLine(Render.stringify y)
        should.equal y "null"

    [<Fact>]
    member this.``deserialize DBNull``() =
        let x = "null"
        let y = Urljson.deserialize<DBNull> x
        //output.WriteLine(Render.stringify y)
        should.equal y <| DBNull.Value


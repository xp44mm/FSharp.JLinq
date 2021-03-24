namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit
open Newtonsoft.Json.Linq


type NullableTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``serialize nullable``() =
        let x = Nullable 3
        let y = ObjectConverter.serialize x
        should.equal y "3"

    [<Fact>]
    member this.``serialize nullable null``() =
        let x = Nullable ()
        let y = ObjectConverter.serialize x
        should.equal y "null"

    [<Fact>]
    member this.``deserialize nullable``() =
        let x = "3" 
        let y = ObjectConverter.deserialize<Nullable<int>> x
        should.equal y <| Nullable 3

    [<Fact>]
    member this.``deserialize nullable null``() =
        let x = "null"
        let y = ObjectConverter.deserialize<Nullable<_>> x
        should.equal y <| Nullable ()

    [<Fact>]
    member this.``read nullable``() =
        let x = Nullable 3
        let y = ObjectConverter.read x
        should.equal y (JValue 3 :> JToken)

    [<Fact>]
    member this.``read nullable null``() =
        let x = Nullable ()
        let y = ObjectConverter.read x
        should.equal y (JValue(null:obj) :> JToken)


    [<Fact>]
    member this.``write nullable``() =
        let x = JValue 3.0
        let y = ObjectConverter.write<Nullable<int>> x

        should.equal y <| Nullable 3

    [<Fact>]
    member this.``write nullable null``() =
        let x = JValue(null:obj)
        let y = ObjectConverter.write<Nullable<_>> x

        should.equal y <| Nullable ()


namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit
open Newtonsoft.Json.Linq

type GuidTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``serialize``() =
        let x = Guid("936da01f-9abd-4d9d-80c7-02af85c822a8")
        let y = ObjectConverter.serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "\"936da01f-9abd-4d9d-80c7-02af85c822a8\""

    [<Fact>]
    member this.``deserialize``() =
        let x = "\"936da01f-9abd-4d9d-80c7-02af85c822a8\""
        let y = ObjectConverter.deserialize<Guid> x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| Guid("936da01f-9abd-4d9d-80c7-02af85c822a8")


    [<Fact>]
    member this.``read``() =
        let x = Guid("936da01f-9abd-4d9d-80c7-02af85c822a8")
        let y = ObjectConverter.read x :?> JValue
        //output.WriteLine(Render.stringify y)
        Should.equal y 
        <| JValue x

    [<Fact>]
    member this.``instantiate``() =
        let x = Guid("936da01f-9abd-4d9d-80c7-02af85c822a8")
        let y = ObjectConverter.write<Guid> (JValue x)

        //output.WriteLine(Render.stringify y)
        Should.equal y x



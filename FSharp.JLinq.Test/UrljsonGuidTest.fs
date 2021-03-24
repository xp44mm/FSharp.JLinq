namespace FSharp.JLinq.Urls

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit

type UrljsonGuidTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``serialize``() =
        let x = Guid("936da01f-9abd-4d9d-80c7-02af85c822a8")
        let y = Urljson.serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "'936da01f-9abd-4d9d-80c7-02af85c822a8'"

    [<Fact>]
    member this.``deserialize``() =
        let x = "'936da01f-9abd-4d9d-80c7-02af85c822a8'"
        let y = Urljson.deserialize<Guid> x
        //output.WriteLine(Render.stringify y)
        Should.equal y <| Guid("936da01f-9abd-4d9d-80c7-02af85c822a8")

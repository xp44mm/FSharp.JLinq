namespace FSharp.JLinq.Urls

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

type Person = { name : string; age : int }

type UrljsonRecordTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``serialize record``() =
        let x = { name = "cuisl"; age = 18 }
        let y = Urljson.serialize x
        //output.WriteLine(Render.stringify y)
        Should.equal y "(name!'cuisl'*age!18)"

    [<Fact>]
    member this.``deserialize record``() =
        let x = "(name!'cuisl'*age!18)"
        let y = Urljson.deserialize<Person> x
        //output.WriteLine(Render.stringify y)
        Should.equal y { name = "cuisl"; age = 18 }
        

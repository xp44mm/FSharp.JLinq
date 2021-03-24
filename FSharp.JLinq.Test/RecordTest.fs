namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open Newtonsoft.Json.Linq

type Person = { name : string; age : int }

type RecordTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``serialize record``() =
        let x = { name = "abcdefg"; age = 18 }
        let y = ObjectConverter.serialize x
        //output.WriteLine(Render.stringify y)
        should.equal y """{"name":"abcdefg","age":18}"""

    [<Fact>]
    member this.``deserialize record``() =
        let x = """{"age":18,"name":"abcdefg"}"""
        let y = ObjectConverter.deserialize<Person> x
        //output.WriteLine(Render.stringify y)
        should.equal y { name = "abcdefg"; age = 18 }
        

    [<Fact>]
    member this.``read record``() =
        let x = { name = "abcdefg"; age = 18 }
        let y = ObjectConverter.read x :?> JObject
        //output.WriteLine(Render.stringify y)
        should.equal y <| JObject [JProperty("name",JValue "abcdefg"); JProperty("age", JValue 18)]

    [<Fact>]
    member this.``write record``() =
        let x = JObject [JProperty("name",JValue "abcdefg"); JProperty("age", JValue 18)]
        let y = ObjectConverter.write<Person> x
        //output.WriteLine(Render.stringify y)
        should.equal y { name = "abcdefg"; age = 18 }


        

namespace FSharp.JLinq.Serialization

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit
open FSharp.JLinq.Serialization
open Newtonsoft.Json.Linq

type JsonDriverTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    [<Fact>]
    member this.``empty object``() =
        let x = "{}"
        let y = JsonDriver.parse x
        //show y
        should.equal y (JObject() :> JToken)

    [<Fact>]
    member this.``empty array``() =
        let x = "[]"
        let y = JsonDriver.parse x
        //show y
        should.equal y (JArray() :> JToken)

    [<Fact>]
    member this.``null``() =
        let x = "null"
        let y = JsonDriver.parse x
        //show y
        should.equal y (JValue(null:obj) :> JToken)

    [<Fact>]
    member this.``false``() =
        let x = "false"
        let y = JsonDriver.parse x
        //show y
        should.equal y (JValue(false) :> JToken)

    [<Fact>]
    member this.``true``() =
        let x = "true"
        let y = JsonDriver.parse x
        //show y
        should.equal y (JValue(true) :> JToken)

    [<Fact>]
    member this.``empty string``() =
        let x = String.replicate 2 "\""
        let y = JsonDriver.parse x
        //show y
        should.equal y (JValue("") :> JToken)

    [<Fact>]
    member this.``number``() =
        let x = "0"
        let y = JsonDriver.parse x
        //show y
        should.equal y (JValue(0.0) :> JToken)

    [<Fact>]
    member this.``single field object``() =
        let x = """{"a":0}"""
        let y = JsonDriver.parse x
        //show y
        should.equal y (JObject(JProperty("a",JValue(0.0))):> JToken)

    [<Fact>]
    member this.``many fields object``() =
        let x = """{"a":0,"b":null}"""
        let y = JsonDriver.parse x
        //show y
        should.equal y (JObject(JProperty("a",JValue(0.0)),JProperty("b",JValue(null:obj))) :> JToken)

    [<Fact>]
    member this.``singleton array``() =
        let x = "[0]"
        let y = JsonDriver.parse x
        //show y
        should.equal y (JArray(JValue(0.0)) :> JToken)

    [<Fact>]
    member this.``many elements array``() =
        let x = "[0,1]"
        let y = JsonDriver.parse x
        //show y
        should.equal y (JArray(JValue(0.0),JValue(1.0)) :> JToken)


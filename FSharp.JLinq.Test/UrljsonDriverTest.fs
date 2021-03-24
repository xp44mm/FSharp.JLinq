namespace FSharp.JLinq.Urls

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit
open FSharp.JLinq
open Newtonsoft.Json.Linq

type UrljsonDriverTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    [<Fact>]
    member this.``empty object``() =
        let x = "(!)"
        let y = UrljsonDriver.parse x :?> JObject
        //show y
        should.equal y 
        <| JObject []

    [<Fact>]
    member this.``empty array``() =
        let x = "()"
        let y = UrljsonDriver.parse x :?> JArray
        //show y
        should.equal y 
        <| JArray []

    [<Fact>]
    member this.``null``() =
        let x = "null"
        let y = UrljsonDriver.parse x :?> JValue
        //show y
        should.equal y 
        <| JValue(null:obj)

    [<Fact>]
    member this.``false``() =
        let x = "false"
        let y = UrljsonDriver.parse x :?> JValue
        //show y
        should.equal y 
        <| JValue(false)
        

    [<Fact>]
    member this.``true``() =
        let x = "true"
        let y = UrljsonDriver.parse x :?> JValue
        //show y
        should.equal y 
        <| JValue(true)

    [<Fact>]
    member this.``empty string``() =
        let x = "''"
        let y = UrljsonDriver.parse x :?> JValue
        //show y
        should.equal y 
        <| JValue("")

    [<Fact>]
    member this.``number``() =
        let x = "0"
        let y = UrljsonDriver.parse x :?> JValue
        //show y
        should.equal y 
        <| JValue 0.0

    [<Fact>]
    member this.``single field object``() =
        let x = "(a!0)"
        let y = UrljsonDriver.parse x :?> JObject
        //show y
        should.equal y 
        <| JObject[JProperty("a",JValue 0.0)]

    [<Fact>]
    member this.``many field object``() =
        let x = "(a!0*b!null)"
        let y = UrljsonDriver.parse x :?> JObject
        //show y
        should.equal y 
        <| JObject[JProperty("a",JValue 0.0);JProperty("b",JValue(null:obj));]

    [<Fact>]
    member this.``singleton array``() =
        let x = "(0)"
        let y = UrljsonDriver.parse x :?> JArray
        //show y
        should.equal y 
        <| JArray [JValue 0.0]

    [<Fact>]
    member this.``many elements array``() =
        let x = "(0*1)"
        let y = UrljsonDriver.parse x :?> JArray
        //show y
        should.equal y 
        <| JArray [JValue 0.0;JValue 1.0]


namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit
open FSharp.JLinq
open Newtonsoft.Json.Linq

type JsonRenderTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    [<Fact>]
    member this.``empty object``() =
        let x = JObject()
        let y = JsonSerializer.stringify x
        //show y
        should.equal y "{}" 
            
    [<Fact>]
    member this.``empty array``() =
        let x = JArray ()
        let y = JsonSerializer.stringify x
        //show y
        should.equal y "[]" 

    [<Fact>]
    member this.``null``() =
        let x =  JValue(null:obj)
        let y = JsonSerializer.stringify x
        //show y
        should.equal y "null"

    [<Fact>]
    member this.``false``() =
        let x = JValue false
        let y = JsonSerializer.stringify x
        //show y
        should.equal y "false" 

    [<Fact>]
    member this.``true``() =
        let x = JValue true
        let y = JsonSerializer.stringify x
        //show y
        should.equal y "true" 

    [<Fact>]
    member this.``empty string``() =
        let x = JValue ""
        let y = JsonSerializer.stringify x
        //show y
        should.equal y "\"\""

    [<Fact>]
    member this.``number``() =
        let x = JValue 0.0 
        let y = JsonSerializer.stringify x
        //show y
        should.equal y "0.0"

    [<Fact>]
    member this.``single field object``() =
        let x = JObject[JProperty("a",JValue 0.0)]
        let y = JsonSerializer.stringify x
        //show y
        should.equal y """{"a":0.0}"""

    [<Fact>]
    member this.``many field object``() =
        let x = JObject[JProperty("a",JValue 0.0);JProperty("b",JValue(null:obj))]
        let y = JsonSerializer.stringify x
        //show y
        should.equal y """{"a":0.0,"b":null}"""

    [<Fact>]
    member this.``singleton array``() =
        let x = JArray [JValue 0.0]
        let y = JsonSerializer.stringify x
        //show y
        should.equal y "[0.0]" 

    [<Fact>]
    member this.``many elements array``() =
        let x = JArray [JValue 0.0;JValue 1.0] 
        let y = JsonSerializer.stringify x
        //show y
        should.equal y "[0.0,1.0]"


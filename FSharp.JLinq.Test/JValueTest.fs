namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open System
open FSharp.xUnit
open Newtonsoft.Json.Linq
open Newtonsoft.Json

type JValueTest(output: ITestOutputHelper) =

    [<Fact>]
    member this.``null``() =
        let x = JValue(null:obj)
        let y = JsonConvert.SerializeObject(x,Formatting.None)
        should.equal y "null" 

    [<Fact>]
    member this.``boolean``() =
        let x = JValue(true)
        let y = JsonConvert.SerializeObject(x,Formatting.None)
        should.equal y "true"

    [<Fact>]
    member this.``char``() =
        let x = JValue('a')
        let y = JsonConvert.SerializeObject(x,Formatting.None)
        should.equal y "\"a\""

    [<Fact>]
    member this.``DateTime``() =
        let x = JValue(DateTime.Parse("2021-03-16T11:16:05+08:00"))
        let y = JsonConvert.SerializeObject(x,Formatting.None)
        should.equal y "\"2021-03-16T11:16:05+08:00\""

    [<Fact>]
    member this.``DateTimeOffset``() =
        let x = JValue(DateTimeOffset.Parse("2021-03-16T11:16:05+08:00"))
        let y = JsonConvert.SerializeObject(x,Formatting.None)
        should.equal y "\"2021-03-16T11:16:05+08:00\""

    [<Fact>]
    member this.``Decimal``() =
        //小数使用了EnsureDecimalPlace
        let x = JValue(0M)
        let y = JsonConvert.SerializeObject(x,Formatting.None)
        should.equal y "0.0"

    [<Fact>]
    member this.``Double``() =
        let x = JValue(0.0)
        let y = JsonConvert.SerializeObject(x,Formatting.None)
        should.equal y "0.0"

    [<Fact>]
    member this.``Guid``() =
        let x = JValue(Guid("00000000-0000-0000-0000-000000000000"))
        let y = JsonConvert.SerializeObject(x,Formatting.None)
        should.equal y "\"00000000-0000-0000-0000-000000000000\""

    [<Fact>]
    member this.``Int64``() =
        let x = JValue(0L)
        let y = JsonConvert.SerializeObject(x,Formatting.None)
        should.equal y "0"

    [<Fact>]
    member this.``Single``() =
        let x = JValue(0f)
        let y = JsonConvert.SerializeObject(x,Formatting.None)
        should.equal y "0.0"

    [<Fact>]
    member this.``String``() =
        let x = JValue("")
        let y = JsonConvert.SerializeObject(x,Formatting.None)
        should.equal y "\"\""

    [<Fact>]
    member this.``TimeSpan``() =
        let x = JValue(TimeSpan(0,0,0))
        let y = JsonConvert.SerializeObject(x,Formatting.None)
        should.equal y "\"00:00:00\""

    [<Fact>]
    member this.``UInt64``() =
        let x = JValue(0UL)
        let y = JsonConvert.SerializeObject(x,Formatting.None)
        should.equal y "0"

    [<Fact>]
    member this.``Uri``() =
        let x = JValue(Uri("http://www.contoso.com/"))
        let y = JsonConvert.SerializeObject(x,Formatting.None)
        should.equal y "\"http://www.contoso.com/\""

    [<Fact>]
    member this.``equals null``() =
        let x = JValue(null:obj)
        let y = JValue(null:obj)
        should.equal x y

    [<Fact>]
    member this.``equals prop``() =
        let x = JProperty("a",JValue(null:obj))
        let y = JProperty("a",JValue(null:obj))
        should.equal x y

    [<Fact>]
    member this.``equals prop number``() =
        let x = JProperty("a",JValue(0.0))
        let y = JProperty("a",JValue(0.0))
        should.equal x y


namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open System
open FSharp.xUnit
open Newtonsoft.Json.Linq

type FallbackReaderTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``covert from sbyte test``() =
        let x = 0y
        let y = ObjectConverter.read x :?> JValue
        should.equal y <| JValue x 

    [<Fact>]
    member this.``covert from byte test``() =
        let x = 0uy
        let y = ObjectConverter.read x :?> JValue

        should.equal y <| JValue x 

    [<Fact>]
    member this.``covert from int16 test``() =
        let x = 0s
        let y = ObjectConverter.read x :?> JValue
        should.equal y <| JValue x 

    [<Fact>]
    member this.``covert from uint16 test``() =
        let x = 0us
        let y = ObjectConverter.read x :?> JValue
        should.equal y <| JValue x 

    [<Fact>]
    member this.``covert from int test``() =
        let x = 0
        let y = ObjectConverter.read x :?> JValue
        should.equal y <| JValue x 

    [<Fact>]
    member this.``covert from uint32 test``() =
        let x = 0u
        let y = ObjectConverter.read x :?> JValue
        should.equal y <| JValue x 

    [<Fact>]
    member this.``covert from int64 test``() =
        let x = 0L
        let y = ObjectConverter.read x :?> JValue
        should.equal y <| JValue x 

    [<Fact>]
    member this.``covert from uint64 test``() =
        let x = 0UL
        let y = ObjectConverter.read x :?> JValue
        should.equal y <| JValue x 

    [<Fact>]
    member this.``covert from single test``() =
        let x = 0.1f
        let y = ObjectConverter.read x :?> JValue 
        should.equal y <| JValue x 

    [<Fact>]
    member this.``covert from decimal test``() =
        let x = 0M
        let y = ObjectConverter.read x :?> JValue
        should.equal y <| JValue x 

    [<Fact>]
    member this.``covert from nativeint test``() =
        let x = 0n
        let y = ObjectConverter.read x :?> JValue
        should.equal y <| JValue 0L

    [<Fact>]
    member this.``covert from unativeint test``() =
        let x = 0un
        let y = ObjectConverter.read x :?> JValue
        should.equal y <| JValue 0UL

    [<Fact>]
    member this.``covert from nullable test``() =
        let x0 = Nullable()
        let y0 = ObjectConverter.read x0 :?> JValue
        should.equal y0 <| JValue x0 

        let x = Nullable(3)
        let y = ObjectConverter.read x  :?> JValue
        should.equal y <| JValue x 

    [<Fact>]
    member this.``covert from null test``() =
        let x:obj = null
        let y = ObjectConverter.read x :?> JValue
        should.equal y <| JValue x 

    [<Fact>]
    member this.``covert from char test``() =
        let x = '\t'
        let y = ObjectConverter.read x :?> JValue
        should.equal y <| JValue x 

    [<Fact>]
    member this.``covert from string test``() =
        let x = ""
        let y = ObjectConverter.read x :?> JValue
        should.equal y <| JValue x 

namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open System
open FSharp.xUnit

type FallbackSerializationTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``covert from sbyte test``() =
        let x = 0y
        let y = ObjectConverter.serialize x
        Assert.Equal(y, "0")

    [<Fact>]
    member this.``covert from byte test``() =
        let x = 0uy
        let y = ObjectConverter.serialize x
        Assert.Equal(y,"0")

    [<Fact>]
    member this.``covert from int16 test``() =
        let x = 0s
        let y = ObjectConverter.serialize x
        Assert.Equal(y,"0")

    [<Fact>]
    member this.``covert from uint16 test``() =
        let x = 0us
        let y = ObjectConverter.serialize x
        Assert.Equal(y,"0")

    [<Fact>]
    member this.``covert from int test``() =
        let x = 0
        let y = ObjectConverter.serialize x
        Assert.Equal(y,"0")

    [<Fact>]
    member this.``covert from uint32 test``() =
        let x = 0u
        let y = ObjectConverter.serialize x
        Assert.Equal(y,"0")

    [<Fact>]
    member this.``covert from int64 test``() =
        let x = 0L
        let y = ObjectConverter.serialize x
        Assert.Equal(y,"0")

    [<Fact>]
    member this.``covert from uint64 test``() =
        let x = 0UL
        let y = ObjectConverter.serialize x
        Assert.Equal(y,"0")

    [<Fact>]
    member this.``covert from nativeint test``() =
        let x = 0n
        let y = ObjectConverter.serialize x
        Assert.Equal(y, "0")

    [<Fact>]
    member this.``covert from unativeint test``() =
        let x = 0un
        let y = ObjectConverter.serialize x
        Assert.Equal(y,"0")

    [<Fact>]
    member this.``covert from single test``() =
        let x = 0.1f
        let y = ObjectConverter.serialize x
        Assert.Equal(y,"0.1")

    [<Fact>]
    member this.``covert from decimal test``() =
        let x = 0M
        let y = ObjectConverter.serialize x
        Assert.Equal(y,"0.0")

    [<Fact>]
    member this.``covert from char test``() =
        let x = '\t'
        let y = ObjectConverter.serialize x
        Assert.Equal(y, "\"\\t\"")

    [<Fact>]
    member this.``covert from string test``() =
        let x = ""
        let y = ObjectConverter.serialize x
        Assert.Equal(y, "\"\"")

    [<Fact>]
    member this.``covert from null test``() =
        let ls = null
        let res = ObjectConverter.serialize ls
        Should.equal res "null"

    [<Fact>]
    member this.``covert from nullable test``() =
        let x0 = Nullable()
        let y0 = ObjectConverter.serialize x0
        Assert.Equal(y0,"null")

        let x = Nullable(3)
        let y = ObjectConverter.serialize x
        Assert.Equal(y,"3")


namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open Newtonsoft.Json.Linq

type FallbackWriterTest(output: ITestOutputHelper) =

    [<Fact>]
    member this.``null``() =
        let json = JValue(null:obj)
        let y = ObjectConverter.write<_> json
        Should.equal y null

    [<Fact>]
    member this.``false``() =
        let json = JValue false
        let y = ObjectConverter.write<_> json
        Should.equal y false

    [<Fact>]
    member this.``true``() =
        let json = JValue true
        let y = ObjectConverter.write<_> json
        Should.equal y true

    [<Fact>]
    member this.``string``() =
        let json = JValue ""
        let y = ObjectConverter.write<string> json
        Should.equal y ""

    [<Fact>]
    member this.``char``() =
        let json = JValue "0"
        let y = ObjectConverter.write<char> json
        Should.equal y '0'

    [<Fact>]
    member this.``number sbyte``() =
        let json = JValue 0.0
        let y = ObjectConverter.write<sbyte> json
        Should.equal y 0y

    [<Fact>]
    member this.``number byte``() =
        let x = JValue 0.0
        let y = ObjectConverter.write<_> x
        Assert.Equal(y, 0uy)

    [<Fact>]
    member this.``number int16``() =
        let x = JValue 0.0
        let y = ObjectConverter.write<_> x
        Assert.Equal(y, 0s)

    [<Fact>]
    member this.``number uint16``() =
        let x = JValue 0.0
        let y = ObjectConverter.write<_> x
        Assert.Equal(y, 0us)

    [<Fact>]
    member this.``number int``() =
        let x = JValue 0.0
        let y = ObjectConverter.write<_> x
        Assert.Equal(y, 0)

    [<Fact>]
    member this.``number uint32``() =
        let x = JValue 0.0
        let y = ObjectConverter.write<_> x
        Assert.Equal(y, 0u)

    [<Fact>]
    member this.``number int64``() =
        let x = JValue 0.0
        let y = ObjectConverter.write<_> x
        Assert.Equal(y, 0L)

    [<Fact>]
    member this.``number uint64``() =
        let x = JValue 0.0 
        let y = ObjectConverter.write<_> x
        Assert.Equal(y,0UL)

    [<Fact>]
    member this.``number single``() =
        let x = JValue 0.1
        let y = ObjectConverter.write<_> x 
        Assert.Equal(y, 0.1f)

    [<Fact>]
    member this.``number decimal``() =
        let x = JValue 0.0
        let y = ObjectConverter.write<_> x
        Assert.Equal(y, 0M)

    [<Fact>]
    member this.``number nativeint``() =
        let x = JValue 0.0
        let y = ObjectConverter.write<_> x
        Assert.Equal(y, 0n)

    [<Fact>]
    member this.``number unativeint``() =
        let x = JValue 0.0
        let y = ObjectConverter.write<_> x
        Assert.Equal(y, 0un)

namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open FSharp.xUnit

type FallbackDeserializationTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``empty string``() =
        Assert.Throws<exn>(fun _ ->
            let y = ObjectConverter.deserialize<_> ""
            ()
        )

    [<Fact>]
    member this.``null``() =
        let x = "null"
        let y = ObjectConverter.deserialize<_> x
        Should.equal y null

    [<Fact>]
    member this.``false``() =
        let x = "false"
        let y = ObjectConverter.deserialize<_> x
        Should.equal y false

    [<Fact>]
    member this.``true``() =
        let x = "true"
        let y = ObjectConverter.deserialize<_> x
        Should.equal y true

    [<Fact>]
    member this.``string``() =
        let x = "\"\""
        let y = ObjectConverter.deserialize<string> x
        Should.equal y ""

    [<Fact>]
    member this.``char``() =
        let x = "\"0\""
        let y = ObjectConverter.deserialize<char> x
        Should.equal y '0'

    [<Fact>]
    member this.``number sbyte``() =
        let x = "0"
        let y = ObjectConverter.deserialize<sbyte> x
        Should.equal y 0y

    [<Fact>]
    member this.``number byte``() =
        let x = "0"
        let y = ObjectConverter.deserialize<_> x
        Assert.Equal(y, 0uy)

    [<Fact>]
    member this.``number int16``() =
        let x = "0"
        let y = ObjectConverter.deserialize<_> x
        Assert.Equal(y, 0s)

    [<Fact>]
    member this.``number uint16``() =
        let x = "0"
        let y = ObjectConverter.deserialize<_> x
        Assert.Equal(y, 0us)

    [<Fact>]
    member this.``number int``() =
        let x = "0"
        let y = ObjectConverter.deserialize<_> x
        Assert.Equal(y, 0)

    [<Fact>]
    member this.``number uint32``() =
        let x = "0"
        let y = ObjectConverter.deserialize<_> x
        Assert.Equal(y, 0u)

    [<Fact>]
    member this.``number int64``() =
        let x = "0"
        let y = ObjectConverter.deserialize<_> x
        Assert.Equal(y, 0L)

    [<Fact>]
    member this.``number uint64``() =
        let x = "0"
        let y = ObjectConverter.deserialize<_> x
        Assert.Equal(y,0UL)

    [<Fact>]
    member this.``number single``() =
        let x = "0.1"
        let y = ObjectConverter.deserialize<_> x 
        Assert.Equal(y, 0.1f)

    [<Fact>]
    member this.``number decimal``() =
        let x = "0"
        let y = ObjectConverter.deserialize<_> x
        Assert.Equal(y, 0M)

    [<Fact>]
    member this.``number nativeint``() =
        let x = "0"
        let y = ObjectConverter.deserialize<_> x
        Assert.Equal(y, 0n)

    [<Fact>]
    member this.``number unativeint``() =
        let x = "0"
        let y = ObjectConverter.deserialize<_> x
        Assert.Equal(y, 0un)

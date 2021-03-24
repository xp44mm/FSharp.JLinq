namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit
open Newtonsoft.Json.Linq

type MutableVector2D() =
    let mutable currDX = 0.0
    let mutable currDY = 0.0
    member vec.DX with get() = currDX and set v = currDX <- v
    member vec.DY with get() = currDY and set v = currDY <- v
    member vec.Length
        with get () = sqrt (currDX * currDX + currDY * currDY)

    member vec.Angle
        with get () = atan2 currDY currDX

type ClassTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``ObjectConverter read``() =
        let x = MutableVector2D()
        let y = ObjectConverter.read x :?> JObject
        //output.WriteLine(Render.stringify y)
        should.equal y <| JObject [JProperty("DX",JValue 0.0);JProperty("DY",JValue 0.0)]

    [<Fact>]
    member this.``ObjectConverter write``() =
        let x = JObject [JProperty("DX",JValue 0.0);JProperty("DY",JValue 0.0)]
        let y = ObjectConverter.write<MutableVector2D> x
        //output.WriteLine(Render.stringify y)
        let z = MutableVector2D()
        should.equal (y.DX,y.DY) (z.DX,z.DY)

    [<Fact>]
    member this.``is class``() =
        should.equal typeof<bool>.IsClass       false
        should.equal typeof<sbyte>.IsClass      false
        should.equal typeof<byte>.IsClass       false
        should.equal typeof<int16>.IsClass      false
        should.equal typeof<uint16>.IsClass     false
        should.equal typeof<int>.IsClass        false
        should.equal typeof<uint32>.IsClass     false
        should.equal typeof<int64>.IsClass      false
        should.equal typeof<uint64>.IsClass     false
        should.equal typeof<single>.IsClass     false
        should.equal typeof<float>.IsClass      false
        should.equal typeof<char>.IsClass       false
        should.equal typeof<string>.IsClass     true
        should.equal typeof<decimal>.IsClass    false
        should.equal typeof<nativeint>.IsClass  false
        should.equal typeof<unativeint>.IsClass false

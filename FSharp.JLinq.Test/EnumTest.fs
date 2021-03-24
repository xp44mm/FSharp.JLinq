namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open FSharp.xUnit
open System.Reflection
open System.Text.RegularExpressions
open System
open Newtonsoft.Json.Linq

type EnumTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``serialize enum``() =
        let x = DateTimeKind.Local
        let y = ObjectConverter.serialize x
        
        //output.WriteLine(Render.stringify y)
        should.equal y "\"Local\""

    [<Fact>]
    member this.``deserialize enum``() =
        let x = "\"Utc\""
        let y = ObjectConverter.deserialize<DateTimeKind> x

        //output.WriteLine(Render.stringify y)
        should.equal y DateTimeKind.Utc

    [<Fact>]
    member this.``serialize flags``() =
        let x = BindingFlags.Public ||| BindingFlags.NonPublic
        let y = ObjectConverter.serialize x

        //output.WriteLine(Render.stringify y)
        should.equal y "\"Public,NonPublic\""

    [<Fact>]
    member this.``deserialize flags``() =
        let x = "\"Public,NonPublic\""
        let y = ObjectConverter.deserialize<BindingFlags> x

        //output.WriteLine(Render.stringify y)
        should.equal y (BindingFlags.Public ||| BindingFlags.NonPublic)

    [<Fact>]
    member this.``serialize zero flags``() =
        let x = RegexOptions.None
        let y = ObjectConverter.serialize x

        //output.WriteLine(Render.stringify y)
        should.equal y "\"None\""

    [<Fact>]
    member this.``deserialize zero flags``() =
        let x = "\"None\""
        let y = ObjectConverter.deserialize<RegexOptions> x

        //output.WriteLine(Render.stringify y)
        should.equal y RegexOptions.None

    [<Fact>]
    member this.``read enum``() =
        let x = DateTimeKind.Local
        let y = ObjectConverter.read x :?> JValue

        //output.WriteLine(Render.stringify y)
        should.equal y <| JValue "Local"
    [<Fact>]
    member this.``enum instantiate``() =
        let x = JValue "Local"
        let y = ObjectConverter.write<DateTimeKind> x

        //output.WriteLine(Render.stringify y)
        should.equal y DateTimeKind.Local
         
    [<Fact>]
    member this.``read flags``() =
        let x = BindingFlags.Public ||| BindingFlags.NonPublic
        let y = ObjectConverter.read x :?> JValue
        //output.WriteLine(Render.stringify res)
        should.equal y <| JValue "Public,NonPublic"

    [<Fact>]
    member this.``flags instantiate``() =
        let x = JValue "Public,NonPublic"
        let y = ObjectConverter.write<BindingFlags> x

        //output.WriteLine(Render.stringify y)
        should.equal y (BindingFlags.Public ||| BindingFlags.NonPublic)

    [<Fact>]
    member this.``read zero flags``() =
        let x = RegexOptions.None
        let y = ObjectConverter.read x :?> JValue
        //output.WriteLine(Render.stringify res)
        should.equal y <| JValue "None"

    [<Fact>]
    member this.``zero flags instantiate``() =
        let x = JValue "None"
        let y = ObjectConverter.write<RegexOptions> x

        //output.WriteLine(Render.stringify y)
        should.equal y RegexOptions.None


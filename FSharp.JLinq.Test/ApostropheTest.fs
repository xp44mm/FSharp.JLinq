namespace FSharp.JLinq.Urls

open Xunit
open Xunit.Abstractions
open System
open FSharp.xUnit

type ApostropheTest(output: ITestOutputHelper) =

    [<Fact>]
    member this.``parseLiteral empty``() =
        let x = "''"
        let y = Apostrophe.parseLiteral x
        should.equal y ""

    [<Fact>]
    member this.``toLiteral empty``() =
        let x = ""
        let y = Apostrophe.toLiteral x
        should.equal y "''"

    [<Fact>]
    member this.``parseLiteral apostrophe``() =
        let x = "'~''"
        let y = Apostrophe.parseLiteral x
        should.equal y "'"

    [<Fact>]
    member this.``toLiteral apostrophe``() =
        let x = "'"
        let y = Apostrophe.toLiteral x
        should.equal y @"'~''"

    [<Fact>]
    member this.``parseLiteral Escape Characters``() =
        let x = "'~~~'~b~f~n~r~t~v'"
        let y = Apostrophe.parseLiteral x
        should.equal y <| String [|'~';'\'';'\b';'\f';'\n';'\r';'\t';'\v'|]

    [<Fact>]
    member this.``toLiteral Escape Characters``() =
        let x = String [|'~';'\'';'\b';'\f';'\n';'\r';'\t';'\v'|]
        let y = Apostrophe.toLiteral x
        should.equal y "'~~~'~b~f~n~r~t~v'"

    [<Fact>]
    member this.``parseLiteral Unicode character``() =
        let x = "'~01'"
        let y = Apostrophe.parseLiteral x
        should.equal y "\u0001"

    [<Fact>]
    member this.``toLiteral Unicode character``() =
        let x = "\u0001"
        let y = Apostrophe.toLiteral x
        should.equal y "'~01'"

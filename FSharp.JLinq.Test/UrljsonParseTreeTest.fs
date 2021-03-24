namespace FSharp.JLinq.Urls

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit

open FSharpCompiler.Parsing
open FSharp.JLinq.Urls

type UrljsonParseTreeTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    let parse(text:string) =
        let tokens =
            UrljsonToken.tokenize text
            |> UrljsonAnylizer.normalize
        UrljsonDriver.parser.parse(tokens, fun tok ->tok.tag)

    
    [<Fact>]
    member this.``empty object``() =
        let x = "(!)"
        let y = parse x
        //show y
        Should.equal y 
        <| Interior("value",[Interior("object",[Terminal EMPTY_OBJECT])])

    [<Fact>]
    member this.``empty array``() =
        let x = "()"
        let y = parse x
        //show y
        Should.equal y 
        <| Interior("value",[Interior("array",[Terminal LEFT_PAREN;Terminal RIGHT_PAREN])])

    [<Fact>]
    member this.``null``() =
        let x = "null"
        let y = parse x
        //show y
        Should.equal y 
        <| Interior("value",[Terminal NULL])

    [<Fact>]
    member this.``false``() =
        let x = "false"
        let y = parse x
        //show y
        Should.equal y 
        <| Interior("value",[Terminal FALSE])

    [<Fact>]
    member this.``true``() =
        let x = "true"
        let y = parse x
        //show y
        Should.equal y 
        <| Interior("value",[Terminal TRUE])

    [<Fact>]
    member this.``empty string``() =
        let x = "''"
        let y = parse x
        //show y
        Should.equal y 
        <| Interior("value",[Terminal(STRING "")])

    [<Fact>]
    member this.``number``() =
        let x = "0"
        let y = parse x
        //show y
        Should.equal y 
        <| Interior("value",[Terminal(NUMBER 0.0)])

    [<Fact>]
    member this.``many fields object``() =
        let x = "(a!0*'b!'!null)"
        let y = parse x
        //show y
        Should.equal y 
        <| Interior("value",[Interior("object",[Terminal LEFT_PAREN;Interior("fields",[Interior("fields",[Interior("field",[Terminal(KEY "a");Terminal EXCLAM;Interior("value",[Terminal(NUMBER 0.0)])])]);Terminal ASTERISK;Interior("field",[Terminal(KEY "b!");Terminal EXCLAM;Interior("value",[Terminal NULL])])]);Terminal RIGHT_PAREN])])
    [<Fact>]
    member this.``many elements array``() =
        let x = "(0*1)"
        let y = parse x
        //show y
        Should.equal y 
        <| Interior("value",[Interior("array",[Terminal LEFT_PAREN;Interior("values",[Interior("values",[Interior("value",[Terminal(NUMBER 0.0)])]);Terminal ASTERISK;Interior("value",[Terminal(NUMBER 1.0)])]);Terminal RIGHT_PAREN])])


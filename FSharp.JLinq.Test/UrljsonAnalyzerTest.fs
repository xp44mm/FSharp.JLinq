namespace FSharp.JLinq.Urls

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit
open FSharp.JLinq.Urls

type UrljsonAnalyzerTest(output: ITestOutputHelper) =
    [<Fact>]
    member this.``key``() =
        let x = "('xyz'!''*null!null*false!false*true!true*key!0)"
        let toks = UrljsonToken.tokenize x |> UrljsonAnylizer.normalize |> Seq.toList

        //output.WriteLine(Render.stringify toks)
        let res = [LEFT_PAREN;KEY "xyz";EXCLAM;STRING "";ASTERISK;KEY "null";EXCLAM;NULL;ASTERISK;KEY "false";EXCLAM;FALSE;ASTERISK;KEY "true";EXCLAM;TRUE;ASTERISK;KEY "key";EXCLAM;NUMBER 0.0;RIGHT_PAREN]
        Should.equal toks res
    [<Fact>]
    member this.``empty object``() =
        let x = "(('xyz'*(!))"
        let toks = UrljsonToken.tokenize x |> UrljsonAnylizer.normalize |> Seq.toList

        //output.WriteLine(Render.stringify toks)
        let res = [LEFT_PAREN;LEFT_PAREN;STRING "xyz";ASTERISK;EMPTY_OBJECT;RIGHT_PAREN]
        Should.equal toks res

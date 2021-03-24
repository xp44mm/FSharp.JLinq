namespace FSharp.JLinq.Urls

open Xunit
open Xunit.Abstractions
open System
open FSharp.Literals
open FSharp.xUnit
open FSharp.JLinq.Urls

type UrljsonToken(output:ITestOutputHelper) =
    let scan =
        UrljsonToken.tokenize
        >> UrljsonAnylizer.normalize

    [<Fact>]
    member this.json() =
        let x = """(inlet!(SO2!0*SO3!1)*eobj!(!)*earr!()*arr!(10*11*12)*dirtyLeakage!'Yes! "it~'s great*"')"""
        let toks = scan x |> Seq.toList

        output.WriteLine(Render.stringify toks)
        let res = [LEFT_PAREN;KEY "inlet";EXCLAM;LEFT_PAREN;KEY "SO2";EXCLAM;NUMBER 0.0;ASTERISK;KEY "SO3";EXCLAM;NUMBER 1.0;RIGHT_PAREN;ASTERISK;KEY "eobj";EXCLAM;EMPTY_OBJECT;ASTERISK;KEY "earr";EXCLAM;LEFT_PAREN;RIGHT_PAREN;ASTERISK;KEY "arr";EXCLAM;LEFT_PAREN;NUMBER 10.0;ASTERISK;NUMBER 11.0;ASTERISK;NUMBER 12.0;RIGHT_PAREN;ASTERISK;KEY "dirtyLeakage";EXCLAM;STRING "Yes! \"it's great*\"";RIGHT_PAREN]
        should.equal res toks

    [<Fact>]
    member this.jsonEmptyArray() =
        let x = " ( )"
        let toks = scan x |> Seq.toList

        //output.WriteLine(Render.stringify toks)
        let res = [LEFT_PAREN;RIGHT_PAREN]
        should.equal toks res

    [<Fact>]
    member this.``simple tokens``() =
        let x = "((null*true*false)*(!)*($!-1.1e2*x~!''))"
        let toks = scan x |> Seq.toList

        //output.WriteLine(Render.stringify toks)
        let res = [LEFT_PAREN;LEFT_PAREN;NULL;ASTERISK;TRUE;ASTERISK;FALSE;RIGHT_PAREN;ASTERISK;EMPTY_OBJECT;ASTERISK;LEFT_PAREN;KEY "$";EXCLAM;NUMBER -110.0;ASTERISK;KEY "x~";EXCLAM;STRING "";RIGHT_PAREN;RIGHT_PAREN]
        should.equal res toks

    [<Fact>]
    member this.``string tokens``() =
        let x = """''*'~''*'"'*''*''"""
        let toks = scan x |> Seq.toList

        //output.WriteLine(Render.stringify toks)
        let res = [STRING "";ASTERISK;STRING "'";ASTERISK;STRING "\"";ASTERISK;STRING "";ASTERISK;STRING ""]
        should.equal toks res

    [<Fact>]
    member this.``number tokens``() =
        let x = """0*0.0*+1*1E2*0.5e-3"""
        let toks = scan x |> Seq.toList

        //output.WriteLine(Render.stringify toks)
        let res = [NUMBER 0.0;ASTERISK;NUMBER 0.0;ASTERISK;NUMBER 1.0;ASTERISK;NUMBER 100.0;ASTERISK;NUMBER 0.0005]
        should.equal toks res

    [<Fact>]
    member this.``key tokens``() =
        let x = "id!0*$!1*x~!2*x1!3*汉字!4"
        let toks = scan x |> Seq.toList

        //output.WriteLine(Render.stringify toks)
        let res = [KEY "id";EXCLAM;NUMBER 0.0;ASTERISK;KEY "$";EXCLAM;NUMBER 1.0;ASTERISK;KEY "x~";EXCLAM;NUMBER 2.0;ASTERISK;KEY "x1";EXCLAM;NUMBER 3.0;ASTERISK;KEY "汉字";EXCLAM;NUMBER 4.0]
        should.equal toks res



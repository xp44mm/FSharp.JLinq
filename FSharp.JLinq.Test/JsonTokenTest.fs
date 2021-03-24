namespace FSharp.JLinq.Serialization

open System
open Xunit
open Xunit.Abstractions
open FSharp.Literals
open FSharp.xUnit
open FSharp.JLinq.Serialization

type JsonTokenTest(output: ITestOutputHelper) =

    [<Fact>]
    member this.``simple element test``() =
        let x = "[{\"x\":1},null,true,false]"
        let y = JsonToken.tokenize x |> Seq.toList
        //output.WriteLine(Render.stringify y)
        should.equal y [
            LEFT_BRACK;LEFT_BRACE;STRING "x";COLON;NUMBER 1.0;
            RIGHT_BRACE;COMMA;NULL;COMMA;TRUE;COMMA;FALSE;RIGHT_BRACK]

    [<Fact>]
    member this.``number element test``() =
        let x = "[1,-1,+1,1.1,3e2,3.0E3]"
        let y = JsonToken.tokenize x |> Seq.toList
        //output.WriteLine(Render.stringify y)
        should.equal y [LEFT_BRACK;
            NUMBER 1.0;COMMA;
            NUMBER -1.0;COMMA;
            NUMBER 1.0;COMMA;
            NUMBER 1.1;COMMA;
            NUMBER 3e2;COMMA;
            NUMBER 3.0E3;RIGHT_BRACK]

    [<Fact>]
    member this.``string element test``() =
        let x = """["","\"","\\","\t"]"""
        let y = JsonToken.tokenize x |> Seq.toList
        //output.WriteLine(Render.stringify y)
        should.equal y [LEFT_BRACK;
            STRING "";COMMA;
            STRING "\"";COMMA;
            STRING "\\";COMMA;
            STRING "\t";RIGHT_BRACK]


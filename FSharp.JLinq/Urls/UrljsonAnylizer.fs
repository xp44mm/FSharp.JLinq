module FSharp.JLinq.Urls.UrljsonAnylizer

open FSharpCompiler.Analyzing
open System

let analyzer = LexicalAnalyzer(UrljsonDFA.dtran,UrljsonDFA.finalLexemes)

let normalize (tokens:seq<UrljsonToken>) =
    analyzer.split(tokens,fun tok -> tok.tag)
    |> Seq.collect(function
        | 0, [STRING s] -> [KEY s]
        | 1, [STRING s] -> [STRING s]
        | 2, [KEY k] -> [KEY k]
        | 3, [KEY k] ->
            if k = "null" then
                NULL
            elif k = "false" then
                FALSE
            elif k = "true" then
                TRUE
            else
                NUMBER(Double.Parse k)
            |> List.singleton

        | 4, [LEFT_PAREN; EXCLAM; RIGHT_PAREN] -> [EMPTY_OBJECT]
        | _, ls -> ls
    )

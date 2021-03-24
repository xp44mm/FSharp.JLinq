module FSharp.JLinq.Urls.UrljsonDriver

open FSharpCompiler.Parsing

let parser =
    SyntacticParser(
        UrljsonParsingTable.rules,
        UrljsonParsingTable.kernelSymbols,
        UrljsonParsingTable.parsingTable)

let parseTokens (tokens:seq<UrljsonToken>) =
    let parsingTree = parser.parse(tokens, fun tok -> tok.tag)
    UrljsonTranslation.translateValue parsingTree

let parse(text:string) =
    if System.String.IsNullOrEmpty(text) then
        failwith "text is empty."
    else
        let tokens = 
            UrljsonToken.tokenize text
            |> UrljsonAnylizer.normalize
        parseTokens tokens

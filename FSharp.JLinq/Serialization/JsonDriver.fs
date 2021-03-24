module FSharp.JLinq.Serialization.JsonDriver

open FSharpCompiler.Parsing

let parser =
    SyntacticParser(
        JsonParsingTable.rules,
        JsonParsingTable.kernelSymbols,
        JsonParsingTable.parsingTable)

let parseTokens tokens =
    let parsingTree = parser.parse(tokens, fun(tok:JsonToken) -> tok.tag)
    JsonTranslation.translateValue parsingTree

let parse(text:string) =
    let tokens = JsonToken.tokenize text
    parseTokens tokens

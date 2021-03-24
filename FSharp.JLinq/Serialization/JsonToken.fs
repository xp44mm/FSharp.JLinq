namespace FSharp.JLinq.Serialization

open System
open FSharp.Idioms.StringOps

type JsonToken = 
| COMMA
| COLON
| RIGHT_BRACK
| LEFT_BRACK
| RIGHT_BRACE
| LEFT_BRACE
| NULL
| FALSE
| TRUE
| STRING of string
| NUMBER of float

    member this.tag = 
        match this with
        | COMMA       -> ","
        | COLON       -> ":"
        | LEFT_BRACK  -> "["
        | RIGHT_BRACK -> "]"
        | LEFT_BRACE  -> "{"
        | RIGHT_BRACE -> "}"
        | NULL        -> "NULL"
        | FALSE       -> "FALSE"
        | TRUE        -> "TRUE"
        | STRING      _ -> "STRING"
        | NUMBER      _ -> "NUMBER"

    static member tokenize(inp:string) =
        let rec loop (inp:string) =
            seq {
                match inp with
                | "" -> ()
        
                | Prefix @"\s+" (_,rest) -> 
                    yield! loop rest

                | PrefixChar '{' rest ->
                    yield LEFT_BRACE
                    yield! loop rest

                | PrefixChar '}' rest ->
                    yield RIGHT_BRACE
                    yield! loop rest

                | PrefixChar '[' rest ->
                    yield LEFT_BRACK
                    yield! loop rest

                | PrefixChar ']' rest ->
                    yield RIGHT_BRACK
                    yield! loop rest

                | PrefixChar ':' rest ->
                    yield COLON
                    yield! loop rest

                | PrefixChar ',' rest ->
                    yield COMMA
                    yield! loop rest

                | Prefix @"null\b" (_,rest) ->
                    yield NULL
                    yield! loop rest
        
                | Prefix @"true\b" (_,rest) ->
                    yield TRUE
                    yield! loop rest
        
                | Prefix @"false\b" (_,rest) ->
                    yield FALSE
                    yield! loop rest
        
                | Prefix """(?:"(\\[/'"bfnrt\\]|\\u[0-9A-Fa-f]{4}|[^\\"])*")""" (lexeme,rest) ->
                    yield  STRING(StringLiteral.parseLiteral lexeme)
                    yield! loop rest

                | Prefix @"[-+]?\d+(\.\d+)?([eE][-+]?\d+)?" (lexeme,rest) ->
                    yield NUMBER(Double.Parse lexeme)
                    yield! loop rest

                | never -> failwith never
            }
        
        loop inp

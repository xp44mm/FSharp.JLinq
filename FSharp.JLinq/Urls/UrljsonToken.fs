namespace FSharp.JLinq.Urls

open FSharp.Idioms.StringOps
open System

type UrljsonToken = 
| EXCLAM      // COMMA
| ASTERISK    // COLON
| LEFT_PAREN  // LEFT_BRACK,  LEFT_BRACE
| RIGHT_PAREN // RIGHT_BRACK, RIGHT_BRACE
| STRING of string
| KEY of string // KEY          = STRING / STAR
| NUMBER of float
| NULL
| FALSE
| TRUE
| EMPTY_OBJECT  // EMPTY_OBJECT = LEFT_PAREN STAR RIGHT_PAREN

    member this.tag =
        match this with
        | EXCLAM -> "!"
        | ASTERISK -> "*"
        | LEFT_PAREN -> "("
        | RIGHT_PAREN -> ")"
        | NULL -> "NULL"
        | FALSE -> "FALSE"
        | TRUE -> "TRUE"
        | STRING _ -> "STRING"
        | NUMBER _ -> "NUMBER"
        | KEY _ -> "KEY"
        | EMPTY_OBJECT -> "EMPTY_OBJECT"

    static member tokenize (inp:string) =
        let rec loop (inp:string) =
            seq {
                match inp with
                | "" -> ()
        
                | Prefix @"\s+" (_,rest) -> 
                    yield! loop rest
        
                | PrefixChar '(' rest ->
                    yield LEFT_PAREN
                    yield! loop rest
        
                | PrefixChar ')' rest ->
                    yield RIGHT_PAREN
                    yield! loop rest
                
                | PrefixChar '!' rest ->
                    yield EXCLAM
                    yield! loop rest
        
                | PrefixChar '*' rest ->
                    yield ASTERISK
                    yield! loop rest
        
                | Prefix @"'(~[~'bfnrtv]|~[01][0-9A-Fa-f]|[^~']+)*'" (lexeme,rest) -> // 不支持落单的反斜杠
                    yield  STRING (Apostrophe.parseLiteral lexeme)
                    yield! loop rest
        
                | Prefix @"[\S-[()*!']]+" (lexeme,rest) ->
                    yield  KEY lexeme
                    yield! loop rest

                | never -> failwith never
            }
        
        loop inp
        

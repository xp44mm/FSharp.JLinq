module FSharp.JLinq.Serialization.JsonTranslation

open Newtonsoft.Json.Linq
open FSharpCompiler.Parsing

let rec translateValue = function
| Interior("value",[Interior("object",_) as object]) ->
    translateObject object :> JToken
| Interior("value",[Interior("array",_) as arr]) ->
    translateArray arr :> JToken
| Interior("value",[Terminal NULL]) ->
    JValue(null:obj) :> JToken
| Interior("value",[Terminal FALSE]) ->
    JValue(false) :> JToken
| Interior("value",[Terminal TRUE]) ->
    JValue true :> JToken
| Interior("value",[Terminal(STRING s)]) ->
    JValue s :> JToken
| Interior("value",[Terminal(NUMBER n)]) ->
    JValue n :> JToken
| never -> failwithf "%A"  <| never.firstLevel()

and translateObject = function
| Interior("object",[Terminal LEFT_BRACE;Terminal RIGHT_BRACE]) ->
    JObject()
| Interior("object",[Terminal LEFT_BRACE; fields; Terminal RIGHT_BRACE]) ->
    let props:JProperty[] = 
        translateFields fields
        |> List.rev
        |> List.toArray
    JObject(props)
| never -> failwithf "%A"  <| never.firstLevel()

and translateArray = function
| Interior("array",[Terminal LEFT_BRACK;Terminal RIGHT_BRACK]) ->
    JArray()
| Interior("array",[Terminal LEFT_BRACK;values;Terminal RIGHT_BRACK]) ->
    let values: JToken []=
        values
        |> translateValues
        |> List.rev
        |> List.toArray
    JArray(values)
| never -> failwithf "%A"  <| never.firstLevel()

and translateFields = function
| Interior("fields",[Interior("fields",_) as ls; Terminal COMMA; field]) ->
    translateField field :: translateFields ls
| Interior("fields",[field]) ->
    [translateField field]
| never -> failwithf "%A"  <| never.firstLevel()

and translateField = function
| Interior("field",[Terminal(STRING s);Terminal COLON;value]) ->
    JProperty(s, translateValue value)
| never -> failwithf "%A"  <| never.firstLevel()

and translateValues = function
| Interior("values",[value]) ->
    [translateValue value]
| Interior("values",[Interior("values",_) as ls;Terminal COMMA; value]) ->
    translateValue value :: translateValues ls
| never -> failwithf "%A" <| never.firstLevel()



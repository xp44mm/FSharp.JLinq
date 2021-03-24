module FSharp.JLinq.Urls.UrljsonTranslation

open FSharpCompiler.Parsing
open FSharp.JLinq
open FSharp.JLinq.Urls
open Newtonsoft.Json.Linq

let rec translateValue = function
| Interior("value",[Interior("object",_) as object]) ->
    object
    |> translateObject
    :> JToken
| Interior("value",[Interior("array",_) as array]) ->
    array
    |> translateArray
    :> JToken

| Interior("value",[Terminal UrljsonToken.NULL]) ->
    JValue(null:obj):> JToken

| Interior("value",[Terminal UrljsonToken.FALSE]) ->
    JValue(false):> JToken

| Interior("value",[Terminal UrljsonToken.TRUE]) ->
    JValue(true):> JToken

| Interior("value",[Terminal(UrljsonToken.STRING s)]) ->
    JValue s:> JToken

| Interior("value",[Terminal(UrljsonToken.NUMBER s)]) ->
    JValue s:> JToken

| never -> failwithf "%A"  <| never.firstLevel()

and translateObject = function
| Interior("object",[Terminal UrljsonToken.EMPTY_OBJECT]) ->
    JObject()
| Interior("object",[Terminal UrljsonToken.LEFT_PAREN;fields;Terminal UrljsonToken.RIGHT_PAREN]) ->
    let props =
        translateFields fields
        |> List.rev
    JObject props
| never -> failwithf "%A"  <| never.firstLevel()

and translateFields = function
| Interior("fields",[field]) ->
    [translateField field]
| Interior("fields",[Interior("fields",_) as fields; Terminal UrljsonToken.ASTERISK; field]) ->
    translateField field :: translateFields fields
| never -> failwithf "%A"  <| never.firstLevel()

and translateField = function
| Interior("field",[Terminal(KEY key); Terminal UrljsonToken.EXCLAM; value]) ->
    JProperty(key, translateValue value)
| never -> failwithf "%A"  <| never.firstLevel()

and translateArray = function
| Interior("array",[Terminal UrljsonToken.LEFT_PAREN;Terminal UrljsonToken.RIGHT_PAREN]) ->
    JArray()
| Interior("array",[Terminal UrljsonToken.LEFT_PAREN;values;Terminal UrljsonToken.RIGHT_PAREN]) ->
    let elements =
        values
        |> translateValues
        |> List.rev
    JArray elements
| never -> failwithf "%A"  <| never.firstLevel()

and translateValues = function
| Interior("values",[value]) ->
    [translateValue value]
| Interior("values",[Interior("values",_) as values; Terminal UrljsonToken.ASTERISK; value]) ->
    translateValue value :: translateValues values
| never -> failwithf "%A" <| never.firstLevel()

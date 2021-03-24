module FSharp.JLinq.Serialization.JsonRender

open FSharp.Literals
open FSharp.Idioms
open System
open Newtonsoft.Json
open Newtonsoft.Json.Linq

let rec stringify (json:JToken)= 
    match json with
    | :? JObject as jobj ->
        jobj.Children()
        |> Seq.map(fun jtok ->
            let prop = jtok :?> JProperty
            StringLiteral.toStringLiteral prop.Name + ":" + stringify prop.Value
        )
        |> String.concat ","
        |> sprintf "{%s}"

    | :? JArray as jarr ->
        jarr.Children()
        |> Seq.map(fun v -> stringify v)
        |> String.concat ","
        |> sprintf "[%s]"
    | :? JValue as jval when jval.Value = null -> "null"
    | :? JValue as jval ->
        match jval.Value with
        | :? bool as v -> Render.stringify v
        | :? string as v -> JsonConvert.SerializeObject(v)
        | :? float as v -> JsonConvert.SerializeObject(v)
        | v -> JsonConvert.SerializeObject(v)
    | _ -> JsonConvert.SerializeObject(json)



module FSharp.JLinq.Urls.Urljson

open FSharp.JLinq
open FSharp.JLinq.JTokenAdapters

open System
open Newtonsoft.Json
open Newtonsoft.Json.Linq

let rec stringify (json:JToken) = 
    match json with
    | :? JObject as jo ->
        jo :> seq<_>
        |> Seq.map(fun jt -> jt:?> JProperty)
        |> Seq.map(fun jp -> Apostrophe.toKey jp.Name + "!" + stringify jp.Value )
        |> String.concat "*"
        |> sprintf "(%s)"

    | :? JArray as ls ->
        ls :> seq<_>
        |> Seq.map(fun v -> stringify v )
        |> String.concat "*"
        |> sprintf "(%s)"
    | :? JValue as jv ->
        if isNull jv.Value then
            "null"
        elif jv.Value.GetType() = typeof<string> then
            let s = unbox<string> jv.Value
            Apostrophe.toLiteral s

        elif jv.Value.GetType() = typeof<char> then
            let s = unbox<char> jv.Value
            Apostrophe.toLiteral <| String(s,1)
        else
            JsonConvert.SerializeObject jv.Value
    | _ -> failwithf "%A" <| json.GetType()

/// convert from value to string in json format
let serialize<'t> (value:'t) = 
    value |> ObjectConverter.read |> stringify

let deserializeObj (ty:Type) (text:string) = 
    text |> UrljsonDriver.parse |> JTokenReader.mainReadDynamic JTokenReader.readers ty

/// convert from string instantiate value
let deserialize<'t> (text:string) = 
    text |> UrljsonDriver.parse |> ObjectConverter.write<'t>



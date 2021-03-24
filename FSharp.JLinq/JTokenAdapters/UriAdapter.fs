namespace FSharp.JLinq.JTokenAdapters

open System
open Newtonsoft.Json.Linq

type UriWriterAdapter() = 
    static member Singleton = UriWriterAdapter() :> JTokenWriterAdapter
    interface JTokenWriterAdapter with
        member this.filter(ty,value) = ty = typeof<Uri>
        member this.write(loop,ty,value) = JValue(value) :> JToken

type UriReaderAdapter() =
    static member Singleton = UriReaderAdapter() :> JTokenReaderAdapter
    interface JTokenReaderAdapter with
        member this.filter(ty,json) = ty = typeof<Uri>
        member this.read(loop,ty,json) = 
            match (json:?>JValue).Value with
            | :? string as s -> Uri(s)
            | :? Uri as d -> d
            | x -> failwithf "%A" x
            |> box



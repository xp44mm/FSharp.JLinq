namespace FSharp.JLinq.JTokenAdapters

open System
open Newtonsoft.Json.Linq

type DateTimeOffsetWriterAdapter() = 
    static member Singleton = DateTimeOffsetWriterAdapter() :> JTokenWriterAdapter
    interface JTokenWriterAdapter with
        member this.filter(ty,value) = ty = typeof<DateTimeOffset>
        member this.write(loop,ty,value) = JValue(value) :> JToken

type DateTimeOffsetReaderAdapter() =
    static member Singleton = DateTimeOffsetReaderAdapter() :> JTokenReaderAdapter
    interface JTokenReaderAdapter with
        member this.filter(ty,json) = ty = typeof<DateTimeOffset>
        member this.read(loop,ty,json) = 
            match (json:?>JValue).Value with
            | :? string as s -> DateTimeOffset.Parse(s)
            | :? DateTimeOffset as d -> d
            | x -> failwithf "%A" x
            |> box

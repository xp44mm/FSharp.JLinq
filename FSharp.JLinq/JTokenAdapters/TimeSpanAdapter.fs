namespace FSharp.JLinq.JTokenAdapters

open System
open Newtonsoft.Json.Linq

type TimeSpanWriterAdapter() = 
    static member Singleton = TimeSpanWriterAdapter() :> JTokenWriterAdapter
    interface JTokenWriterAdapter with
        member this.filter(ty,value) = ty = typeof<TimeSpan>
        member this.write(loop,ty,value) = JValue(value) :> JToken

type TimeSpanReaderAdapter() =
    static member Singleton = TimeSpanReaderAdapter() :> JTokenReaderAdapter
    interface JTokenReaderAdapter with
        member this.filter(ty,json) = ty = typeof<TimeSpan>
        member this.read(loop,ty,json) = 
            match (json:?>JValue).Value with
            | :? string as s -> TimeSpan.Parse(s)
            | :? TimeSpan as d -> d
            | x -> failwithf "%A" x
            |> box


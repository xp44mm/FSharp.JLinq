namespace FSharp.JLinq.JTokenAdapters

open System
open Newtonsoft.Json.Linq

type GuidWriterAdapter() = 
    static member Singleton = GuidWriterAdapter() :> JTokenWriterAdapter
    interface JTokenWriterAdapter with
        member this.filter(ty,value) = ty = typeof<Guid>
        member this.write(loop,ty,value) = JValue(value) :> JToken

type GuidReaderAdapter() =
    static member Singleton = GuidReaderAdapter() :> JTokenReaderAdapter
    interface JTokenReaderAdapter with
        member this.filter(ty,json) = ty = typeof<Guid>
        member this.read(loop,ty,json) = 
            match (json:?>JValue).Value with
            | :? string as s -> Guid.Parse(s)
            | :? Guid as d -> d
            | x -> failwithf "%A" x
            |> box



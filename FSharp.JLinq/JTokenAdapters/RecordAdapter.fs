namespace FSharp.JLinq.JTokenAdapters

open System
open Newtonsoft.Json.Linq
open FSharp.Idioms
open Microsoft.FSharp.Reflection

type RecordWriterAdapter () = 
    static member Singleton = RecordWriterAdapter() :> JTokenWriterAdapter
    interface JTokenWriterAdapter with
        member _.filter(ty,value) = FSharpType.IsRecord ty
        member _.write(loop, ty, value) = 
            let read = RecordType.readRecord ty

            let fields =
                read value
                |> Array.map(fun(pi,value) -> JProperty(pi.Name, loop pi.PropertyType value))

            JObject fields :> JToken

type RecordReaderAdapter() =
    static member Singleton = RecordReaderAdapter() :> JTokenReaderAdapter

    interface JTokenReaderAdapter with
        member this.filter(ty, json) = FSharpType.IsRecord ty
        member this.read(loop:Type -> JToken -> obj, ty:Type, json:JToken) =
            let jo = json :?> JObject
            let values =
                RecordType.getRecordFields(ty)
                |> Array.map(fun pi -> loop pi.PropertyType jo.[pi.Name])

            FSharpValue.MakeRecord(ty,values)


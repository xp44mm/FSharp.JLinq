namespace FSharp.JLinq.JTokenAdapters

open System
open Newtonsoft.Json.Linq
open FSharp.Idioms
open Microsoft.FSharp.Reflection

type TupleWriterAdapter () = 
    static member Singleton = TupleWriterAdapter() :> JTokenWriterAdapter
    interface JTokenWriterAdapter with
        member _.filter(ty,value) = FSharpType.IsTuple ty
        member _.write(loop, ty, value) = 
            let read = TupleType.readTuple ty

            let elements =
                read value
                |> List.ofArray
                |> List.map(fun(ftype,field)-> loop ftype field)
            JArray elements :> JToken

type TupleReaderAdapter() =
    static member Singleton = TupleReaderAdapter() :> JTokenReaderAdapter

    interface JTokenReaderAdapter with
        member this.filter(ty, json) = FSharpType.IsTuple ty
        member this.read(loop:Type -> JToken -> obj, ty:Type, json:JToken) =
            let values = (json :?> JArray).Values() |> Array.ofSeq
            let elementTypes = FSharpType.GetTupleElements(ty)

            let values =
                values
                |> Array.zip elementTypes
                |> Array.map(fun(tp,json)-> loop tp json)

            FSharpValue.MakeTuple(values,ty)








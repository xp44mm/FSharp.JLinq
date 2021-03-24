namespace FSharp.JLinq.JTokenAdapters

open System
open Newtonsoft.Json.Linq
open System
open FSharp.Idioms
open System.Collections.Generic

type ArrayWriterAdapter () = 
    static member Singleton = ArrayWriterAdapter() :> JTokenWriterAdapter
    interface JTokenWriterAdapter with
        member _.filter(ty,value) = ty.IsArray && ty.GetArrayRank() = 1
        member _.write(loop, ty, value) = 
            let elemType,elements = ArrayType.readArray ty value
            let ls =
                elements
                |> Array.map(loop elemType)
            JArray ls :> JToken

type ArrayReaderAdapter() =
    static member Singleton = ArrayReaderAdapter() :> JTokenReaderAdapter

    interface JTokenReaderAdapter with
        member this.filter(ty, json) = ty.IsArray && ty.GetArrayRank() = 1
        member this.read(loop:Type -> JToken -> obj, ty:Type, json:JToken) =
            let elementType = ArrayType.getElementType ty
            let jarr = json :?> JArray
            let arr = (Array.CreateInstance:Type*int->Array)(elementType, jarr.Count)

            jarr.Values()
            |> Seq.map(fun e -> loop elementType e)
            |> Seq.iteri(fun i v -> arr.SetValue(v, i))
            box arr

namespace FSharp.JLinq.JTokenAdapters

open System
open Newtonsoft.Json.Linq
open FSharp.Idioms
open Microsoft.FSharp.Reflection

type SetWriterAdapter () = 
    static member Singleton = SetWriterAdapter() :> JTokenWriterAdapter
    interface JTokenWriterAdapter with
        member _.filter(ty,value) = ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Set<_>>
        member _.write(loop, ty, value) = 
            let elemType, elements = SetType.readSet ty value
            let elements =
                elements
                |> Array.map(loop elemType)
            JArray elements :> JToken

type SetReaderAdapter() =
    static member Singleton = SetReaderAdapter() :> JTokenReaderAdapter
    interface JTokenReaderAdapter with
        member this.filter(ty, json) = ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Set<_>>
        member this.read(loop:Type -> JToken -> obj, ty:Type, json:JToken) =
            let elementType = SetType.getElementType ty
            let arrayType = elementType.MakeArrayType()
            let arr = loop arrayType json
            let mOfArray = SetType.getOfArray ty
            mOfArray.Invoke(null, Array.singleton arr)

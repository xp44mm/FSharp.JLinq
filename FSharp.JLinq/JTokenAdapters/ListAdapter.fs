namespace FSharp.JLinq.JTokenAdapters

open System
open Newtonsoft.Json.Linq
open FSharp.Idioms
open Microsoft.FSharp.Reflection

type ListWriterAdapter () = 
    static member Singleton = ListWriterAdapter() :> JTokenWriterAdapter
    interface JTokenWriterAdapter with
        member _.filter(ty,value) = ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<List<_>>
        member _.write(loop, ty, value) = 
            let elemType, elements = ListType.readList ty value
            let elements =
                elements
                |> Array.map(loop elemType)
            JArray elements :> JToken

type ListReaderAdapter() =
    static member Singleton = ListReaderAdapter() :> JTokenReaderAdapter
    interface JTokenReaderAdapter with
        member this.filter(ty, json) = ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<List<_>>
        member this.read(loop:Type -> JToken -> obj, ty:Type, json:JToken) =
            let elementType = ListType.getElementType ty
            let arrayType = elementType.MakeArrayType()
            let arr = loop arrayType json
            let mOfArray = ListType.getOfArray ty
            mOfArray.Invoke(null, Array.singleton arr)


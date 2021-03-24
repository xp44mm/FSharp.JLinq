namespace FSharp.JLinq.JTokenAdapters

open System
open Newtonsoft.Json.Linq
open FSharp.Idioms
open Microsoft.FSharp.Reflection

type MapWriterAdapter () = 
    static member Singleton = MapWriterAdapter() :> JTokenWriterAdapter
    interface JTokenWriterAdapter with
        member _.filter(ty,value) = ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Map<_,_>>
        member _.write(loop, ty, value) = 
            let elemType, elements = MapType.readMap ty value
            let elements =
                elements
                |> Array.map(loop elemType)
            JArray elements :> JToken

type MapReaderAdapter() =
    static member Singleton = MapReaderAdapter() :> JTokenReaderAdapter
    interface JTokenReaderAdapter with
        member this.filter(ty, json) = ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Map<_,_>>
        member this.read(loop:Type -> JToken -> obj, ty:Type, json:JToken) =
            let elementType = MapType.getElementType ty
            let arrayType = elementType.MakeArrayType()
            let arr = loop arrayType json
            let mOfArray = MapType.getOfArray ty
            mOfArray.Invoke(null, Array.singleton arr)

namespace FSharp.JLinq.JTokenAdapters

open System
open Newtonsoft.Json.Linq

type NullableWriterAdapter () = 
    static member Singleton = NullableWriterAdapter() :> JTokenWriterAdapter

    interface JTokenWriterAdapter with
        member _.filter(ty,value) = 
            ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Nullable<_>>
        member _.write(loop, ty, value) = 
            if value = null then
                JValue(null:obj) :> JToken
            else
                let underlyingType = ty.GenericTypeArguments.[0]
                loop underlyingType value

type NullableReaderAdapter() =
    static member Singleton = NullableReaderAdapter() :> JTokenReaderAdapter

    interface JTokenReaderAdapter with
        member this.filter(ty, json) =
            ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Nullable<_>>
        member this.read(loop:Type -> JToken -> obj, ty:Type, json:JToken) =
            match (json :?> JValue).Value with
            | null -> null
            | _ ->
                let underlyingType = ty.GenericTypeArguments.[0]
                loop underlyingType json

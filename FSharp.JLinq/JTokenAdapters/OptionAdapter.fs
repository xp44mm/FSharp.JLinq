namespace FSharp.JLinq.JTokenAdapters

open System
open Newtonsoft.Json.Linq
open FSharp.Idioms
open Microsoft.FSharp.Reflection

type OptionWriterAdapter () = 
    static member Singleton = OptionWriterAdapter() :> JTokenWriterAdapter
    interface JTokenWriterAdapter with
        member _.filter(ty,value) = ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Option<_>>
        member _.write(loop, ty, value) = 
            if value = null then
                JValue(null:obj) :> JToken
            else
                let reader = UnionType.readUnion ty
                let _,fields = reader value
                let ftype,fvalue = fields.[0]
                loop ftype fvalue

type OptionReaderAdapter() =
    static member Singleton = OptionReaderAdapter() :> JTokenReaderAdapter
    interface JTokenReaderAdapter with
        member this.filter(ty, json) = ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Option<_>>
        member this.read(loop:Type -> JToken -> obj, ty:Type, json:JToken) =
            match json with
            | :? JValue as jv when jv.Value = null -> box None
            | jvalue ->
                let unionCaseInfo =
                    FSharpType.GetUnionCases ty
                    |> Array.find(fun c -> c.Name = "Some")

                let uionFieldType =
                    unionCaseInfo.GetFields()
                    |> Array.map(fun info -> info.PropertyType)
                    |> Array.exactlyOne

                FSharpValue.MakeUnion(unionCaseInfo, Array.singleton(loop uionFieldType jvalue))

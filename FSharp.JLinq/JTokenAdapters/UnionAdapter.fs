namespace FSharp.JLinq.JTokenAdapters

open System
open Newtonsoft.Json.Linq
open FSharp.Idioms
open Microsoft.FSharp.Reflection

type UnionWriterAdapter () = 
    static member Singleton = UnionWriterAdapter() :> JTokenWriterAdapter
    interface JTokenWriterAdapter with
        member _.filter(ty,value) = FSharpType.IsUnion ty
        member _.write(loop, ty, value) = 
            let reader = UnionType.readUnion ty
            let name,fields = reader value
            //简化
            let unionFields =
                if Array.isEmpty fields then 
                    // union case is paramless
                    JValue(null:obj) :> JToken
                elif fields.Length = 1 then 
                    // union case is single param
                    loop <|| fields.[0]
                else 
                    // union case is tuple
                    fields
                    |> Array.map(fun(ftype,field)-> loop ftype field)
                    |> JArray
                    :> JToken
            JObject(JProperty(name,unionFields)) :> JToken


type UnionReaderAdapter() =
    static member Singleton = UnionReaderAdapter() :> JTokenReaderAdapter
    interface JTokenReaderAdapter with
        member this.filter(ty, json) = FSharpType.IsUnion ty
        member this.read(loop:Type -> JToken -> obj, ty:Type, json:JToken) =
            let jp = (json :?> JObject) |> Seq.exactlyOne :?> JProperty
            let jkey, jvalue =  jp.Name, jp.Value

            let unionCaseInfo =
                UnionType.getUnionCases ty
                |> Array.find(fun c -> c.Name = jkey)

            let uionFieldTypes =
                UnionType.getCaseFields unionCaseInfo
                |> Array.map(fun info -> info.PropertyType)

            match uionFieldTypes with
            | [||] ->
                FSharpValue.MakeUnion(unionCaseInfo, Array.empty)
            | [|fieldType|] ->
                FSharpValue.MakeUnion(unionCaseInfo, Array.singleton(loop fieldType jvalue))
            | _ ->
                let fields =
                    (jvalue :?> JArray).Values()
                    |> Array.ofSeq
                    |> Array.zip uionFieldTypes
                    |> Array.map(fun(t,j) -> loop t j)
                FSharpValue.MakeUnion(unionCaseInfo, fields)






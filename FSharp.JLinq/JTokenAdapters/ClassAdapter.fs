namespace FSharp.JLinq.JTokenAdapters

open System
open Newtonsoft.Json.Linq
open FSharp.Idioms
open System.Reflection
open System.Collections.Generic

type ClassWriterAdapter () = 
    static member Singleton = ClassWriterAdapter() :> JTokenWriterAdapter
    interface JTokenWriterAdapter with
        member _.filter(ty,value) = ty.IsClass
        member _.write(loop, ty, value) = 
            if isNull value then 
                JValue(null:obj):>JToken
            elif ty = typeof<string> then
                let value = unbox<string> value
                JValue value:> JToken
            else
                let members =
                    ty.GetMembers(BindingFlags.Public ||| BindingFlags.Instance)
                    |> Array.filter(fun mmbr ->
                        match mmbr.MemberType with
                        | MemberTypes.Field ->
                            let fieldInfo = mmbr :?> FieldInfo
                            not fieldInfo.IsLiteral && not fieldInfo.IsInitOnly
                        | MemberTypes.Property ->
                            let propertyInfo = mmbr :?> PropertyInfo
                            propertyInfo.CanRead && propertyInfo.CanWrite
                         | _ -> false
                    )
                    |> Array.map(fun mmbr ->
                        let json =
                            match mmbr.MemberType with
                            | MemberTypes.Field ->
                                let fieldInfo = mmbr:?>FieldInfo
                                let value = fieldInfo.GetValue(value)
                                loop fieldInfo.FieldType value
                            | MemberTypes.Property ->
                                let propertyInfo = mmbr :?> PropertyInfo
                                let value = propertyInfo.GetValue(value)                        
                                loop propertyInfo.PropertyType value
                            | mt -> failwithf "never %A" mt
                        JProperty(mmbr.Name, json)
                    )
                JObject members :> JToken

type ClassReaderAdapter() =
    static member Singleton = ClassReaderAdapter() :> JTokenReaderAdapter
    interface JTokenReaderAdapter with
        member this.filter(ty, json) = ty.IsClass
        member this.read(loop:Type -> JToken -> obj, ty:Type, json:JToken) =
            match json with
            | :? JValue as jv when jv.Value = null || jv.Value.GetType() = ty -> jv.Value
            | :? JObject as jo ->
                let target = Activator.CreateInstance(ty)

                jo :> seq<KeyValuePair<string, JToken>>
                |> Seq.iter(fun(KeyValue(name,jtok)) ->
                    let mmbr = 
                        ty.GetMember(name, BindingFlags.Public ||| BindingFlags.Instance)
                        |> Array.exactlyOne

                    match mmbr.MemberType with
                    | MemberTypes.Field ->
                        let fieldInfo = mmbr:?>FieldInfo
                        let value = loop fieldInfo.FieldType jtok
                        fieldInfo.SetValue(target,value)
                    | MemberTypes.Property ->
                        let propertyInfo = mmbr:?>PropertyInfo
                        let value = loop propertyInfo.PropertyType jtok
                        propertyInfo.SetValue(target,value)
                    | _ -> ()
                       
                    )
                target
            | _ -> failwith "ClassReaderAdapter.read()"

namespace FSharp.JLinq.JTokenAdapters

open System
open Newtonsoft.Json.Linq
open FSharp.Idioms

type EnumWriterAdapter() = 
    static member Singleton = EnumWriterAdapter() :> JTokenWriterAdapter
    interface JTokenWriterAdapter with
        member this.filter(ty,value) = ty.IsEnum
        member this.write(loop,ty,value) = 
            let value = 
                if ty.IsDefined(typeof<FlagsAttribute>,false) then
                    let read = EnumType.readFlags ty
                    read value
                    |> String.concat ","
                else
                    Enum.GetName(ty,value)

            JValue(value) :> JToken

type EnumReaderAdapter() =
    static member Singleton = EnumReaderAdapter() :> JTokenReaderAdapter
    interface JTokenReaderAdapter with
        member this.filter(ty,json) = ty.IsEnum
        member this.read(loop,ty,json) = 
            let enumUnderlyingType = EnumType.getEnumUnderlyingType ty
            let values = EnumType.getValues ty

            let s = (json:?>JValue).Value :?> string

            if ty.IsDefined(typeof<FlagsAttribute>,false) then
                s.Split(',')
                |> Array.map(fun flag -> values.[flag])
                |> Array.reduce(|||)
            else
                values.[s]
            |> EnumType.fromUInt64 enumUnderlyingType
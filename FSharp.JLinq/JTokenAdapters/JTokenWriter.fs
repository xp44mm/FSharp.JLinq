namespace FSharp.JLinq.JTokenAdapters

open System
open FSharp.Literals
open Newtonsoft.Json.Linq

module JTokenWriter =
    let writeFromArrayElements (loop: Type -> obj -> JToken) (elemType: Type) (elements: obj[]) =
        let ls =
            elements
            |> List.ofArray
            |> List.map(loop elemType)
    
        JArray ls

    let writeFromTupleFields (loopRead:Type -> obj -> JToken) (fields:(Type*obj)[]) =
        let elements =
            fields
            |> List.ofArray
            |> List.map(fun(ftype,field)-> loopRead ftype field)
        JArray elements

    let fallback(loop:Type -> obj -> JToken, ty:Type, value:obj) =
        if ty = typeof<bool> then
            let b = unbox<bool> value
            if b then JValue(true) else JValue(false)
            :> JToken
        elif ty = typeof<sbyte> then
            let value = unbox<sbyte> value
            JValue value:> JToken
            
        elif ty = typeof<byte> then
            let value = unbox<byte> value
            JValue value:> JToken
    
        elif ty = typeof<int16> then
            let value = unbox<int16> value
            JValue value:> JToken
    
        elif ty = typeof<uint16> then
            let value = unbox<uint16> value
            JValue value:> JToken
    
        elif ty = typeof<int> then
            let value = unbox<int> value
            JValue value:> JToken
    
        elif ty = typeof<uint32> then
            let value = unbox<uint32> value
            JValue value:> JToken
    
        elif ty = typeof<int64> then
            let value = unbox<int64> value
            JValue value:> JToken
    
        elif ty = typeof<uint64> then
            let value = unbox<uint64> value
            JValue value:> JToken
    
        elif ty = typeof<single> then
            let value = unbox<single> value
            JValue value:> JToken
    
        elif ty = typeof<float> then
            let value = unbox<float> value
            JValue value:> JToken
    
        elif ty = typeof<char> then
            let value = unbox<char> value
            JValue value:> JToken

        elif ty = typeof<string> then
            let value = unbox<string> value
            JValue value:> JToken

        elif ty = typeof<decimal> then
            let value = unbox<decimal> value
            JValue value:> JToken
    
        elif ty = typeof<nativeint> then
            let value = (unbox<nativeint> value).ToInt64()
            JValue value:> JToken
    
        elif ty = typeof<unativeint> then
            let value = (unbox<unativeint> value).ToUInt64()
            JValue value :> JToken
    
        elif isNull value then
            JValue (null:obj):> JToken
        elif ty = typeof<obj> && value.GetType() <> typeof<obj> then
            loop (value.GetType()) value
        else
            JValue (Render.stringify value) :> JToken
        
    let rec mainWriteDynamic (adapters:#seq<JTokenWriterAdapter>) (ty:Type) (value:obj) =
        let action =
            adapters
            |> Seq.tryFind(fun reader -> reader.filter(ty,value))
            |> Option.map(fun x -> x.write)
            |> Option.defaultValue (fallback)

        action(mainWriteDynamic adapters, ty, value)

    let writers = [
        DateTimeOffsetWriterAdapter.Singleton
        TimeSpanWriterAdapter.Singleton
        GuidWriterAdapter.Singleton
        EnumWriterAdapter.Singleton
        DBNullWriterAdapter.Singleton
        NullableWriterAdapter.Singleton
        ArrayWriterAdapter.Singleton
        TupleWriterAdapter.Singleton
        RecordWriterAdapter.Singleton
        ListWriterAdapter.Singleton
        SetWriterAdapter.Singleton
        MapWriterAdapter.Singleton
        OptionWriterAdapter.Singleton
        UnionWriterAdapter.Singleton
        ClassWriterAdapter.Singleton
        ]

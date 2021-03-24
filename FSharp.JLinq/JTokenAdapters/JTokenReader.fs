namespace FSharp.JLinq.JTokenAdapters

open System
open Microsoft.FSharp.Core
open Newtonsoft.Json.Linq
open FSharp.JLinq.JTokenAdapters

module JTokenReader =
    ///
    let fallback (loop:Type -> JToken -> obj, ty:Type, json:JToken) =
        let jv = json :?> JValue

        if jv.Value = null || jv.Value.GetType() = ty then
            box jv.Value
        else
            let value = jv.Value
            if ty = typeof<sbyte> then
                Convert.ToSByte value
                |> box
    
            elif ty = typeof<byte> then
                Convert.ToByte value
                |> box
    
            elif ty = typeof<int16> then
                Convert.ToInt16 value
                |> box
    
            elif ty = typeof<uint16> then
                Convert.ToUInt16 value
                |> box
  
            elif ty = typeof<int> then
                Convert.ToInt32 value
                |> box

            elif ty = typeof<uint32> then
                Convert.ToUInt32 value
                |> box

            elif ty = typeof<int64> then
                Convert.ToInt64 value
                |> box
    
            elif ty = typeof<uint64> then
                Convert.ToUInt64 value
                |> box
    
            elif ty = typeof<single> then
                Convert.ToSingle value
                |> box
    
            elif ty = typeof<float> then
                Convert.ToDouble value
                |> box
    
            elif ty = typeof<decimal> then
                Convert.ToDecimal value
                |> box
    
            elif ty = typeof<nativeint> then
                value
                |> if value.GetType() = typeof<int64> then unbox<int64> else Convert.ToInt64
                |> IntPtr
                |> box
    
            elif ty = typeof<unativeint> then
                value
                |> if value.GetType() = typeof<uint64> then unbox<uint64> else Convert.ToUInt64
                |> UIntPtr
                |> box
    
            elif ty = typeof<char> then
                (unbox<string> value).Chars 0
                |> box
            else
                failwithf "target type is `%s`"  ty.Name

    /// write to obj from json.
    let rec mainReadDynamic (adapters:#seq<JTokenReaderAdapter>) (ty:Type) (json:JToken) =
        let write =
            adapters
            |> Seq.tryFind(fun w -> w.filter(ty,json))
            |> Option.map(fun w -> w.read)
            |> Option.defaultValue fallback

        write(mainReadDynamic adapters, ty, json)

    let readers = [
        DateTimeOffsetReaderAdapter.Singleton
        TimeSpanReaderAdapter.Singleton
        GuidReaderAdapter.Singleton
        EnumReaderAdapter.Singleton
        DBNullReaderAdapter.Singleton
        NullableReaderAdapter.Singleton
        ArrayReaderAdapter.Singleton
        TupleReaderAdapter.Singleton
        RecordReaderAdapter.Singleton
        ListReaderAdapter.Singleton
        SetReaderAdapter.Singleton
        MapReaderAdapter.Singleton
        OptionReaderAdapter.Singleton
        UnionReaderAdapter.Singleton
        ClassReaderAdapter.Singleton
    ]
    
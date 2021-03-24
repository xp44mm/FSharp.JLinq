namespace FSharp.JLinq.JTokenAdapters

open System
open Newtonsoft.Json.Linq

type DBNullWriterAdapter() = 
    static member Singleton = DBNullWriterAdapter() :> JTokenWriterAdapter
    interface JTokenWriterAdapter with
        member this.filter(ty,value) = ty = typeof<DBNull>
        member this.write(loop,ty,value) = JValue(null:obj) :> JToken

type DBNullReaderAdapter() =
    static member Singleton = DBNullReaderAdapter() :> JTokenReaderAdapter
    interface JTokenReaderAdapter with
        member this.filter(ty,json) = ty = typeof<DBNull>
        member this.read(loop,ty,json) = box DBNull.Value

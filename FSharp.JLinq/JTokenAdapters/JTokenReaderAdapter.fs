namespace FSharp.JLinq.JTokenAdapters

open System
open Newtonsoft.Json.Linq

/// write to fsharp value from json
type JTokenReaderAdapter =
    abstract filter: targetType:Type * json:JToken -> bool
    abstract read: loop:(Type -> JToken -> obj) * targetType:Type * json:JToken -> obj

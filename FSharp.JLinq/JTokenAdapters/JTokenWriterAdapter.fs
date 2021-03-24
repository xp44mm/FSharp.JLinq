namespace FSharp.JLinq.JTokenAdapters

open System
open Newtonsoft.Json.Linq

type JTokenWriterAdapter = 
    abstract filter: ty:Type * value:obj -> bool
    abstract write: loop:(Type -> obj -> JToken) * ty:Type * value:obj -> JToken


    
    
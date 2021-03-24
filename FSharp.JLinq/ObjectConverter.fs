module FSharp.JLinq.ObjectConverter

open FSharp.JLinq.JTokenAdapters
open Newtonsoft.Json.Linq

/// convert from value to jtoken
let read<'t> (value:'t) = JTokenWriter.mainWriteDynamic JTokenWriter.writers typeof<'t> value

/// convert from jtoken to value
let write<'t> (json:JToken) = JTokenReader.mainReadDynamic JTokenReader.readers typeof<'t> json :?> 't

/// convert from value to string in jtoken format
let serialize<'t> (value:'t) = value |> read |> JsonSerializer.stringify

/// convert from string instantiate value
let deserialize<'t> (text:string) = 
    text |> JsonSerializer.parse |> write<'t>

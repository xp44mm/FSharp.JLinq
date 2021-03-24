module FSharp.JLinq.JsonSerializer

open FSharp.JLinq.Serialization

let parse(text:string) = 
    if System.String.IsNullOrEmpty text then
        failwith "empty string is illeagal json string."
    else
        JsonDriver.parse text

let stringify jtok = JsonRender.stringify jtok



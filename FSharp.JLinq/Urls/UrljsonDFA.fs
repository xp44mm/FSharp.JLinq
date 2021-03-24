module FSharp.JLinq.Urls.UrljsonDFA
let dtran = 
    set [0u,"!",9u;0u,"(",5u;0u,")",8u;0u,"*",10u;0u,"KEY",3u;0u,"STRING",1u;1u,"!",2u;3u,"!",4u;5u,"!",6u;6u,")",7u]
let finalLexemes:(Set<uint32>*Set<uint32>) list = 
    [set [2u],set [1u];set [1u],Set.empty;set [4u],set [3u];set [3u],Set.empty;set [7u],Set.empty;set [5u],Set.empty;set [8u],Set.empty;set [9u],Set.empty;set [10u],Set.empty]
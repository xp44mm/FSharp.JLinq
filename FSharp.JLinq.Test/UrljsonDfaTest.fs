namespace FSharp.JLinq.Urls

open System
open System.IO

open Xunit
open Xunit.Abstractions
open FSharp.Literals
open FSharp.xUnit
open FSharpCompiler.Lex

type UrljsonDfaTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    let locatePath = Path.Combine(DirectoryInfo(__SOURCE_DIRECTORY__).Parent.FullName,@"FSharp.JLinq\Urls")
    let filePath = Path.Combine(locatePath, @"urljson.lex")
    let text = File.ReadAllText(filePath)

    let dfa = LexFileDriver.parse text

    [<Fact(Skip="done")>]
    member this.``generate DFA``() =
        let result =
            [
                "module FSharp.JLinq.Urls.UrljsonDFA"
                "let dtran = "
                "    " + Render.stringify dfa.transitions
                "let finalLexemes:(Set<uint32>*Set<uint32>) list = "
                "    " + Render.stringify dfa.finalLexemes
            ] |> String.concat Environment.NewLine
        let outputDir = Path.Combine(locatePath, @"UrljsonDFA.fs")
        File.WriteAllText(outputDir,result)
        output.WriteLine("output lex:"+outputDir)

    [<Fact>]
    member this.``verify DFA``() =
        Should.equal dfa.transitions  UrljsonDFA.dtran
        Should.equal dfa.finalLexemes UrljsonDFA.finalLexemes

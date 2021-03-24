namespace FSharp.JLinq.Serialization

open Xunit
open Xunit.Abstractions
open System
open System.IO

open FSharpCompiler.Yacc
open FSharp.Literals
open FSharp.xUnit

type JsonParsingTableTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    let locatePath = Path.Combine(DirectoryInfo(__SOURCE_DIRECTORY__).Parent.FullName,"FSharp.JLinq/Serialization")
    let filePath = Path.Combine(locatePath, @"json.yacc")
    let text = File.ReadAllText(filePath)
    let yaccFile = YaccFile.parse text

    [<Fact>]
    member this.``1-input data``() =
        show yaccFile.mainRules
        let y = [
            ["value";"object"];["value";"array"];["value";"NULL"];["value";"FALSE"];["value";"TRUE"];["value";"STRING"];["value";"NUMBER"];
            ["object";"{";"}"];["object";"{";"fields";"}"];
            ["array";"[";"]"];["array";"[";"values";"]"];
            ["fields";"field"];["fields";"fields";",";"field"];
            ["field";"STRING";":";"value"];
            ["values";"value"];["values";"values";",";"value"]]
        
        should.equal y yaccFile.mainRules

        Assert.True(yaccFile.precedences.IsEmpty)

    [<Fact>]
    member this.``2-产生式冲突``() =
        let tbl = AmbiguousTable.create yaccFile.mainRules
        let pconflicts = ConflictFactory.productionConflict tbl.ambiguousTable
        //show pconflicts
        Assert.True(pconflicts.IsEmpty)

    [<Fact>]
    member this.``3-符号多用警告``() =
        let tbl = AmbiguousTable.create yaccFile.mainRules
        let warning = ConflictFactory.overloadsWarning tbl
        //show warning
        Assert.True(warning.IsEmpty)

    [<Fact>]
    member this.``4-优先级冲突``() =
        let tbl = AmbiguousTable.create yaccFile.mainRules
        let srconflicts = ConflictFactory.shiftReduceConflict tbl
        show srconflicts
        Assert.True(srconflicts.IsEmpty)

    [<Fact(Skip="generate file")>]
    member this.``5-generate parsing table``() =
        let yacc = ParseTable.create(yaccFile.mainRules, yaccFile.precedences)

        //解析表数据
        let result =
            [
                "module FSharp.JLinq.Serialization.JsonParsingTable"
                "let rules = " + Render.stringify yacc.rules
                "let kernelSymbols = " + Render.stringify yacc.kernelSymbols
                "let parsingTable = " + Render.stringify yacc.parsingTable
            ] |> String.concat Environment.NewLine
        let outputDir = Path.Combine(locatePath, @"JsonParsingTable.fs")
        File.WriteAllText(outputDir,result)
        output.WriteLine("output yacc:"+outputDir)

    [<Fact>]
    member this.``5-verify parsing table``() =
        let yacc = ParseTable.create(yaccFile.mainRules, yaccFile.precedences)

        should.equal yacc.rules         JsonParsingTable.rules
        should.equal yacc.kernelSymbols JsonParsingTable.kernelSymbols
        should.equal yacc.parsingTable  JsonParsingTable.parsingTable










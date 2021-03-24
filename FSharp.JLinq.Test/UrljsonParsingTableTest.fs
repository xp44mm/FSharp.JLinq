namespace FSharp.JLinq.Urls

open Xunit
open Xunit.Abstractions
open System.IO

open FSharpCompiler.Yacc
open FSharp.Literals
open FSharp.xUnit

type UrljsonParsingTableTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    let locatePath = Path.Combine(DirectoryInfo(__SOURCE_DIRECTORY__).Parent.FullName,"FSharp.JLinq")
    let filePath = Path.Combine(locatePath, @"urljson.yacc")
    let text = File.ReadAllText(filePath)
    let yaccFile = YaccFile.parse text

    [<Fact>]
    member this.``1-input data``() =
        //show yaccFile.mainRules
        let y = [["value";"object"];["value";"array"];["value";"NULL"];["value";"FALSE"];["value";"TRUE"];["value";"STRING"];["value";"NUMBER"];["object";"EMPTY_OBJECT"];["object";"(";"fields";")"];["fields";"field"];["fields";"fields";"*";"field"];["field";"KEY";"!";"value"];["array";"(";")"];["array";"(";"values";")"];["values";"value"];["values";"values";"*";"value"]]
        
        Should.equal y yaccFile.mainRules
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


    [<Fact(Skip="done")>]
    member this.``5-generate parsing table``() =
        let yacc = ParseTable.create(yaccFile.mainRules, yaccFile.precedences)

        //解析表数据
        let result =
            [
                "module FSharp.JLinq.Urls.UrljsonParsingTable"
                "let rules = " + Render.stringify yacc.rules
                "let kernelSymbols = " + Render.stringify yacc.kernelSymbols
                "let parsingTable = " + Render.stringify yacc.parsingTable
            ] |> String.concat System.Environment.NewLine
        let outputDir = Path.Combine(locatePath, @"UrljsonParsingTable.fs")
        File.WriteAllText(outputDir,result)
        output.WriteLine("output yacc:"+outputDir)

    [<Fact>]
    member this.``5-verify parsing table``() =
        let yacc = ParseTable.create(yaccFile.mainRules, yaccFile.precedences)

        Should.equal yacc.rules         UrljsonParsingTable.rules
        Should.equal yacc.kernelSymbols UrljsonParsingTable.kernelSymbols
        Should.equal yacc.parsingTable  UrljsonParsingTable.parsingTable



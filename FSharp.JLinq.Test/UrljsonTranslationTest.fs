namespace FSharp.JLinq.Urls

open Xunit
open Xunit.Abstractions

open FSharp.Literals
open FSharp.xUnit

open FSharpCompiler.Parsing
open FSharp.JLinq
open FSharp.JLinq.Urls
open Newtonsoft.Json.Linq

type UrljsonTranslationTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine
    
    [<Fact>]
    member this.``empty object``() =
        let x = Interior("value",[Interior("object",[Terminal EMPTY_OBJECT])])
        let y = UrljsonTranslation.translateValue x :?> JObject
        //show y
        should.equal y <| JObject ()

    [<Fact>]
    member this.``empty array``() =
        let x = Interior("value",[Interior("array",[Terminal LEFT_PAREN;Terminal RIGHT_PAREN])])
        let y = UrljsonTranslation.translateValue x :?> JArray
        //show y
        should.equal y 
        <| JArray []

    [<Fact>]
    member this.``null``() =
        let x = Interior("value",[Terminal NULL])
        let y = UrljsonTranslation.translateValue x :?> JValue
        //show y
        should.equal y <| JValue(null:obj)

    [<Fact>]
    member this.``false``() =
        let x = Interior("value",[Terminal FALSE])
        let y = UrljsonTranslation.translateValue x :?> JValue
        //show y
        should.equal y <| JValue(false)

    [<Fact>]
    member this.``true``() =
        let x = Interior("value",[Terminal TRUE])
        let y = UrljsonTranslation.translateValue x :?> JValue
        //show y
        should.equal y <| JValue true

    [<Fact>]
    member this.``empty string``() =
        let x = Interior("value",[Terminal(STRING "")])
        let y = UrljsonTranslation.translateValue x :?> JValue
        //show y
        should.equal y <| JValue ""

    [<Fact>]
    member this.``number``() =
        let x = Interior("value",[Terminal(NUMBER 0.0)])
        let y = UrljsonTranslation.translateValue x :?> JValue
        //show y
        should.equal y 
        <| JValue 0.0

    [<Fact>]
    member this.``translateObject``() =
        let x = Interior("object",[Terminal LEFT_PAREN;Interior("fields",[Interior("fields",[Interior("field",[Terminal(KEY "a");Terminal EXCLAM;Interior("value",[Terminal(NUMBER 0.0)])])]);Terminal ASTERISK;Interior("field",[Terminal(KEY "b!");Terminal EXCLAM;Interior("value",[Terminal NULL])])]);Terminal RIGHT_PAREN])
        let y = UrljsonTranslation.translateObject x
        //show y
        should.equal y <| JObject [JProperty("a",JValue 0.0);JProperty("b!",JValue(null:obj))]

    [<Fact>]
    member this.``translateFields``() =
        let x = Interior("fields",[Interior("fields",[Interior("field",[Terminal(KEY "a");Terminal EXCLAM;Interior("value",[Terminal(NUMBER 0.0)])])]);Terminal ASTERISK;Interior("field",[Terminal(KEY "b!");Terminal EXCLAM;Interior("value",[Terminal NULL])])])
        let y = UrljsonTranslation.translateFields x
        //show y
        should.equal y <| [JProperty("b!",JValue(null:obj));JProperty("a",JValue 0.0);]

    [<Fact>]
    member this.``translateField``() =
        let x = Interior("field",[Terminal(KEY "a");Terminal EXCLAM;Interior("value",[Terminal(NUMBER 0.0)])])
        let y = UrljsonTranslation.translateField x
        //show y
        should.equal y <| JProperty("a",JValue 0.0)

    [<Fact>]
    member this.``translateArray``() =
        let x = Interior("array",[Terminal LEFT_PAREN;Interior("values",[Interior("values",[Interior("value",[Terminal(NUMBER 0.0)])]);Terminal ASTERISK;Interior("value",[Terminal(NUMBER 1.0)])]);Terminal RIGHT_PAREN])
        let y = UrljsonTranslation.translateArray x
        //show y
        should.equal y 
        <| JArray [JValue 0.0; JValue 1.0]

    [<Fact>]
    member this.``translateValues``() =
        let x = Interior("values",[Interior("values",[Interior("value",[Terminal(NUMBER 0.0)])]);Terminal ASTERISK;Interior("value",[Terminal(NUMBER 1.0)])])
        let y = UrljsonTranslation.translateValues x
        //show y
        should.equal y 
        <| [JValue 1.0; JValue 0.0]



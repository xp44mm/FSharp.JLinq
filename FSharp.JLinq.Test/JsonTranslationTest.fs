namespace FSharp.JLinq.Serialization

open Xunit
open Xunit.Abstractions
open FSharp.Literals

open FSharpCompiler.Parsing

open Newtonsoft.Json.Linq
open FSharp.JLinq.Serialization


type JsonTranslationTest(output:ITestOutputHelper) =
    let show res =
        res
        |> Render.stringify
        |> output.WriteLine

    [<Fact>]
    member this.``empty object``() =
        let x = Interior("value",[Interior("object",[Terminal LEFT_BRACE;Terminal RIGHT_BRACE])])
        let y = JsonTranslation.translateValue x
        //show y
        should.equal y (JObject() :> JToken)

    [<Fact>]
    member this.``empty array``() =
        let x = Interior("value",[Interior("array",[Terminal LEFT_BRACK;Terminal RIGHT_BRACK])])
        let y = JsonTranslation.translateValue x
        //show y
        should.equal y <| (JArray() :> JToken)

    [<Fact>]
    member this.``null``() =
        let x = Interior("value",[Terminal NULL])
        let y = JsonTranslation.translateValue x
        //show y
        should.equal y (JValue(null:obj) :> JToken)

    [<Fact>]
    member this.``false``() =
        let x = Interior("value",[Terminal FALSE])
        let y = JsonTranslation.translateValue x
        //show y
        should.equal y (JValue(false) :> JToken)

    [<Fact>]
    member this.``true``() =
        let x = Interior("value",[Terminal TRUE])
        let y = JsonTranslation.translateValue x
        //show y
        should.equal y (JValue(true) :> JToken)

    [<Fact>]
    member this.``empty string``() =
        let x = Interior("value",[Terminal(STRING "")])
        let y = JsonTranslation.translateValue x
        //show y
        should.equal y (JValue("") :> JToken)

    [<Fact>]
    member this.``number``() =
        let x = Interior("value",[Terminal(NUMBER 0.0)])
        let y = JsonTranslation.translateValue x
        //show y
        should.equal y (JValue(0.0) :> JToken)

    [<Fact>]
    member this.``translateObject empty Object``() =
        let x = Interior("object",[Terminal LEFT_BRACE;Terminal RIGHT_BRACE])
        let y = JsonTranslation.translateObject x
        //show y
        should.equal y <| JObject()

    [<Fact>]
    member this.``translateObject``() =
        let x = Interior("object",[Terminal LEFT_BRACE;Interior("fields",[Interior("field",[Terminal(STRING "a");Terminal COLON;Interior("value",[Terminal(NUMBER 0.0)])])]);
        Terminal RIGHT_BRACE])
        let y = JsonTranslation.translateObject x
        //show y
        should.equal y <| JObject(JProperty("a",JValue(0.0)))

    [<Fact>]
    member this.``translateFields single``() =
        let x = Interior("fields",[Interior("field",[Terminal(STRING "a");Terminal COLON;Interior("value",[Terminal(NUMBER 0.0)])])])
        let y = JsonTranslation.translateFields x
        let z = [JProperty("a",JValue(0.0))]

        should.equal y z

    [<Fact>]
    member this.``translateFields``() =
        let x = Interior("fields",[Interior("fields",[
            Interior("field",[Terminal(STRING "a");Terminal COLON;Interior("value",[Terminal(NUMBER 0.0)])])]);Terminal COMMA;
            Interior("field",[Terminal(STRING "b");Terminal COLON;Interior("value",[Terminal NULL])])]);
        let y = JsonTranslation.translateFields x
        let z = [JProperty("b",JValue(null:obj));JProperty("a",JValue(0.0))]
        
        should.equal y z

    [<Fact>]
    member this.``translateField``() =
        let x = Interior("field",[Terminal(STRING "a");Terminal COLON;Interior("value",[Terminal(NUMBER 0.0)])])
        let y = JsonTranslation.translateField x
        //show y
        should.equal y <| JProperty("a",JValue(0.0))

    [<Fact>]
    member this.``translateArray empty array``() =
        let x = Interior("array",[Terminal LEFT_BRACK;Terminal RIGHT_BRACK])
        let y = JsonTranslation.translateArray x
        //show y
        should.equal y <| JArray()

    [<Fact>]
    member this.``translateArray``() =
        let x = Interior("array",[Terminal LEFT_BRACK;Interior("values",[Interior("value",[Terminal(NUMBER 0.0)])]);Terminal RIGHT_BRACK])
        let y = JsonTranslation.translateArray x
        //show y
        should.equal y <| JArray(JValue(0.0))

    [<Fact>]
    member this.``translateValues single``() =
        let x = Interior("values",[Interior("value",[Terminal(NUMBER 0.0)])])
        let y = JsonTranslation.translateValues x
        //show y
        should.equal y [JValue(0.0)]

    [<Fact>]
    member this.``translateValues``() =
        let x = Interior("values",[Interior("values",[
            Interior("value",[Terminal(NUMBER 0.0)])]);Terminal COMMA;
            Interior("value",[Terminal(NUMBER 1.0)])]);
        let y = JsonTranslation.translateValues x
        //show y
        should.equal y [JValue(1.0);JValue(0.0)]



namespace FSharp.JLinq

open Xunit
open Xunit.Abstractions
open FSharp.JLinq
open FSharp.Literals
open FSharp.xUnit
open System
open Newtonsoft.Json.Linq
open FSharp.JLinq.Serialization
open FSharp.JLinq.JTokenAdapters

type DemoTest(output: ITestOutputHelper) =
    let show outp =
        outp
        |> Render.stringify
        |> output.WriteLine

    [<Fact>]
    member this.``Deserializing test``() =
        let text = """
        {
          "Name": "Apple",
          "ExpiryDate": "2008-12-28T00:00:00",
          "Price": 3.99,
          "Sizes": [
            "Small",
            "Medium",
            "Large"
          ]
        }
        """

        let value = ObjectConverter.deserialize<{|Name:string;ExpiryDate:DateTimeOffset;Price:float;Sizes:string[]|}> text

        let  y = {|
            ExpiryDate=DateTimeOffset(2008,12,28,0,0,0,0,TimeSpan(0,8,0,0,0));
            Name="Apple";
            Price=3.99;Sizes=[|"Small";"Medium";"Large"|]
            |}

        should.equal y value

    [<Fact>]
    member this.``Serialization test``() =
        let value = {|
            ExpiryDate = DateTimeOffset(2008,12,28,0,0,0,0,TimeSpan(0,8,0,0,0));
            Name = "Apple";
            Price = 3.99;
            Sizes = [|"Small";"Medium";"Large"|]
            |}

        let y = ObjectConverter.serialize value

        let text = "{\"ExpiryDate\":\"2008-12-28T00:00:00+08:00\",\"Name\":\"Apple\",\"Price\":3.99,\"Sizes\":[\"Small\",\"Medium\",\"Large\"]}"

        should.equal y text

    [<Fact>]
    member this.``object convert to jtoken test``() =
        let x = {|
            ExpiryDate = DateTimeOffset(2020,3,31,9,2,18,0,TimeSpan(0,8,0,0,0))
            Name = "Apple"
            Price = 3.99
            Sizes = [|"Small";"Medium";"Large"|]
            |}
        let e = JObject [
            JProperty("ExpiryDate",JValue x.ExpiryDate)
            JProperty("Name",JValue "Apple")
            JProperty("Price",JValue 3.99)
            JProperty("Sizes",JArray [
                JValue "Small";
                JValue "Medium";
                JValue "Large"])]
        let y = ObjectConverter.read x :?> JObject
        should.equal e y

    [<Fact>]
    member this.``object dynamically convert to jtoken test``() =
        let x = {|
            ExpiryDate = DateTimeOffset(2020,3,31,9,2,18,0,TimeSpan(0,8,0,0,0))
            Name = "Apple"
            Price = 3.99
            Sizes = [|"Small";"Medium";"Large"|]
            |}
        let e = JObject [
            JProperty("ExpiryDate",JValue x.ExpiryDate)
            JProperty("Name",JValue "Apple")
            JProperty("Price",JValue 3.99)
            JProperty("Sizes",JArray [
                JValue "Small";
                JValue "Medium";
                JValue "Large"])]

        let y = JTokenWriter.mainWriteDynamic JTokenWriter.writers (x.GetType()) x :?> JObject
        should.equal e y

    [<Fact>]
    member this.``jtoken dynamically convert to object test``() =
        let x = JObject [
            JProperty("ExpiryDate",JValue(DateTimeOffset(2020,3,31,9,2,18,0,TimeSpan(0,8,0,0,0))))
            JProperty("Name",JValue "Apple")
            JProperty("Price",JValue 3.99)
            JProperty("Sizes",JArray [
                JValue "Small";
                JValue "Medium";
                JValue "Large"])]
        let e = {|
            ExpiryDate = (x.["ExpiryDate"] :?> JValue).Value :?> DateTimeOffset
            Name = "Apple"
            Price = 3.99
            Sizes = [|"Small";"Medium";"Large"|]
            |} 
        let ty = typeof<{|Name:string;ExpiryDate:DateTimeOffset;Price:float;Sizes:string[]|}>
        let y = JTokenReader.mainReadDynamic JTokenReader.readers ty x

        should.equal y (box e)

    [<Fact>]
    member this.``jtoken convert to object test``() =
        let x = JObject [
            JProperty("ExpiryDate",JValue(DateTimeOffset(2020,3,31,9,2,18,0,TimeSpan(0,8,0,0,0))))
            JProperty("Name",JValue "Apple")
            JProperty("Price",JValue 3.99)
            JProperty("Sizes",JArray [
                JValue "Small";
                JValue "Medium";
                JValue "Large"])]
        let e = {|
            ExpiryDate = (x.["ExpiryDate"] :?> JValue).Value :?> DateTimeOffset
            Name = "Apple"
            Price = 3.99
            Sizes = [|"Small";"Medium";"Large"|]
            |}

        let y = ObjectConverter.write x

        should.equal e y

    [<Fact>]
    member this.``Navigate test``() =

        let tree = JObject[
            JProperty("ExpiryDate",JValue "2008-12-28T00:00:00")
            JProperty("Name",JValue "Apple")
            JProperty("Price",JValue 3.99)
            JProperty("Sizes",JArray [
                JValue "Small";
                JValue "Medium";
                JValue "Large"])]

        let name = tree.["Name"] :?> JValue
        should.equal name (JValue "Apple")

        let size0 = tree.["Sizes"].[0] :?> JValue
        should.equal size0 (JValue "Small")

    [<Fact>]
    member this.``htmlColor test``() =
        let tree = JObject [
            JProperty("Blue",JValue 0.0)
            JProperty("Green",JValue 0.0)
            JProperty("Red",JValue 255.0)
            ]
        let x = JsonRender.stringify tree
        let y = JsonDriver.parse x :?> JObject
        //show y
        should.equal y tree

    [<Fact>]
    member this.``roles test``() =
        let tree = JArray [JValue "User";JValue "Admin"]
        let x = JsonRender.stringify tree
        let y = JsonDriver.parse x :?> JArray
        //show y
        should.equal y tree

    [<Fact>]
    member this.``dailyRegistrations test``() =
        let tree = JObject [
            JProperty("2014-06-01T00:00:00",JValue 23.0)
            JProperty("2014-06-02T00:00:00",JValue 50.0)
            ]
        let x = JsonRender.stringify tree
        let y = JsonDriver.parse x :?> JObject
        //show y
        should.equal y tree

    [<Fact>]
    member this.``city test``() =
        let tree = JObject [
            JProperty("Name",JValue "Oslo")
            JProperty("Population",JValue 650000.0)
            ]

        let x = JsonRender.stringify tree
        let y = JsonDriver.parse x :?> JObject
        //show y
        should.equal y tree

    [<Fact>]
    member this.``map name test``() =
        let tree = JObject [
            JProperty("Blue",JValue 0.0)
            JProperty("Green",JValue 1.0)
            JProperty("Red",JValue 255.0)]
        let y = tree.["Blue"] :?> JValue
        //show y
        should.equal y <| JValue 0.0



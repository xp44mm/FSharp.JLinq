# Serializing and Deserializing JSON

The quickest method of converting between JSON text and a F# object is using the `ObjectConverter`. The `ObjectConverter` converts F# objects into their JSON equivalent and back again by mapping the F# object property names to the JSON property names and copies the values for you.


## ObjectConverter

For simple scenarios where you want to convert to and from a JSON string, the `serialize<>` and `deserialize<>` methods on `ObjectConverter` provide an easy-to-use wrapper over JsonSerializer.

Serializing JSON with `ObjectConverter`

```F#
let value = {|
    ExpiryDate=DateTimeOffset(2008,12,28,0,0,0,0,TimeSpan(0,8,0,0,0));
    Name="Apple";
    Price=3.99;
    Sizes=[|"Small";"Medium";"Large"|]
|}
let y = ObjectConverter.serialize value
let text = """{"ExpiryDate":"2008/12/28 0:00:00 +08:00","Name":"Apple","Price":3.99,"Sizes":["Small","Medium","Large"]}"""
Should.equal y text
```
Deserializing JSON with `ObjectConverter`

```F#
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
            Price=3.99;
            Sizes=[|"Small";"Medium";"Large"|]
|}

Should.equal y value
```


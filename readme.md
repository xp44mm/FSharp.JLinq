# FSharp.JLinq

`FSharp.JLinq` is a library to enhance JToken located in `Newtonsoft.Json.Linq`. 

其主要功能有：

- F# 对象与`JToken`的互相转换。

- 自定义F#对象与`JToken`的相互转换规则。避免访问底层的Converter自定义方式。

- `JToken`至查询字符串的序列化与反序列化。

## Install over NuGet

```powershell
Install-Package FSharp.JLinq
```

## Get Started

F# object convert to `JToken` object：

```F#
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

```

`JToken`对象转化为F#对象（泛型，反射）：

```F#
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
```

当你的对象的类型信息是由反射获得而来的，不能放在类型参数中，则用反射的方法转化对象：

object dynamically convert to jtoken

```F#
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
```

jtoken dynamically convert to object

```F#
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
```

泛型方法调用了动态方法，动态方法的第一个参数是每种类型的读写实现的接口，第二个参数是类型信息，第三个参数是输入数据。

类库的作者不可能穷尽每种类型的实现，类库的使用者可以补充自己所需要的读写实现。补充的读写实现，放到列表的开头将覆盖现有的实现逻辑。

```F#
type JTokenWriterAdapter = 
    abstract filter: ty:Type * value:obj -> bool
    abstract write: loop:(Type -> obj -> JToken) * ty:Type * value:obj -> JToken

type JTokenReaderAdapter =
    abstract filter: targetType:Type * json:JToken -> bool
    abstract read: loop:(Type -> JToken -> obj) * targetType:Type * json:JToken -> obj

```

客制化适配器接口只需实现当前层次的逻辑，如果类型有泛型参数，则可以调用`loop`进行递归。比如可空类型：

```F#
type NullableWriterAdapter () = 
    interface JTokenWriterAdapter with
        member _.filter(ty,value) = ty.IsGenericType && ty.GetGenericTypeDefinition() = typedefof<Nullable<_>>
        member _.write(loop, ty, value) = 
            if value = null then
                JValue(null:obj) :> JToken
            else
                let underlyingType = ty.GenericTypeArguments.[0]
                loop underlyingType value
```

当定义好了自己的适配器后，只需要将其放在适配器集合的开头，传递给动态主动作方法：

```F#
let readers = NullableWriterAdapter() :: JTokenReader.readers
let y = JTokenReader.mainReadDynamic readers ty x
```

## Url

Restful GET方法具有幂等性，但只能把参数放在Url中传递，且Url的长度是有限的，JSON是表示结构化数据的最简洁格式，我们保持结构不变，进一步替换关键字，以避免转义后的字符串变长，主要改变是：

- 字符串由双引号变为单引号。转义字符用`~`波浪线代替反斜杠。
- 当属性名称满足标识符规则时，属性名称可以不括单引号。
- 大括号，中括号用小括号代替。
- 星号代替逗号。
- 叹号代替冒号。
- `()`表示空数组。
- `(!)`表示空对象。

解析url中的查询字符串为`JToken`对象：


序列化`JToken`对象为url中的查询字符串：

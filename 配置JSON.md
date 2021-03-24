# 配置JSON

内置的JSON功能可以满足基本的F#数据类型序列化的需求。当然，如果你需要，是可以增加或删除读写器来实现功能或改进性能的。F#序列化和反序列化不是直接打印成文本的，而是先将F#数据类型转化为树结构，树结构再打印成文本。介绍JSON配置方法之前，先学习核心类型的用法。

## 核心类型`Json`

这个树结构是：

```F#
[<RequireQualifiedAccess>]
type Json =
| Object of list<string*Json>
| Array  of Json list
| Null
| False
| True
| String of string
| Number of float 
```

该类型位于`FSharpCompiler.Json`名字空间，如果是对象可以用字段名称索引字段值，如：

```F#
let x = Json.Object ["name",Json.String "abcdefg"; "age", Json.Number 18.0]
let y = x.["name"]
Should.equal y <| Json.String "abcdefg"
```

如果是数组可以用索引来引用元素值：

```F#
let x = Json.Array [Json.Number 1.0;Json.Number 2.0;Json.Number 3.0]
let y = x.[1]
Should.equal y <| Json.Number 2.0
```

`Json`类型是JSON的树形表达，可以解析JSON文本：

```F#
let x = """{"a":0,"b":null}"""
let y = JsonSerializer.parse x
//show y
Should.equal y 
<| Json.Object ["a",Json.Number 0.0;"b",Json.Null;]
```

解析的反向操作，序列化为JSON文本：

```F#
let x = Json.Array [Json.Number 0.0;Json.Number 1.0] 
let y = JsonSerializer.stringify x
Should.equal y "[0,1]"
```

## 读者

JSON到文本的序列化，和反序列化已经内置，我们只需要客制从F#类型到树类型`Json`的读取器和写入器即可。读取器和写入器分别实现了统一的接口。读取器的接口定义如下：

```F#
/// read from fsharp value to json
type ObjReader = 
    abstract filter: valueType:Type * value:obj -> bool
    abstract read: loopRead:(Type -> obj -> Json) * valueType:Type * value:obj -> Json
```

成员`filter`函数，Gets a value indicating whether this `ObjReader` can read an object to JSON. 
`valueType` is Type of the object. 
`value` is value of the object. 

成员方法`read`，返回数据的`Json`。

`loopRead` 递归调用读取器时所用的读取方法。输入要读取数据的类型和值信息。

`valueType` is Type of the object. 

`value` is value of the object. 

## 作者

```F#
/// write to fsharp value from json
type ObjWriter =
    abstract filter: targetType:Type * json:Json -> bool
    abstract write: loopWrite:(Type -> Json -> obj) * targetType:Type * json:Json -> obj
```

成员`filter`函数，Gets a value indicating whether this `ObjReader` can write to an object from JSON. 
`targetType` is Type of the object. 
`json` is data source of the object. 

成员方法`write`，返回数据。

`loopWrite` 递归作者。用它继续写嵌套的json数据。

`targetType` is Type of the object. 

`json` is data source of the object. 


# 将json数据放入url中

restful GET用于获取指定的数据，具有幂等性。但是这个方法没有请求体，数据放在url中发送，而url的长度是有限的。通常，我们发送数据用queryString格式来，但是当发送结构化数据时，这个格式会遍历每个路径，路径名占用相当的长度。所以，我们想是否可以将json格式用在url中替代query格式。但是json中的标点符号都需要转义，一是增加长度。二是可读性不强。再有json格式的键值如果简单的情况下，不需要加引号括起来。三是大多数时候我们还是只发送单层的键值对，所有query语法仍有他的优点。本文将介绍一种二者优点结合的序列化格式。

## Urljson格式

Urljson is a plain-text format for data storage. Urljson用途在于把json文本放入url中。

## Data Format

Urljson stores data as plain text. Its grammar 类似 the grammar of JSON. For example:

```json
{
    "first": "Jane",
    "last": "Porter",
    "married": true,
    "born": 1890,
    "friends": [ "Tarzan", "Cheeta" ]
}
```

convert to Urljson:

```
"(first!'Jane'*last!'Porter'*married!true*born!1890*friends!('Tarzan'*'Cheeta'))"
```

## 格式说明

The grammar can be transcribed as follows:

```
value : object
	  | array
	  | NULL
	  | FALSE
	  | TRUE
	  | STRING
	  | NUMBER
	  ;
object : "(" "!" ")"
       | "(" fields ")"
       ;
fields : field
	   | fields "*" field
	   ;
field : KEY "!" value
	  ;
array : "(" ")"
      | "(" values ")"
      ;
values : value
	   | values "*" value
       ;
```


Urljson与json格式的区别：

非空对象`{}`或非空数组`[]`两端的括号都被替换为`()`。

空数组`[]`被替换为`()`，空对象`{}`被替换为`(!)`。

键值分隔符`:`替换为`!`

成员分隔符`,`替换为`*`

双引号字符串`"`替换为单引号字符串`'`。字符串中需要转义的符号有后斜杠，单引号，控制字符。

属性键单引号是可选的。除非当包含控制字符，空白字符，标点符号时必须用单引号括起来。

其他基元类型：数字`number`，布尔真`true`，布尔假`false`，空`null`，他们都和JSON格式一样。没有变化。


## 生成查询字符串

它会序列化传入的 `obj` 中以下类型的值：：string | number | boolean 。 任何其他的输入值都将会被强制转换为空字符串。

通过url传递对象数据使用查询字符串。这是标准用法。各种现有开发工具都支持解析。查询字符串以`?`开头，以`&`分隔开相邻参数，以`=`分隔参数名与参数值。

当对象涉及多层嵌套是查询字符串的参数名变得冗余。所以我们传递对象的第一层成员用查询字符串格式，当对象成员仍然是对象时（包括数组），用Urljson格式。

* 如果成员值为字符串类型，则参数值为成员值，不加引号。例如，`{x:"abc"}`将表示为`?x=abc`

* 如果成员值为`null`、`function`、`undefined`，则字段值转化为空字符串。

* 如果成员值为基元类型，对应的参数值是该成员值的字符串化表示。

* 如果成员值为对象或数组，则用Urljson格式字符化为参数值。注，这里和queryString有区别，即使是基元数组也用jzon格式字符化，而不是多个同名参数顺序排列。


### 编写控制器的操作方法

查询字符串解析时，需要知道每个参数的类型，缺省类型为`string`，即不解析。

当参数名是简单的，参数值是基元类型时，JZON格式与查询字符串相同。也就是可以使用asp.net core 自带的匹配功能，如`?x=0&y=true&z=abc`，可以传给`action(x:float,y:bool,z:string)`。

查询字符串用多个相同名称的字段表示简单数组，而JZON格式不同，它将简单数组序列化并放到一个字段中。

### 示例一：

当一个对象的成员全部都是基元类型时，兼容forms格式的字符串。
生成的请求的查询字符串：

```F#
?press=100760&temp=40.2&humid=6.2
```

这个字符串与forms形式的字符串没有什么不同，所以可以使用ASP.NET默认的解析方法。客户端的请求：

```javascript
 const airVol = {
     press: 100760,
     temp: 40.2,
     humid: 6.2,
 };

 ajax.getJSON(mainUrl('desulphur', 'airVol') + queryString(airVol))

```

一个对象被表示为查询字符串，在这里对象是`airVol`，`queryString()`将其序列化成上面列出的查询字符串。

函数`queryString`已经被定义好，源代码位于queryString文件夹下面。

控制器的操作方法：

```F#
[<HttpGet>]
member __.airVol(press:float, temp:float, humid:float) =
    Atmosphere.airVol press temp humid
```

### 示例二：

当
生成的请求的查询字符串：

```F#
?sinlet=kind*~rectangle~!rectangle*(width*10!height*10)&soutlet=kind*~rectangle~!rectangle*(width*15!height*15)&angle=15
```
翻译成javascript对象：

```javascript
{
     sinlet: {"kind":"rectangle","rectangle":{"width":10,"height":10}},
     soutlet: {"kind":"rectangle","rectangle":{"width":15,"height":15}},
     angle: 15
}
```

控制器的操作方法：

```F#
[<HttpGet>]
member this.Reducer() =
    task {
        let input = QueryCollection.toObject<Reducer.Input>(this.Request.Query)
        let result = Reducer.calc input
        return result
    }
```

这个控制器的特点，第一没有输入参数，查询数据直接通过`this.Request.Query`获取。第二，将获取的数据通过一个函数直接转换成目标对象。这个函数是`QueryCollection.toObject<>()`，已经定义好了。

下面看看类型的定义：

```F#
type Input =
    {
        sinlet:Shape
        soutlet:Shape
        angle:float
    }
```

这个类型是F#记录，对应着json对象。`Shape`类型的序列化是自定义的，通过`ShapeConverter`，它在`Startup`中被注册，继承自`JsonConverter<>`，详见json.net用法。
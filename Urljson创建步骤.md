# 创建说明

添加新项目：C#、 ASP.NET Core Web 应用程序，

选择 .NET Core、 ASP.NET Core 最新版本，Angular模板，

HTTPS取消选中，此项可以在属性中修改。点确定按钮生成项目。

项目的包开始时会有黄色三角号，此时项目正在下载包，表示包还没有下载完成，等待片刻符号会自行消失。

修改`Startup.cs`文件

第一，修改方法`ConfigureServices`，不需要视图功能所以替换如下：

```C#
//services.AddControllersWithViews();
services.AddControllers()
    ;
```

第二，修改方法`Configure`，重新配置开发服务器：

```cs
if (env.IsDevelopment())
{
    //spa.UseAngularCliServer(npmScript: "start");
    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
}
```

最后，删除不必要的`Using`语句。



配置`.gitignore`

```
# ClientApp
/ClientApp/dist
/ClientApp/node_modules
```

删除ClientApp中的所有文件，复制为你自己的客户端文件夹内容。

# 客户端文件夹说明

入口文件是`package.json`。

基本构架为webpack，有三个配置文件`webpack.common.js`，`webpack.dev.js`，`webpack.prod.js`。

Babel配置文件`babel.config.js`。

代码格式配置文件`prettier.config.js`。




# 运行说明

首先在`ClientApp`文件夹下，启动客户端的服务，运行命令：

```bash
> npm start
  ...
  Compiled successfully.
```

然后，在VS中执行调试，F5或shift+F5。



# 复制chrome f12里面打印的對象字面量

在chrome的控制台里

```js
let a = { 1:1 } 
copy(a)
```

在新的命令行，Ctrl+V，將粘貼如下結果：

```js
{
  "1": 1
}
```

回車，執行命令，會輸出此對象的樹，在打印對象的樹範圍内右鍵，彈出菜單有選項`Store as global variable`，會將其保存到一個變量，例如第一次會保存到`temp1`變量。

`$_`表示命令行最後一個表达式的值，保存全局變量后，執行如下命令：

```js
copy($_)
```

表示複製最後一個表達式的值到剪貼板。

命令行輸入元素的id值，可以得到這個元素。


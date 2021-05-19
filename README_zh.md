[English](https://github.com/Afanyiyu/SearchSharp/blob/master/README.md)

<div align="center">
  <h1><a href="https://github.com/Afanyiyu/SearchSharp" target="_blank">SearchSharp</a></h1>
  <p>轻量、高效的关键词搜索工具。</p>

![Workflow](https://img.shields.io/github/workflow/status/Afanyiyu/SearchSharp/build?style=flat-square)
[![Nuget](https://img.shields.io/nuget/v/SearchSharp?style=flat-square)](https://www.nuget.org/packages/SearchSharp)
[![Downloads](https://img.shields.io/nuget/dt/SearchSharp?style=flat-square)](https://www.nuget.org/packages/SearchSharp)
[![License](https://img.shields.io/github/license/Afanyiyu/SearchSharp?style=flat-square)](https://github.com/Afanyiyu/SearchSharp/blob/master/LICENSE)

</div>

# 这是……

SearchSharp 是一个轻量、高效的关键词搜索工具，它可以用来进行关键词搜索，从一组对象中搜索到键值相匹配的对象。

# 这不是……

- SearchSharp **不是搜索引擎**。它只是一个搜索库，允许你在一小组对象中进行关键字搜索。

- SearchSharp **不是搜索工具**。SearchSharp 不含 CLI 或 GUI，你也不能用它来搜索文件或网络。

- SearchSharp **不是一个全文搜索工具**。尽管你可以在搜索对象中添加字符串并且设置 `x => x`，你不应该使用 SearchSharp 进行全文搜索。这会导致严重的内存和性能问题。

# 功能

## 轻量且高效

参见 [Benchmark](#Benchmark)。

## 易于使用

参见 [使用](#使用)。

## 支持部分搜索

键入...|以搜索...
-|-
arp|SearchSharp
sea|Search
searchwithoutspace|Search Without Space

## 支持汉字/拼音/拼音首字母搜索

键入...|以搜索...
-|-
sousuoneirong|搜索内容
sharpss|SearchSharp 搜索
pptyshi|PPT 演示
szmss|首字母搜索
py汉字hhsousuo|拼音汉字混合搜索
buf|部分拼音搜索

# 安装

你可以从 [NuGet](https://www.nuget.org/packages/SearchSharp) 或 [GitHub Packages](https://github.com/Afanyiyu/SearchSharp/packages/797283) 上安装 SearchSharp。

```ps1
# 使用 .NET CLI
dotnet add package SearchSharp
# 或使用 NuGet 程序包管理器
Install-Package SearchSharp
```

# 使用

## 创建搜索索引

要创建搜索索引，我们需要提供 **数据类型** 和 **数据集**。

比如：

```cs
// 数据类型
public record MyData
{
    public string MySearchKey;
    public int MyValue;
}

// 创建数据集
List<MyData> myDataSet = new()
{
    new() {MySearchKey = "TheFirstNumber", MyValue = 1},
    new() {MySearchKey = "TheSecondNumber", MyValue = 2}
};
```

首先，初始化一个新的 `SearchStorage<T>`，其中 `T` 就是我们的数据类型。本例中，`T` 是 `MyData`。

```cs
SearchStorage<MyData> storage = new();
```

如果你需要汉字搜索，设置 `Mode` 为 `CharParseMode.EnableChineseCharSearch`。

```cs
SearchStorage<MyData> storage = new()
{
    Mode = CharParseMode.EnableChineseCharSearch
};
```

如果你需要拼音搜索，设置 `Mode` 为 `CharParseMode.EnablePinyinSearch`。这将同时启用 `CharParseMode.EnableChineseCharSearch`。

```cs
SearchStorage<MyData> storage = new()
{
    Mode = CharParseMode.EnablePinyinSearch
};
```

## 添加数据

接着，往 `storage` 里添加数据。

### 基本

```cs
foreach (MyData data in myDataSet) storage.Add(data.MySearchKey, data);
```

### 重载

`SearchStorage<T>.Add()` 有许多重载方法可供使用。

重载|用法
-|-
`Add(string key, T value)`|`storage.Add("key", item);`

### `Tuple` 和 `ValueTuple` 支持

重载|用法
-|-
`Add((string Key, T Value) item)`|`storage.Add(("key", item));`
`Add(Tuple<string, T> item)`|`storage.Add(new Tuple<string, MyData>("key", item));`

### `KeyValuePair` 支持

重载|用法
-|-
`Add(KeyValuePair<string, T> item)`|`foreach (KeyValuePair<string, MyData> pair in myDataDictionary) storage.Add(pair);`

### `IEnumerable<T>` 支持（类似 `DynamicData.AddRange()`）

重载|用法
-|-
`Add(IEnumerable<(string Key, T Value)> items)`|`storage.Add(myValueTupleList);`
`Add(IEnumerable<Tuple<string, T>> items)`|`storage.Add(myTupleList);`
`Add(IEnumerable<KeyValuePair<string, T>> items)`|`storage.Add(myDictionary);`

### 使用 Lambda 获取键

重载|用法
-|-
`Add(T item, Func<T, string> key)`|`storage.Add(item, x => x.Key);`
`Add(IEnumerable<T> items, Func<T, string> key)`|`storage.Add(itemList, x => x.Key);`

### 链式调用

```cs
storage
    .Add("TheFirstItem", item1)
    .Add("TheSecondItem", item2);
```

在添加完数据之后，索引也随之建立。现在，我们可以进行搜索了。

## 搜索

使用 `SearchStorage<T>.Search()` 方法进行搜索。

```cs
HashSet<MyData> result = storage.Search("num");
```

`SearchStorage<T>.Search()` 将会返回一个 `HashSet<T>` 存放结果。

这是在即时窗口中 `result` 显示的值：

```
result
Count = 2
    [0]: {MyData { MySearchKey = TheFirstNumber, MyValue = 1 }}
    [1]: {MyData { MySearchKey = TheSecondNumber, MyValue = 2 }}
```

## 完整示例

参见 [SearchSharpIntro](https://github.com/Afanyiyu/SearchSharp/tree/master/samples/SearchSharpIntro)。

# Benchmark

- NormalSearchBenchmark: 搜索仅包含英文字符的项目。

- ChineseCharSearchBenchmark: 启用 `EnableChineseCharSearch` 选项，使用汉字搜索包含中文字符的项目。

- PinyinSearchBenchmark: 启用 `EnablePinyinSearch` 选项，使用汉字搜索包含中文字符的项目。

## 结果

``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.21318
Intel Core i5-10400F CPU 2.90GHz, 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=5.0.203
  [Host]     : .NET Core 5.0.6 (CoreCLR 5.0.621.22011, CoreFX 5.0.621.22011), X64 RyuJIT
  CoreRt 5.0 : .NET 5.0.29408.02 @BuiltBy: dlab14-DDVSOWINAGE075 @Branch: master @Commit: 4ce1c21ac0d4d1a3b7f7a548214966f69ac9f199, X64 AOT

Job=CoreRt 5.0  Runtime=CoreRt 5.0  

```

|                     Method | ItemCount |             Mean |          Error |         StdDev |              Min |              Max |           Median |
|--------------------------- |---------- |-----------------:|---------------:|---------------:|-----------------:|-----------------:|-----------------:|
|      **NormalSearchBenchmark** |       **100** |         **86.48 μs** |       **0.262 μs** |       **0.205 μs** |         **86.20 μs** |         **86.80 μs** |         **86.45 μs** |
|      **NormalSearchBenchmark** |     **10000** |      **7,959.13 μs** |     **149.663 μs** |     **153.693 μs** |      **7,648.81 μs** |      **8,228.19 μs** |      **7,928.38 μs** |
|      **NormalSearchBenchmark** |    **100000** |     **72,560.96 μs** |     **585.133 μs** |     **488.612 μs** |     **71,867.97 μs** |     **73,405.86 μs** |     **72,618.13 μs** |
|                            |           |                  |                |                |                  |                  |                  |
| **ChineseCharSearchBenchmark** |       **100** |         **99.74 μs** |       **0.112 μs** |       **0.087 μs** |         **99.62 μs** |         **99.84 μs** |         **99.73 μs** |
| **ChineseCharSearchBenchmark** |     **10000** |     **11,510.43 μs** |     **137.843 μs** |     **128.939 μs** |     **11,326.86 μs** |     **11,767.57 μs** |     **11,480.44 μs** |
| **ChineseCharSearchBenchmark** |    **100000** |    **115,347.79 μs** |   **1,956.152 μs** |   **1,633.476 μs** |    **113,070.74 μs** |    **119,427.30 μs** |    **115,307.96 μs** |
|                            |           |                  |                |                |                  |                  |                  |
|      **PinyinSearchBenchmark** |       **100** |      **9,122.63 μs** |      **49.976 μs** |      **39.018 μs** |      **9,076.81 μs** |      **9,212.18 μs** |      **9,111.95 μs** |
|      **PinyinSearchBenchmark** |     **10000** |  **1,018,983.41 μs** |  **18,426.030 μs** |  **27,579.215 μs** |    **987,004.70 μs** |  **1,088,789.00 μs** |  **1,011,044.00 μs** |
|      **PinyinSearchBenchmark** |    **100000** | **10,185,748.11 μs** | **133,558.292 μs** | **111,527.223 μs** | **10,041,883.80 μs** | **10,461,182.00 μs** | **10,159,970.30 μs** |

# Bug & 问题

请自由提交问题。

# 贡献

我们欢迎合并请求！请自由在这个项目上做贡献。

# 许可

MIT

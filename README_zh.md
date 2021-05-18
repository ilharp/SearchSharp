[English](https://github.com/Afanyiyu/SearchSharp/blob/master/README.md)

<div align="center">
  <h1><a href="https://github.com/Afanyiyu/SearchSharp" target="_blank">SearchSharp</a></h1>
  <p>轻量、高效的关键词搜索工具。</p>

![Workflow](https://img.shields.io/github/workflow/status/Afanyiyu/SearchSharp/build?style=flat-square)
[![Nuget](https://img.shields.io/nuget/v/SearchSharp?style=flat-square)](https://www.nuget.org/packages/SearchSharp)
[![Downloads](https://img.shields.io/nuget/dt/SearchSharp?style=flat-square)](https://www.nuget.org/packages/SearchSharp)
[![License](https://img.shields.io/github/license/Afanyiyu/SearchSharp?style=flat-square)](https://github.com/Afanyiyu/SearchSharp/blob/master/LICENSE)

</div>

## 这是……

SearchSharp 是一个轻量、高效的关键词搜索工具，它可以用来进行关键词搜索，从一组对象中搜索到键值相匹配的对象。

## 这不是……

- SearchSharp **IS NOT a search engine**. It's a search utility to let you search in a small set of objects.

- SearchSharp **IS NOT a search tool**. SearchSharp doesn't have a CLI or GUI, and you cannot use SearchSharp to search files or web directly.

- SearchSharp **IS NOT a Full Text search library**. Although you can add `string`s into the search storage and set `x => x`, you shouldn't use SearchSharp to perform Full Text search. This can cause memory and performance issues.

## Features

### Lightweight and Fast

See [Benchmark](#Benchmark).

### Support Part-Spelling Search

Type...|To Search...
-|-
arp|SearchSharp
sea|Search
searchwithoutspace|Search Without Space

### Support Chinese/Pinyin Search

Type...|To Search...
-|-
ssnr|搜索内容
sharpss|SearchSharp 搜索
pptyshi|PPT 演示
szmss|首字母搜索
py汉字hhsousuo|拼音汉字混合搜索

## Install

You can install SearchSharp from [NuGet](https://www.nuget.org/packages/SearchSharp) or [GitHub Packages](https://github.com/Afanyiyu/SearchSharp/packages/797283).

```ps1
# Use .NET CLI
dotnet add package SearchSharp
# Or use Package Manager
Install-Package SearchSharp
```

## Usage

TODO

## Benchmark

- NormalSearchBenchmark: Search for items that contain only letters.

- ChineseCharSearchBenchmark: Search for items that contain Chinese characters, with `EnableChineseCharSearch` enabled.

- PinyinSearchBenchmark: Search for items that contain Chinese characters, with `EnablePinyinSearch` enabled.

### Result

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

## Bugs & Issues

Feel free to open issues.

## Contributions

PRs are welcome! Feel free to contribute on this project.

## LICENSE

MIT

using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace SearchSharp.Test.Benchmark
{
    [SimpleJob(RuntimeMoniker.CoreRt50)]
    [MarkdownExporter]
    [RPlotExporter]
    [MinColumn]
    [MaxColumn]
    [MeanColumn]
    [MedianColumn]
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByMethod)]
    public class SearchBenchmark
    {
        private const int SearchTextCount = 1000;
        private Random _random;
        private SearchBenchmarkData _searchBenchmarkData;

        [Params(100, 10000, 100000)]
        public int ItemCount;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _searchBenchmarkData = new(ItemCount, SearchTextCount);
            _random = new();
        }

        [Benchmark]
        public HashSet<string> NormalSearchBenchmark()
        {
            SearchStorage<string> storage = new();
            storage.Add(_searchBenchmarkData.SearchItemData, x => x);

            return storage.Search(_searchBenchmarkData.SearchTextData[_random.Next(SearchTextCount)]);
        }

        [Benchmark]
        public HashSet<string> ChineseSearchBenchmark()
        {
            SearchStorage<string> storage = new()
            {
                EnableChinesePinyinSearch = true
            };
            storage.Add(_searchBenchmarkData.ChineseSearchItemData, x => x);

            return storage.Search(_searchBenchmarkData.ChineseSearchTextData[_random.Next(SearchTextCount)]);
        }
    }

    public static class Program
    {
        public static void Main(string[] args) =>
            BenchmarkRunner.Run<SearchBenchmark>();
    }
}

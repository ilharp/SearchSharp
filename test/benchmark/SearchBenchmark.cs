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

        private readonly SearchStorage<string> _normalSearchData = new();
        private readonly SearchStorage<string> _chineseCharSearchData = new()
        {
            Mode = CharParseMode.EnableChineseCharSearch
        };
        private readonly SearchStorage<string> _pinyinSearchData = new()
        {
            Mode = CharParseMode.EnablePinyinSearch
        };

        [Params(100, 10000, 100000)]
        public int ItemCount;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _searchBenchmarkData = new(ItemCount, SearchTextCount);
            _random = new();

            _normalSearchData.Add(_searchBenchmarkData.SearchItemData, x => x);
            _chineseCharSearchData.Add(_searchBenchmarkData.ChineseSearchItemData, x => x);
            _pinyinSearchData.Add(_searchBenchmarkData.ChineseSearchItemData, x => x);
        }

        [Benchmark]
        public HashSet<string> NormalSearchBenchmark() =>
            _normalSearchData.Search(_searchBenchmarkData.SearchTextData[_random.Next(SearchTextCount)]);

        [Benchmark]
        public HashSet<string> ChineseCharSearchBenchmark() =>
            _chineseCharSearchData.Search(_searchBenchmarkData.ChineseSearchTextData[_random.Next(SearchTextCount)]);

        [Benchmark]
        public HashSet<string> PinyinSearchBenchmark() =>
            _pinyinSearchData.Search(_searchBenchmarkData.ChineseSearchTextData[_random.Next(SearchTextCount)]);
    }

    public static class Program
    {
        public static void Main(string[] args) =>
            BenchmarkRunner.Run<SearchBenchmark>();
    }
}

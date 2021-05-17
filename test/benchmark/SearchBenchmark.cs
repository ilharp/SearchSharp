using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using SearchSharp;
using SearchSharp.Test.Benchmark;

namespace benchmark
{
    [SimpleJob(RuntimeMoniker.CoreRt50)]
    [MarkdownExporter]
    [RPlotExporter]
    [AllStatisticsColumn]
    [MinColumn]
    [MaxColumn]
    [MemoryDiagnoser]
    public class SearchBenchmark
    {
        private SearchBenchmarkData _searchBenchmarkData;

        [GlobalSetup]
        public void GlobalSetup() => _searchBenchmarkData = new();

        [Benchmark]
        public void ChineseSearchBenchmark()
        {
            SearchStorage<string> storage = new()
            {
                EnableChinesePinyinSearch = true
            };
            storage.Add(_searchBenchmarkData.ChineseSearchItemData, x => x);
            foreach (string searchText in _searchBenchmarkData.ChineseSearchTextData) _ = storage.Search(searchText);
        }

        [Benchmark]
        public void NormalSearchBenchmark()
        {
            SearchStorage<string> storage = new();
            storage.Add(_searchBenchmarkData.SearchItemData, x => x);
            foreach (string searchText in _searchBenchmarkData.SearchTextData) _ = storage.Search(searchText);
        }
    }

    public static class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<SearchBenchmark>();
        }
    }
}

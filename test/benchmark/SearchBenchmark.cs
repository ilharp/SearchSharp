using System.Threading.Tasks;
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

            Parallel.ForEach(
                _searchBenchmarkData.ChineseSearchTextData,
                searchText => storage.Search(searchText));
        }

        [Benchmark]
        public void NormalSearchBenchmark()
        {
            SearchStorage<string> storage = new();
            storage.Add(_searchBenchmarkData.SearchItemData, x => x);

            Parallel.ForEach(
                _searchBenchmarkData.SearchTextData,
                searchText => storage.Search(searchText));
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

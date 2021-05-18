using System.Collections.Generic;
using System.Linq;
using Xunit;

// ReSharper disable StringLiteralTypo

namespace SearchSharp.Test
{
    public static class SearchTest
    {
        #region SearchStorage

        private static SearchStorage<string> _storage;

        private static SearchStorage<string> Storage
        {
            get
            {
                if (_storage is not null) return _storage;

                _storage = new()
                {
                    Mode = CharParseMode.EnablePinyinSearch
                };

                _storage
                    .Add("AA方式", "SEARCH_REAULT_A")
                    .Add("AA房十", "SEARCH_REAULT_B")
                    .Add("AB模式", "SEARCH_REAULT_C");

                return _storage;
            }
        }

        #endregion

        #region Utils

        private static bool ValidateSearchResult(
            HashSet<string> expected,
            HashSet<string> actual) =>
            actual.All(expected.Contains) &&
            actual.Count == expected.Count;

        #endregion

        #region Tests

        [Theory]
        [InlineData("aafa")]
        [InlineData("aafangs")]
        [InlineData("aafangsh")]
        [InlineData("aafsh")]
        [InlineData("afsh")]
        [InlineData("fsh")]
        public static void ChinesePinyinSearchTest(string searchText) =>
            Assert.True(
                ValidateSearchResult(
                    new() {"SEARCH_REAULT_A", "SEARCH_REAULT_B"},
                    Storage.Search(searchText)));

        [Theory]
        [InlineData("方sh")]
        [InlineData("A方式")]
        public static void ChineseCharSearchTest(string searchText) =>
            Assert.True(
                ValidateSearchResult(
                    new() {"SEARCH_REAULT_A"},
                    Storage.Search(searchText)));

        [Theory]
        [InlineData("bb")]
        [InlineData("aafangg")]
        [InlineData("aafngshi")]
        [InlineData("aafangshii")]
        [InlineData("啊")]
        [InlineData("房十i")]
        [InlineData("a房i")]
        public static void MistakeSearchTest(string searchText) =>
            Assert.True(
                ValidateSearchResult(
                    new(),
                    Storage.Search(searchText)));

        [Theory]
        [InlineData("a房十")]
        [InlineData("fang十")]
        public static void VariousChineseCharSearchTest(string searchText) =>
            Assert.True(
                ValidateSearchResult(
                    new() {"SEARCH_REAULT_B"},
                    Storage.Search(searchText)));

        #endregion
    }
}

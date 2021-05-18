using System;
using System.Text;

namespace SearchSharp.Test.Benchmark
{
    public class SearchBenchmarkData
    {
        public SearchBenchmarkData(int itemCount, int searchTextCount)
        {
            ChineseSearchItemData = GenerateChineseChar(itemCount);
            ChineseSearchTextData = GenerateChineseChar(searchTextCount);
            SearchItemData = GenerateChar(itemCount);
            SearchTextData = GenerateChar(searchTextCount);
        }

        private static string[] GenerateChineseChar(int itemCount)
        {
            string[] rBase = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f"};

            Random random = new();
            byte[] bytes = new byte[itemCount * 2 * 5];
            for (int i = 0; i < itemCount * 5; i++)
            {
                int r1 = random.Next(11, 14);
                string s1 = rBase[r1].Trim();
                random = new();
                var r2 = random.Next(0, r1 == 13 ? 7 : 16);
                string s2 = rBase[r2].Trim();
                random = new();
                int r3 = random.Next(10, 16);
                string s3 = rBase[r3].Trim();
                random = new();
                int r4 = r3 switch
                {
                    10 => random.Next(1, 16),
                    15 => random.Next(0, 15),
                    _ => random.Next(0, 16)
                };
                string s4 = rBase[r4].Trim();
                byte byte1 = Convert.ToByte(s1 + s2, 16);
                byte byte2 = Convert.ToByte(s3 + s4, 16);
                bytes[i * 2] = byte1;
                bytes[i * 2 + 1] = byte2;
            }

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding encoding = Encoding.GetEncoding("gb2312");

            string[] result = new string[itemCount];

            for (int i = 0; i < itemCount; i++)
            {
                string temp = encoding.GetString(new[]
                {
                    bytes[i * 10],
                    bytes[i * 10 + 1],
                    bytes[i * 10 + 2],
                    bytes[i * 10 + 3],
                    bytes[i * 10 + 4],
                    bytes[i * 10 + 5],
                    bytes[i * 10 + 6],
                    bytes[i * 10 + 7],
                    bytes[i * 10 + 8],
                    bytes[i * 10 + 9]
                });
                result[i] += temp;
            }

            return result;
        }

        private static string[] GenerateChar(int itemCount)
        {
            Random r = new();

            // ReSharper disable once StringLiteralTypo
            const string str = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            string[] result = new string[itemCount];

            for (int i = 0; i < itemCount; i++)
            {
                string temp =
                    str.Substring(r.Next(0, str.Length - 1), 1) +
                    str.Substring(r.Next(0, str.Length - 1), 1) +
                    str.Substring(r.Next(0, str.Length - 1), 1) +
                    str.Substring(r.Next(0, str.Length - 1), 1) +
                    str.Substring(r.Next(0, str.Length - 1), 1);
                result[i] += temp;
            }

            return result;
        }

        #region Data

        public readonly string[] ChineseSearchItemData;
        public readonly string[] ChineseSearchTextData;
        public readonly string[] SearchItemData;
        public readonly string[] SearchTextData;

        #endregion
    }
}

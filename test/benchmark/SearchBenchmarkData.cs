using System;
using System.Text;

namespace SearchSharp.Test.Benchmark
{
    public class SearchBenchmarkData
    {
        private static string[] GenerateChineseChar()
        {
            const int length = 100000;

            string[] rBase = {"0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f"};

            Random random = new();
            byte[] bytes = new byte[length];
            for (int i = 0; i < length / 2; i++)
            {
                int r1 = random.Next(11, 14);
                string s1 = rBase[r1].Trim();
                random = new Random(20);
                var r2 = random.Next(0, r1 == 13 ? 7 : 16);
                string s2 = rBase[r2].Trim();
                random = new Random(30);
                int r3 = random.Next(10, 16);
                string s3 = rBase[r3].Trim();
                random = new Random(40);
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

            string[] result = new string[length / 5];
            int index = 0;
            int caret = 0;

            for (int i = 0; i < length; i++)
            {
                string temp = encoding.GetString(new[] {bytes[i]});
                result[index] += temp;
                caret++;
                if (caret != 5) continue;
                caret = 0;
                index++;
            }

            return result;
        }

        private static string[] GenerateChar()
        {
            const int length = 100000;

            Random r = new(50);

            // ReSharper disable once StringLiteralTypo
            const string str = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

            string[] result = new string[length / 5];
            int index = 0;
            int caret = 0;

            for (int i = 0; i < length; i++)
            {
                string temp = str.Substring(r.Next(0, str.Length - 1), 1);
                result[index] += temp;
                caret++;
                if (caret != 5) continue;
                caret = 0;
                index++;
            }

            return result;
        }

        #region Data

        public readonly string[] ChineseSearchItemData = GenerateChineseChar();
        public readonly string[] ChineseSearchTextData = GenerateChineseChar();
        public readonly string[] SearchItemData = GenerateChar();
        public readonly string[] SearchTextData = GenerateChar();

        #endregion
    }
}

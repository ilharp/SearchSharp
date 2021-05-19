using System.Collections.Generic;
using SearchSharp;

namespace SearchSharpIntro
{
    /// <summary>
    /// The data type.
    /// </summary>
    public record MyData
    {
        public string MySearchKey;
        public int MyValue;
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            // Create DataSet
            List<MyData> myDataSet = new()
            {
                new() {MySearchKey = "TheFirstNumber", MyValue = 1},
                new() {MySearchKey = "TheSecondNumber", MyValue = 2}
            };

            // Create SearchStorage
            SearchStorage<MyData> storage = new();

            // Add Items
            // foreach (MyData data in myDataSet) storage.Add(data.MySearchKey, data);
            // Or
            storage.Add(myDataSet, x => x.MySearchKey);

            // Search
            HashSet<MyData> result = storage.Search("num");
        }
    }
}

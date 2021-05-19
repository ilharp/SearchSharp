using System.Collections.Generic;

namespace SearchSharp
{
    public partial class SearchStorage<T>
    {
        private static void GetAllValues(
            Node node,
            HashSet<T> result)
        {
            if (node.IsTerminal)
                foreach (T item in node.Items)
                    result.Add(item);

            foreach (Node edge in node.Edges.Values) GetAllValues(edge, result);
        }

        private static void SearchFromStart(
            Node node,
            string searchText,
            HashSet<T> result)
        {
            foreach (KeyValuePair<string, Node> edge in node.Edges)
            {
                if (!edge.Key.StartsWith(searchText) &&
                    !searchText.StartsWith(edge.Key)) continue;

                if (edge.Value.IsTerminal &&
                    (searchText == edge.Key ||
                       !searchText.StartsWith(edge.Key)))
                    foreach (T item in edge.Value.Items)
                        result.Add(item);

                if (edge.Key.Length < searchText.Length)
                    SearchFromStart(edge.Value, searchText.Substring(edge.Key.Length), result);
                else
                    GetAllValues(edge.Value, result);
            }
        }

        private static void SearchAllIndex(
            Node node,
            string searchText,
            HashSet<T> result)
        {
            if (node.IsTerminal) return;

            foreach (Node value in node.Edges.Values)
            {
                SearchFromStart(value, searchText, result);
                SearchAllIndex(value, searchText, result);
            }
        }

        public HashSet<T> Search(string searchText)
        {
            searchText = searchText.ToLower();

            HashSet<T> result = new();

            SearchFromStart(Root, searchText, result);
            SearchAllIndex(Root, searchText, result);

            return result;
        }
    }
}

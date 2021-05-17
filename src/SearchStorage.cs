using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SearchSharp
{
    public partial class SearchStorage<T>
    {
        #region Trie Node

        public class Node
        {
            #region Items

            private HashSet<T> _items;

            public HashSet<T> Items => _items ??= new();

            #endregion

            public Dictionary<string, Node> Edges = new();

            public bool IsTerminal => Edges.Count == 0 && Items != null;
        }

        #endregion

        private Node Root = new();

        #region Configuration

        public bool EnableChinesePinyinSearch;

        #endregion

        #region Data

        public int Count { get; private set; }

        #endregion

        #region Add

        public SearchStorage<T> Add(IEnumerable<KeyValuePair<string, T>> items)
        {
            foreach (var pair in items) Add(pair.Key, pair.Value);
            return this;
        }

        public SearchStorage<T> Add(IEnumerable<Tuple<string, T>> items)
        {
            foreach (var tuple in items) Add(tuple.Item1, tuple.Item2);
            return this;
        }

        public SearchStorage<T> Add(IEnumerable<(string Key, T Value)> items)
        {
            foreach ((var key, T value) in items) Add(key, value);
            return this;
        }

        public SearchStorage<T> Add(KeyValuePair<string, T> item)
        {
            Add(item.Key, item.Value);
            return this;
        }

        public SearchStorage<T> Add(Tuple<string, T> item)
        {
            Add(item.Item1, item.Item2);
            return this;
        }

        public SearchStorage<T> Add((string Key, T Value) item)
        {
            Add(item.Key, item.Value);
            return this;
        }

        public SearchStorage<T> Add(IEnumerable<T> items, Func<T, string> key)
        {
            foreach (var item in items) Add(key(item), item);
            return this;
        }

        public SearchStorage<T> Add(T item, Func<T, string> key)
        {
            Add(key(item), item);
            return this;
        }

        public unsafe SearchStorage<T> Add(string key, T value)
        {
            Collection<Node> nodes = new() {Root};

            fixed (char* keyPtr = key)
            {
                char* currentPtr = keyPtr;
                char* endPtr = keyPtr + key.Length;

                while (currentPtr < endPtr)
                {
                    Collection<string> keywords = GenerateKeyword(*currentPtr);
                    if (!keywords.Any()) continue;

                    Collection<Node> nextNodes = new();

                    foreach (Node node in nodes)
                    foreach (string keyword in keywords)
                    {
                        if (!node.Edges.ContainsKey(keyword))
                            node.Edges[keyword] = new();

                        nextNodes.Add(node.Edges[keyword]);
                    }

                    nodes = nextNodes;
                    currentPtr++;
                }
            }

            foreach (Node node in nodes) node.Items.Add(value);

            return this;
        }

        #endregion

        #region Other Operations

        public void Clear() => Root = new();

        #endregion

        #region Search

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
                if (edge.Key.StartsWith(searchText) ||
                    searchText.StartsWith(edge.Key))
                {
                    if (edge.Value.IsTerminal)
                        foreach (T item in edge.Value.Items)
                            result.Add(item);

                    if (edge.Key.Length < searchText.Length)
                        SearchFromStart(edge.Value, searchText.Substring(edge.Key.Length), result);
                    else if (edge.Key.Length > searchText.Length)
                        GetAllValues(edge.Value, result);
                }
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

        #endregion
    }
}

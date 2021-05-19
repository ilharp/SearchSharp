using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SearchSharp
{
    public partial class SearchStorage<T>
    {
        #region Trie Node

        private class Node
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

        /// <summary>
        /// <inheritdoc cref="CharParseMode"/>
        /// </summary>
        public CharParseMode Mode = CharParseMode.Default;

        #endregion

        #region Data

        public int Count { get; private set; }

        #endregion

        #region Add

        /// <summary>
        /// Add items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>The <see cref="SearchStorage{T}"/>.</returns>
        public SearchStorage<T> Add(IEnumerable<KeyValuePair<string, T>> items)
        {
            foreach (var pair in items) Add(pair.Key, pair.Value);
            return this;
        }

        /// <summary>
        /// Add items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>The <see cref="SearchStorage{T}"/>.</returns>
        public SearchStorage<T> Add(IEnumerable<Tuple<string, T>> items)
        {
            foreach (var tuple in items) Add(tuple.Item1, tuple.Item2);
            return this;
        }

        /// <summary>
        /// Add items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns>The <see cref="SearchStorage{T}"/>.</returns>
        public SearchStorage<T> Add(IEnumerable<(string Key, T Value)> items)
        {
            foreach ((var key, T value) in items) Add(key, value);
            return this;
        }

        /// <summary>
        /// Add an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The <see cref="SearchStorage{T}"/>.</returns>
        public SearchStorage<T> Add(KeyValuePair<string, T> item)
        {
            Add(item.Key, item.Value);
            return this;
        }

        /// <summary>
        /// Add an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The <see cref="SearchStorage{T}"/>.</returns>
        public SearchStorage<T> Add(Tuple<string, T> item)
        {
            Add(item.Item1, item.Item2);
            return this;
        }

        /// <summary>
        /// Add an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>The <see cref="SearchStorage{T}"/>.</returns>
        public SearchStorage<T> Add((string Key, T Value) item)
        {
            Add(item.Key, item.Value);
            return this;
        }

        /// <summary>
        /// Add items.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <param name="key">The key for search.</param>
        /// <returns>The <see cref="SearchStorage{T}"/>.</returns>
        public SearchStorage<T> Add(IEnumerable<T> items, Func<T, string> key)
        {
            foreach (var item in items) Add(key(item), item);
            return this;
        }

        /// <summary>
        /// Add an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="key">The key for search.</param>
        /// <returns>The <see cref="SearchStorage{T}"/>.</returns>
        public SearchStorage<T> Add(T item, Func<T, string> key)
        {
            Add(key(item), item);
            return this;
        }

        /// <summary>
        /// Add an item.
        /// </summary>
        /// <param name="key">The key for search.</param>
        /// <param name="value">The item.</param>
        /// <returns>The <see cref="SearchStorage{T}"/>.</returns>
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
                    if (!keywords.Any())
                    {
                        currentPtr++;
                        continue;
                    }

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

        /// <summary>
        /// Clear all items.
        /// </summary>
        public void Clear() => Root = new();

        #endregion
    }
}

using System.Collections;
using System.Collections.Immutable;

namespace Indexers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    /// <inheritdoc cref="IMap2D{TKey1,TKey2,TValue}" />
    public class Map2D<TKey1, TKey2, TValue> : IMap2D<TKey1, TKey2, TValue>
    {
        private Dictionary<Tuple<TKey1, TKey2>, TValue> _map = new Dictionary<Tuple<TKey1, TKey2>, TValue>();

        /// <inheritdoc cref="IMap2D{TKey1, TKey2, TValue}.NumberOfElements" />
        public int NumberOfElements
        {
            get => _map.Count;
        }

        /// <inheritdoc cref="IMap2D{TKey1, TKey2, TValue}.this" />
        public TValue this[TKey1 key1, TKey2 key2]
        {
            get => _map[Tuple.Create(key1, key2)];
            set => _map[Tuple.Create(key1, key2)] = value;
        }

        /// <inheritdoc cref="IMap2D{TKey1, TKey2, TValue}.GetRow(TKey1)" />
        public IList<Tuple<TKey2, TValue>> GetRow(TKey1 key1)
        {
            IList<Tuple<TKey2, TValue>> res = new List<Tuple<TKey2, TValue>>();
            foreach (var row in _map)
            {
                if (row.Key.Item1.Equals(key1))
                {
                    res.Add(Tuple.Create(row.Key.Item2, row.Value));
                }
            }
            return res;
        }

        /// <inheritdoc cref="IMap2D{TKey1, TKey2, TValue}.GetColumn(TKey2)" />
        public IList<Tuple<TKey1, TValue>> GetColumn(TKey2 key2)
        {
            IList<Tuple<TKey1, TValue>> res = new List<Tuple<TKey1, TValue>>();
            foreach (var row in _map)
            {
                if (row.Key.Item2.Equals(key2))
                {
                    res.Add(Tuple.Create(row.Key.Item1, row.Value));
                }
            }
            return res;
        }

        /// <inheritdoc cref="IMap2D{TKey1, TKey2, TValue}.GetElements" />
        public IList<Tuple<TKey1, TKey2, TValue>> GetElements()
        {
            IList<Tuple<TKey1, TKey2, TValue>> res = new List<Tuple<TKey1, TKey2, TValue>>();
            foreach (var row in _map)
            {
                    res.Add(new Tuple<TKey1, TKey2, TValue>(row.Key.Item1, row.Key.Item2, row.Value));
            }
            return res;
        }

        /// <inheritdoc cref="IMap2D{TKey1, TKey2, TValue}.Fill(IEnumerable{TKey1}, IEnumerable{TKey2}, Func{TKey1, TKey2, TValue})" />
        public void Fill(IEnumerable<TKey1> keys1, IEnumerable<TKey2> keys2, Func<TKey1, TKey2, TValue> generator)
        {
            foreach (var row in keys1)
            {
                foreach (var column in keys2)
                {
                    this[row, column] = generator(row, column);
                }
            }
        }

        /// <inheritdoc cref="IEquatable{T}.Equals(T)" />
       
        public bool Equals(IMap2D<TKey1, TKey2, TValue>  other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(GetElements(), other.GetElements());
        }

        /// <inheritdoc cref="object.GetHashCode"/>
        public override int GetHashCode()
        {
            return (_map != null ? _map.GetHashCode() : 0);
        }

        protected bool Equals(Map2D<TKey1, TKey2, TValue> other)
        {
            return Equals(_map, other._map);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Map2D<TKey1, TKey2, TValue>) obj);
        }

        /// <inheritdoc cref="IMap2D{TKey1, TKey2, TValue}.ToString"/>
        public override string ToString()
        {
            string res = "{\n";
            foreach (var row in _map)
            {
                res = $"{res}[{row.Key.Item1}, {row.Key.Item2}]->[{row.Value}],\n";
            }
            res = res.Substring(0, res.Length - 1);
            res = $"{res}}}";
            return res;
        }
    }
}

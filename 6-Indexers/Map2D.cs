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
        private const int InitialSize = 50;//initial arrays sizes
        private int _size = InitialSize;
        private Tuple<TKey1, TKey2>[] _keys = new Tuple<TKey1, TKey2>[InitialSize];
        private TValue[] _values = new TValue[InitialSize];

        /// <inheritdoc cref="IMap2D{TKey1, TKey2, TValue}.NumberOfElements" />
        public int NumberOfElements
        {
            get;
            private set;
        } = 0;

        /// <inheritdoc cref="IMap2D{TKey1, TKey2, TValue}.this" />
        public TValue this[TKey1 key1, TKey2 key2]
        {
            get  {
                for(int i=0;i<NumberOfElements;i++)
                {
                    if (_keys[i].Item1.Equals(key1) && _keys[i].Item2.Equals(key2))
                    {
                        return _values[i];
                    }
                }

                throw new KeyNotFoundException();
            }
            set
            {
                bool found = false;
                for(int i=0;i<NumberOfElements;i++)
                {
                    if (_keys[i].Item1.Equals(key1) && _keys[i].Item2.Equals(key2))
                    {
                        _values[i] = value;
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    AddNewElement(key1, key2, value);
                }
            }
        }

        private void ReallocArrays()
        {
            TValue[] values = _values;
            Tuple<TKey1, TKey2>[] keys = _keys;
            _size *= 3;
            _values = new TValue[_size];
            _keys = new Tuple<TKey1, TKey2>[_size];
            for (int i = 0; i < keys.Length; i++)
            {
                _keys[i] = keys[i];
                _values[i] = values[i];
            }
        }

        private void AddNewElement(TKey1 key1, TKey2 key2, TValue value)
        {
            if (NumberOfElements >= _size)
            {
                ReallocArrays();
            }

            _keys[NumberOfElements] = new Tuple<TKey1, TKey2>(key1, key2);
            _values[NumberOfElements] = value;
            NumberOfElements++;
        }

        /// <inheritdoc cref="IMap2D{TKey1, TKey2, TValue}.GetRow(TKey1)" />
        public IList<Tuple<TKey2, TValue>> GetRow(TKey1 key1)
        {
            IList<Tuple<TKey2, TValue>> res = new List<Tuple<TKey2, TValue>>();
            for (int i = 0; i < NumberOfElements; i++)
            {
                if (_keys[i].Item1.Equals(key1))
                {
                    res.Add(new Tuple<TKey2, TValue>(_keys[i].Item2, _values[i]));
                }
            }

            return res;
        }

        /// <inheritdoc cref="IMap2D{TKey1, TKey2, TValue}.GetColumn(TKey2)" />
        public IList<Tuple<TKey1, TValue>> GetColumn(TKey2 key2)
        {
            IList<Tuple<TKey1, TValue>> res = new List<Tuple<TKey1, TValue>>();
            for (int i = 0; i < NumberOfElements; i++)
            {
                if (_keys[i].Item2.Equals(key2))
                {
                    res.Add(new Tuple<TKey1, TValue>(_keys[i].Item1, _values[i]));
                }
            }

            return res;
        }

        /// <inheritdoc cref="IMap2D{TKey1, TKey2, TValue}.GetElements" />
        public IList<Tuple<TKey1, TKey2, TValue>> GetElements()
        {
            IList<Tuple<TKey1, TKey2, TValue>> res = new List<Tuple<TKey1, TKey2, TValue>>();
            for (int i = 0; i < NumberOfElements; i++)
            {
                res.Add(new Tuple<TKey1, TKey2, TValue>(_keys[i].Item1, _keys[i].Item2, _values[i]));
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
            return HashCode.Combine(_keys, _values);
        }

        /// <inheritdoc cref="IMap2D{TKey1, TKey2, TValue}.ToString"/>
        public override string ToString()
        {
            string res = "{\n";
            for (int i = 0; i < NumberOfElements; i++)
            {
                res = $"{res}[{_keys[i].Item1}, {_keys[i].Item2}]->[{_values[i]}],\n";
            }

            res = res.Substring(0, res.Length - 1);
            res = $"{res}}}";
            return res;
        }
    }
}

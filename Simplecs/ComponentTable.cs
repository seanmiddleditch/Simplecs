// Simplecs - Simple ECS for C#
//
// Written in 2019 by Sean Middleditch (http://github.com/seanmiddleditch)
//
// To the extent possible under law, the author(s) have dedicated all copyright
// and related and neighboring rights to this software to the public domain
// worldwide. This software is distributed without any warranty.
//
// You should have received a copy of the CC0 Public Domain Dedication along
// with this software. If not, see
// <http://creativecommons.org/publicdomain/zero/1.0/>.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Simplecs {
    /// <summary>
    /// Safe readonly view of a component table that will never cause an allocation.
    /// </summary>
    internal interface IComponentTable {
        /// <value>Type of the component data stored in this table.</value>
        Type Type { get; }

        /// <param name="key">Entity key.</param>
        /// <returns>True if a component is stored for this key.</returns>
        bool Has(uint key);

        /// <param name="key">Entity key.</param>
        /// <returns>True if a component was stored for this key and is now removed.</returns>
        bool Remove(uint key);
    }

    /// <summary>
    /// Stores a table of components mapped by unique entity keys.
    /// </summary>
    /// <typeparam name="T">Struct type containing component data.</typeparam>
    internal class ComponentTable<T> : IComponentTable, IEnumerable<(uint, T)> where T : struct {
        private List<T> _data = new List<T>();
        private List<uint> _dense = new List<uint>();
        private List<int> _sparse = new List<int>();

        public Type Type => typeof(T);

        public bool Has(uint key) {
            return key < _sparse.Count && 
                _sparse[(int)key] < _dense.Count &&
                _dense[_sparse[(int)key]] == key;
        }

        public bool Remove(uint key) {
            if (key >= _sparse.Count ||
                _sparse[(int)key] >= _dense.Count ||
                _dense[_sparse[(int)key]] != key) {
                return false;
            }

            int denseIndex = _sparse[(int)key];
            uint newSparse = _dense[_dense.Count - 1];

            _sparse[(int)newSparse] = _sparse[(int)key];
            _sparse[(int)key] = int.MaxValue;

            _dense[denseIndex] = newSparse;
            _data[denseIndex] = _data[_data.Count - 1];

            _dense.RemoveAt(_dense.Count - 1);
            _data.RemoveAt(_data.Count - 1);
            return true;
        }

        /// <summary>
        /// Adds a component to the table.
        /// </summary>
        /// <param name="key">Entity key.</param>
        /// <param name="data">Component data to add.</param>
        public void Set(uint key, T data) {
            if (key < _sparse.Count && 
                _sparse[(int)key] < _dense.Count &&
                _dense[_sparse[(int)key]] == key) {
                _data[_sparse[(int)key]] = data;
                return;
            }

            if (key >= _sparse.Count) {
                _sparse.AddRange(Enumerable.Repeat(int.MaxValue, (int)key - _sparse.Count + 1));
            }

            _sparse[(int)key] = _dense.Count;

            _dense.Add(key);
            _data.Add(data);
        }

        /// <summary>
        /// Attempts to retrieve component data stored for an entity key.
        /// </summary>
        /// <param name="key">Entity key.</param>
        /// <param name="data">Component data associated with key.</param>
        /// <returns>True if an entry existed for the supplied key.</returns>
        public bool TryGet(uint key, out T data) {
            if (key >= _sparse.Count ||
                _sparse[(int)key] >= _dense.Count ||
                _dense[_sparse[(int)key]] != key) {
                data = default(T);
                return false;
            }

            data = _data[_sparse[(int)key]];
            return true;
        }

        /// <summary>
        /// Enumerates all entity keys and associated component data stored in the table.
        /// </summary>
        /// <returns>Enumerator of (key, component) tuples.</returns>
        public IEnumerator<(uint, T)> GetEnumerator() {
            for (int index = 0; index != _data.Count; ++index) {
                yield return (_dense[index], _data[index]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }
    }
}
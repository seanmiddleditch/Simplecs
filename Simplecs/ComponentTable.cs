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
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Simplecs {
    /// <summary>
    /// Safe readonly view of a component table that will never cause an allocation.
    /// </summary>
    internal interface IComponentTable {
        /// <value>Type of the component data stored in this table.</value>
        Type Type { get; }

        /// <value>Number of components stored in table.</value>
        int Count { get; }

        /// <param name="entity">Entity key.</param>
        /// <returns>True if a component is stored for this key.</returns>
        bool Contains(Entity entity);

        /// <param name="entity">Entity key.</param>
        /// <returns>True if a component was stored for this key and is now removed.</returns>
        bool Remove(Entity entity);

        /// <summary>
        /// Removes all components.
        /// </summary>
        void Clear();
    }

    /// <summary>
    /// Stores a table of components mapped by unique entity keys.
    /// </summary>
    /// <typeparam name="T">Struct type containing component data.</typeparam>
    internal class ComponentTable<T> : IComponentTable, IEnumerable<(Entity, T)> where T : struct {
        private ChunkedStorage<T> _data = new ChunkedStorage<T>();
        private List<Entity> _dense = new List<Entity>();
        private List<int> _sparse = new List<int>();

        public delegate void Callback(Entity entity, ref T component);

        public Type Type => typeof(T);

        public int Count => _dense.Count;

        public bool Contains(Entity entity) {
            int index = EntityUtil.DecomposeIndex(entity);

            return index >= 0 &&
                index < _sparse.Count &&
                _sparse[index] < _dense.Count &&
                _dense[_sparse[index]].key == entity.key;
        }

        public bool Remove(Entity entity) {
            if (!Contains(entity)) {
                return false;
            }

            int index = EntityUtil.DecomposeIndex(entity);

            int denseIndex = _sparse[index];
            int newSparse = EntityUtil.DecomposeIndex(_dense[_dense.Count - 1]);

            _sparse[newSparse] = _sparse[index];
            _sparse[index] = int.MaxValue;

            _dense[denseIndex] = _dense[_dense.Count - 1];
            _data[denseIndex] = _data[_data.Count - 1];

            _dense.RemoveAt(_dense.Count - 1);
            _data.RemoveAt(_data.Count - 1);
            return true;
        }

        /// <summary>
        /// Adds a component to the table.
        /// </summary>
        /// <param name="entity">Entity key.</param>
        /// <param name="data">Component data to add.</param>
        public void Add(Entity entity, in T data) {
            if (Contains(entity)) {
                _data[_sparse[EntityUtil.DecomposeIndex(entity)]] = data;
                return;
            }

            int index = EntityUtil.DecomposeIndex(entity);
            if (index >= _sparse.Count) {
                _sparse.AddRange(Enumerable.Repeat(int.MaxValue, index - _sparse.Count + 1));
            }

            _sparse[index] = _dense.Count;

            _dense.Add(entity);
            _data.Add(data);
        }

        /// <summary>
        /// Attempts to retrieve component data stored for an entity key.
        /// </summary>
        /// <param name="entity">Entity key.</param>
        /// <param name="data">Component data associated with key.</param>
        /// <returns>True if an entry existed for the supplied key.</returns>
        public bool TryGet(Entity entity, out T data) {
            int index = EntityUtil.DecomposeIndex(entity);

            if (index < 0 ||
                index >= _sparse.Count ||
                _sparse[index] >= _dense.Count ||
                _dense[_sparse[index]].key != entity.key) {
                data = default(T);
                return false;
            }

            data = _data[_sparse[index]];
            return true;
        }

        /// <summary>
        /// Enumerates all entity keys and associated component data stored in the table.
        /// </summary>
        /// <returns>Enumerator of (key, component) tuples.</returns>
        public IEnumerator<(Entity, T)> GetEnumerator() {
            for (int index = 0; index != _data.Count; ++index) {
                yield return (_dense[index], _data[index]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Iterates all stored components.
        /// </summary>
        /// <param name="callback">Invoked for each entity/component pair in table.</param>
        public void Each(Callback callback) {
            for (int index = 0; index != _data.Count; ++index) {
                callback(_dense[index], ref _data[index]);
            }
        }

        public void Clear() {
            _data.Clear();
            _dense.Clear();
        }
    }
}
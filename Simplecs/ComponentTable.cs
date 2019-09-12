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
using System.Collections.Generic;
using System.Linq;

namespace Simplecs {
    internal interface IComponentTable {
        Type Type { get; }
        int Count { get; }

        bool Contains(Entity entity);
        bool Remove(Entity entity);
        void Add(Entity entity, object component);
        bool TryGet(Entity entity, out object data);
        void Clear();
    }

    /// <summary>
    /// Stores a table of components mapped by unique entity keys.
    /// </summary>
    /// <typeparam name="T">Struct type containing component data.</typeparam>
    internal class ComponentTable<T> : IComponentTable where T : struct {
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

        void IComponentTable.Add(Entity entity, object component) {
            if (component.GetType() != typeof(T)) {
                throw new InvalidOperationException(message:"Incorrect component type");
            }

            Add(entity, (T)component);
        }

        bool IComponentTable.TryGet(Entity entity, out object data) {
            if (!TryGet(entity, out T component)) {
                data = false;
                return false;
            }

            data = component;
            return true;
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

        public ref T this[Entity entity] => ref _data[_sparse[EntityUtil.DecomposeIndex(entity)]];
        public Entity this[int index] => _dense[index];

        public void Clear() {
            _data.Clear();
            _dense.Clear();
        }
    }
}
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
        int Version { get; }

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
        private List<Entity> _entities = new List<Entity>();
        private List<int> _mapping = new List<int>();
        private int _version = 0;

        public delegate void Callback(Entity entity, ref T component);

        public Type Type => typeof(T);
        public int Count => _entities.Count;
        public int Version => _version;

        public bool Contains(Entity entity) {
            int mappingIndex = EntityUtil.DecomposeIndex(entity);

            return mappingIndex >= 0 &&
                mappingIndex < _mapping.Count &&
                _mapping[mappingIndex] < _entities.Count &&
                _entities[_mapping[mappingIndex]].key == entity.key;
        }

        public bool Remove(Entity entity) {
            if (!Contains(entity)) {
                return false;
            }

            int mappingIndex = EntityUtil.DecomposeIndex(entity);
            int dataIndex = _mapping[mappingIndex];

            // Mark the mapping as invalid so the entity can no longer be
            // queried directly.
            //
            _mapping[mappingIndex] = int.MaxValue;

            // Update mapping for the last component which is going to be moved into
            // the removed location.
            //
            int newMappingIndex = EntityUtil.DecomposeIndex(_entities[_entities.Count - 1]);
            _mapping[newMappingIndex] = dataIndex;

            // Move the component and entity information into the removed index.
            //
            _entities[dataIndex] = _entities[_entities.Count - 1];
            _data[dataIndex] = _data[_data.Count - 1];

            // Pop the final component.
            //
            _entities.RemoveAt(_entities.Count - 1);
            _data.RemoveAt(_data.Count - 1);

            // Reordering components can invalid any executing enumerators.
            //
            ++_version;

            return true;
        }

        /// <summary>
        /// Adds a component to the table.
        /// </summary>
        /// <param name="entity">Entity key.</param>
        /// <param name="data">Component data to add.</param>
        public void Add(Entity entity, in T data) {
            if (Contains(entity)) {
                _data[_mapping[EntityUtil.DecomposeIndex(entity)]] = data;
                return;
            }

            int index = EntityUtil.DecomposeIndex(entity);
            if (index >= _mapping.Count) {
                _mapping.AddRange(Enumerable.Repeat(int.MaxValue, index - _mapping.Count + 1));
            }

            _mapping[index] = _entities.Count;

            _entities.Add(entity);
            _data.Add(data);

            // While this add operation is safe in the current data structure, we don't want
            // to support this as a guarantee, so invalid enumerators.
            //
            ++_version;
        }

        void IComponentTable.Add(Entity entity, object component) {
            if (component.GetType() != typeof(T)) {
                throw new InvalidOperationException(message: "Incorrect component type");
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
            if (!Contains(entity)) {
                data = default(T);
                return false;
            }

            data = _data[_mapping[EntityUtil.DecomposeIndex(entity)]];
            return true;
        }

        public ref T this[Entity entity] => ref _data[_mapping[EntityUtil.DecomposeIndex(entity)]];
        public Entity this[int index] => _entities[index];

        public void Clear() {
            _data.Clear();
            _entities.Clear();
            ++_version;
        }
    }
}
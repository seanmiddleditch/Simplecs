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

namespace Simplecs.Containers {
    internal interface IComponentTable {
        Type Type { get; }
        int Count { get; }

        bool Contains(Entity entity);
        bool Remove(Entity entity);
        void Add(Entity entity, object component);
        bool TryGet(Entity entity, out object data);
        int IndexOf(Entity entity);
        Entity EntityAt(int index);
        void Clear();
    }

    /// <summary>
    /// Stores a table of components mapped by unique entity keys.
    /// </summary>
    /// <typeparam name="T">Struct type containing component data.</typeparam>
    internal class ComponentTable<T> : IComponentTable where T : struct {
        private readonly ChunkedStorage<T> _data = new ChunkedStorage<T>();
        private readonly List<Entity> _entities = new List<Entity>();
        private readonly List<int> _mapping = new List<int>();

        public delegate void Callback(Entity entity, ref T component);

        public Type Type => typeof(T);
        public int Count => _entities.Count;

        public bool Contains(Entity entity) => IndexOf(entity) != -1;
        
        public int IndexOf(Entity entity) {
            int mappingIndex = EntityUtil.DecomposeIndex(entity);

            if (mappingIndex < 0 || mappingIndex >= _mapping.Count) {
                return -1;
            }

            int dataIndex = _mapping[mappingIndex];
            if (dataIndex >= _entities.Count) {
                return -1;
            }

            return _entities[dataIndex] == entity ? dataIndex : -1;
        }

        public bool Remove(Entity entity) {
            int dataIndex = IndexOf(entity);
            if (dataIndex == -1) {
                return false;
            }

            // Mark the mapping as invalid so the entity can no longer be
            // queried directly.
            //
            _mapping[EntityUtil.DecomposeIndex(entity)] = int.MaxValue;

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

            return true;
        }

        public void Add(Entity entity, in T data) {
            int dataIndex = IndexOf(entity);
            if (dataIndex != -1) {
                _data[dataIndex] = data;
                return;
            }

            int mappingIndex = EntityUtil.DecomposeIndex(entity);
            if (mappingIndex >= _mapping.Count) {
                _mapping.AddRange(Enumerable.Repeat(int.MaxValue, mappingIndex - _mapping.Count + 1));
            }

            _mapping[mappingIndex] = _entities.Count;

            _entities.Add(entity);
            _data.Add(data);
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

        public bool TryGet(Entity entity, out T data) {
            int dataIndex = IndexOf(entity);
            bool isValid = dataIndex != -1;

            data = isValid ? _data[dataIndex] : default(T);
            return isValid;
        }

        public Entity EntityAt(int index) => _entities[index];

        public ref T ReferenceAt(Entity entity, int index) {
            if (_entities[index] != entity) {
                throw new InvalidOperationException(message:"Dereference on invalidated binding.");
            }

            return ref _data[index];
        }

        public void Clear() {
            _data.Clear();
            _entities.Clear();
        }
    }
}
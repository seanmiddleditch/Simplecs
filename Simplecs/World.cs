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

namespace Simplecs {
    public class World {
        private EntityAllocator _entityAllocator = new EntityAllocator();
        private Dictionary<Type, IComponentTable> _components = new Dictionary<Type, IComponentTable>();

        public EntityBuilder Create() {
            uint key = _entityAllocator.Allocate();
            return new EntityBuilder(world:this, entity:new Entity{key = key});
        }

        public bool Destroy(Entity entity) {
            bool found = false;
            foreach (var table in _components.Values) {
                found = table.Remove(entity.key) && found;
            }
            return found;
        }

        public void Attach<T>(Entity entity, T component) where T : struct {
            _components.TryGetValue(typeof(T), out IComponentTable? table);
            var components = table as ComponentTable<T>;
            if (components == null) {
                components = new ComponentTable<T>();
                _components.Add(components.Type, components);
            }
            components.Add(entity.key, component);
        }

        public bool Detach<T>(Entity entity) where T : struct {
            _components.TryGetValue(typeof(T), out IComponentTable? table);
            var components = table as ComponentTable<T>;
            return components != null && components.Remove(entity.key);
        }

        public T? Fetch<T>(Entity entity) where T : struct {
            var components = Components<T>();
            if (components.TryGet(entity.key, out T data)) {
                return data;
            }
            return null;
        }

        internal ComponentTable<T> Components<T>() where T : struct {
            if (_components.TryGetValue(typeof(T), out IComponentTable? generic) && generic is ComponentTable<T> typed) {
                return typed;
            }

            var table = new ComponentTable<T>();
            _components.Add(table.Type, table);
            return table;
        }
    }
}
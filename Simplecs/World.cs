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
    /// <summary>
    /// Contains a collection of entities and associated components.
    /// </summary>
    public class World {
        private EntityAllocator _entityAllocator = new EntityAllocator();
        private Dictionary<Type, IComponentTable> _components = new Dictionary<Type, IComponentTable>();

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <returns>A builder object that can be used to attach components or extract the id.</returns>
        public EntityBuilder Create() => new EntityBuilder(world: this, entity: _entityAllocator.Allocate());

        /// <summary>
        /// Destroys a given entity and all associated components.
        /// </summary>
        /// <param name="entity">Entity to destroy</param>
        /// <returns>True if the given entity was found and destroyed.</returns>
        public bool Destroy(Entity entity) {
            if (!_entityAllocator.Deallocate(entity)) {
                return false;
            }

            foreach (var table in _components.Values) {
                table.Remove(entity);
            }
            return true;
        }

        /// <summary>
        /// Attaches a component to an existing entity.
        /// </summary>
        /// <param name="entity">Entity the component will be attached to.</param>
        /// <param name="component">Component to attach.</param>
        public void Attach<T>(Entity entity, in T component) where T : struct {
            if (!_entityAllocator.Validate(entity)) {
                throw new InvalidOperationException(message: "Invalid entity key");
            }

            var table = GetTable<T>();
            table.Add(entity, component);
        }

        /// <summary>
        /// Removes a component from an existing entity.
        /// </summary>
        /// <param name="entity">Entity whose component should be destroyed.</param>
        /// <returns>True if the given component was found and destroyed.</returns>
        public bool Detach<T>(Entity entity) where T : struct {
            _components.TryGetValue(typeof(T), out IComponentTable? table);
            var components = table as ComponentTable<T>;
            return components != null && components.Remove(entity);
        }

        internal ComponentTable<T> GetTable<T>() where T : struct {
            if (_components.TryGetValue(typeof(T), out IComponentTable? generic) && generic is ComponentTable<T> typed) {
                return typed;
            }

            var table = new ComponentTable<T>();
            _components.Add(table.Type, table);
            return table;
        }
    }
}
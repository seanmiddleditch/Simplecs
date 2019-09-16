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
using Simplecs.Containers;
using Simplecs.Views;

namespace Simplecs {
    /// <summary>
    /// Contains a collection of entities and associated components.
    /// </summary>
    public sealed class World {
        private readonly EntityAllocator _entities = new EntityAllocator();
        private readonly Dictionary<Type, IComponentTable> _tables = new Dictionary<Type, IComponentTable>();

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <returns>A builder object that can be used to attach components or extract the id.</returns>
        public EntityBuilder CreateEntity() => new EntityBuilder(world: this, entity: _entities.Allocate());

        /// <summary>
        /// Creates a new view builder.
        /// </summary>
        /// <returns>A builder object used to construct a view.</returns>
        public ViewBuilder CreateView() => new ViewBuilder(this);

        /// <summary>
        /// Destroys a given entity and all associated components.
        /// </summary>
        /// <param name="entity">Entity to destroy</param>
        /// <returns>True if the given entity was found and destroyed.</returns>
        public bool Destroy(Entity entity) {
            if (!_entities.IsValid(entity)) {
                return false;
            }

            foreach (var table in _tables.Values) {
                table.Remove(entity);
            }

            return _entities.Deallocate(entity);
        }

        /// <summary>
        /// Checks if a given entity exists.
        /// 
        /// The entity may have no components, however.
        /// </summary>
        /// <param name="entity">Entity to test.</param>
        /// <returns>True if the entity exists.</returns>
        public bool Exists(Entity entity) => _entities.IsValid(entity);

        /// <summary>
        /// Attaches a component to an existing entity.
        /// </summary>
        /// <param name="entity">Entity the component will be attached to.</param>
        /// <param name="component">Component to attach.</param>
        public void Attach<T>(Entity entity, in T component) where T : struct {
            if (!_entities.IsValid(entity)) {
                throw new InvalidOperationException(message: "Invalid entity key");
            }

            var table = GetTable<T>();
            table.Add(entity, component);
        }

        /// <summary>
        /// Attaches a component to an existing entity.
        /// </summary>
        /// <param name="entity">Entity the component will be attached to.</param>
        /// <param name="component">Component to attach.</param>
        public void Attach(Entity entity, object component) {
            if (!_entities.IsValid(entity)) {
                throw new InvalidOperationException(message: "Invalid entity key");
            }

            var table = GetTable(component.GetType());
            table.Add(entity, component);
        }

        /// <summary>
        /// Removes a component from an existing entity.
        /// </summary>
        /// <param name="entity">Entity whose component should be destroyed.</param>
        /// <returns>True if the given component was found and destroyed.</returns>
        public bool Detach<T>(Entity entity) where T : struct {
            _tables.TryGetValue(typeof(T), out IComponentTable? table);
            var components = table as ComponentTable<T>;
            return components != null && components.Remove(entity);
        }

        /// <summary>
        /// Removes a component from an existing entity.
        /// </summary>
        /// <param name="entity">Entity whose component should be destroyed.</param>
        /// /// <param name="componentType">Type of component that should be destroyed.</param>
        /// <returns>True if the given component was found and destroyed.</returns>
        public bool Detach(Entity entity, Type componentType) {
            return _tables.TryGetValue(componentType, out IComponentTable? table) && table != null && table.Remove(entity);
        }

        /// <summary>
        /// Checks if the specified entity has the specified component type attached.
        /// </summary>
        public bool HasComponent<T>(Entity entity) where T : struct {
            return HasComponent(entity, typeof(T));
        }

        /// <summary>
        /// Checks if the specified entity has the specified component type attached.
        /// </summary>
        public bool HasComponent(Entity entity, Type component) {
            return _tables.TryGetValue(component, out IComponentTable? table) && table != null && table.Contains(entity);
        }

        /// <summary>
        /// Retrives the component on the specified entity.null
        /// 
        /// Illegal to call if the component does not exist.
        /// </summary>
        public ref T GetComponent<T>(Entity entity) where T : struct {
            if (!_tables.TryGetValue(typeof(T), out IComponentTable? generic) || !(generic is ComponentTable<T> typed)) {
                throw new InvalidOperationException(message: $"Unknown component type: {typeof(T).FullName}");
            }

            int index = typed.IndexOf(entity);
            if (index == -1) {
                throw new InvalidOperationException(message: "Entity does not have requested component");
            }

            return ref typed.ReferenceAt(entity, index);
        }

        /// <summary>
        /// Retrives the component on the specified entity.null
        /// 
        /// Illegal to call if the component does not exist.
        /// </summary>
        public object GetComponent(Entity entity, Type component) {
            if (!_tables.TryGetValue(component, out IComponentTable? table) || table == null || !table.TryGet(entity, out object data)) {
                throw new InvalidOperationException(message: "Entity does not have requested component");
            }

            return data;
        }

        /// <summary>
        /// Enumerates all the components attached to a given entity.
        /// 
        /// Note that these are returned as objects, so boxing for each component will
        /// be involved! Further, modifications to these instances will not apply
        /// automatically to the stored components; if modified, components should be
        /// re-Attached.
        /// </summary>
        /// <param name="entity">Entity whose components should be enumerated.</param>
        /// <returns>Iterator of boxed copies of the component on the entity.</returns>
        public IEnumerable<Type> ComponentsOn(Entity entity) {
            if (!_entities.IsValid(entity)) {
                throw new InvalidOperationException(message: "Invalid entity key");
            }

            foreach ((var componentType, IComponentTable table) in _tables) {
                if (table.Contains(entity)) {
                    yield return componentType;
                }
            }
        }

        internal ComponentTable<T> GetTable<T>() where T : struct {
            if (_tables.TryGetValue(typeof(T), out IComponentTable? generic) && generic is ComponentTable<T> typed) {
                return typed;
            }

            var table = new ComponentTable<T>();
            _tables.Add(table.Type, table);
            return table;
        }

        internal IComponentTable GetTable(Type type) {
            if (_tables.TryGetValue(type, out IComponentTable? generic) && generic != null) {
                return generic;
            }

            var table = CreateTable(type);
            _tables.Add(table.Type, table);
            return table;
        }

        private IComponentTable CreateTable(Type type) {
            if (!type.IsValueType) {
                throw new InvalidOperationException(message: "Component types must be value types");
            }

            var tableGenericType = typeof(ComponentTable<>);
            var tableType = tableGenericType.MakeGenericType(type);

            var tableObject = Activator.CreateInstance(tableType);
            var table = tableType as IComponentTable;
            if (table == null) {
                throw new InvalidCastException(message: "Failed to cast created table to IComponentTable");
            }

            return (IComponentTable)table;
        }
    }
}
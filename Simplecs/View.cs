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

using System.Collections;
using System.Collections.Generic;

namespace Simplecs {
    /// <summary>
    /// Base class for views.
    /// 
    /// Supplies functionality for views of all arities.
    /// </summary>
    public abstract class ViewBase {
        private World _world;
        private List<IComponentTable> _required = new List<IComponentTable>();
        private List<IComponentTable> _excluded = new List<IComponentTable>();

        /// <param name="world">World for this View.</param>
        protected ViewBase(World world) => _world = world;

        /// <summary>
        /// World which this view inspects.
        /// </summary>
        public World World => _world;

        internal void Exclude<T>() where T : struct => _excluded.Add(_world.GetTable<T>());
        internal void Require<T>() where T : struct => _required.Add(_world.GetTable<T>());

        internal bool IsAllowed(Entity entity) => !IsExcluded(entity) && HasRequired(entity);

        private bool IsExcluded(Entity entity) {
            foreach (IComponentTable table in _excluded) {
                if (table.Contains(entity)) {
                    return true;
                }
            }
            return false;
        }

        private bool HasRequired(Entity entity) {
            foreach (IComponentTable table in _required) {
                if (!table.Contains(entity)) {
                    return false;
                }
            }
            return true;
        }
    }

    /// <summary>
    /// Iterates over all entities with a particular component type.
    /// </summary>
    /// <typeparam name="T">Type of required component.</typeparam>
    public sealed class View<T> : ViewBase, IEnumerable<(Entity, T)> where T : struct {
        private ComponentTable<T> _table;

        /// <summary>
        /// Constructs a new view over a single component type.
        /// </summary>
        /// <param name="world">World this view inspects.</param>
        public View(World world) : base(world) => _table = world.GetTable<T>();

        /// <summary>
        /// Callback for the Each method.
        /// </summary>
        /// <param name="entity">Matched entity.</param>
        /// <param name="component">Component reference of matched entity.</param>
        public delegate void Callback(Entity entity, ref T component);

        /// <summary>
        /// Excludes the specified component from the matched set.
        /// </summary>
        new public View<T> Exclude<U>() where U : struct { base.Exclude<U>(); return this; }

        /// <summary>
        /// Marks the specified as required for the matched set.
        /// </summary>
        new public View<T> Require<U>() where U : struct { base.Require<U>(); return this; }

        /// <summary>
        /// Enumerator for matched entities and components.
        /// 
        /// Note that components are returned _by value_ and hence
        /// should not be modified.
        /// </summary>
        /// <returns>Entity and component enumerator.</returns>
        public IEnumerator<(Entity, T)> GetEnumerator() {
            foreach ((Entity entity, T component) in _table) {
                if (IsAllowed(entity)) {
                    yield return (entity, component);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <summary>
        /// Invokes the given callback for each matched entity and components.
        /// 
        /// Note that the components are reference types, and hence can be safely
        /// modified in this callback.
        /// </summary>
        /// <param name="callback">Callback to invoke for each match.</param>
        public void Each(Callback callback) {
            _table.Each((Entity entity, ref T component) => {
                if (IsAllowed(entity)) {
                    callback(entity, ref component);
                }
            });
        }
    }

    /// <summary>
    /// Iterates over all entities with a particular component types.
    /// </summary>
    /// <typeparam name="T1">Type of required component.</typeparam>
    /// <typeparam name="T2">Type of required component.</typeparam>
    public sealed class View<T1, T2> : ViewBase, IEnumerable<(Entity, T1, T2)> where T1 : struct where T2 : struct {
        private ComponentTable<T1> _table1;
        private ComponentTable<T2> _table2;

        /// <summary>
        /// Callback for the Each method.
        /// </summary>
        /// <param name="entity">Matched entity.</param>
        /// <param name="component1">First component reference of matched entity.</param>
        /// <param name="component2">Second component reference of matched entity.</param>
        public delegate void Callback(Entity entity, ref T1 component1, ref T2 component2);

        /// <summary>
        /// Excludes the specified component from the matched set.
        /// </summary>
        new public View<T1, T2> Exclude<U>() where U : struct { base.Exclude<U>(); return this; }

        /// <summary>
        /// Marks the specified as required for the matched set.
        /// </summary>
        new public View<T1, T2> Require<U>() where U : struct { base.Require<U>(); return this; }

        /// <summary>
        /// Constructs a new view over a single component type.
        /// </summary>
        /// <param name="world">World this view inspects.</param>
        public View(World world) : base(world) {
            _table1 = world.GetTable<T1>();
            _table2 = world.GetTable<T2>();
        }

        /// <summary>
        /// Enumerator for matched entities and components.
        /// 
        /// Note that components are returned _by value_ and hence
        /// should not be modified.
        /// </summary>
        /// <returns>Entity and component enumerator.</returns>
        public IEnumerator<(Entity, T1, T2)> GetEnumerator() {
            foreach ((Entity entity, T1 data1) in _table1) {
                if (IsAllowed(entity) && _table2.TryGet(entity, out T2 data2)) {
                    yield return (entity, data1, data2);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <summary>
        /// Invokes the given callback for each matched entity and components.
        /// 
        /// Note that the components are reference types, and hence can be safely
        /// modified in this callback.
        /// </summary>
        /// <param name="callback">Callback to invoke for each match.</param>
        public void Each(Callback callback) {
            _table1.Each((Entity entity, ref T1 component1) => {
                if (IsAllowed(entity) && _table2.Contains(entity)) {
                    callback(entity, ref component1, ref _table2[entity]);
                }
            });
        }
    }

    /// <summary>
    /// Iterates over all entities with a particular component types.
    /// </summary>
    /// <typeparam name="T1">Type of required component.</typeparam>
    /// <typeparam name="T2">Type of required component.</typeparam>
    /// <typeparam name="T3">Type of required component.</typeparam>
    public sealed class View<T1, T2, T3> : ViewBase, IEnumerable<(Entity, T1, T2, T3)> where T1 : struct where T2 : struct where T3 : struct {
        private ComponentTable<T1> _table1;
        private ComponentTable<T2> _table2;
        private ComponentTable<T3> _table3;

        /// <summary>
        /// Callback for the Each method.
        /// </summary>
        /// <param name="entity">Matched entity.</param>
        /// <param name="component1">First component reference of matched entity.</param>
        /// <param name="component2">Second component reference of matched entity.</param>
        /// <param name="component3">Third component reference of matched entity.</param>
        public delegate void Callback(Entity entity, ref T1 component1, ref T2 component2, ref T3 component3);

        /// <summary>
        /// Excludes the specified component from the matched set.
        /// </summary>
        new public View<T1, T2, T3> Exclude<U>() where U : struct { base.Exclude<U>(); return this; }

        /// <summary>
        /// Marks the specified as required for the matched set.
        /// </summary>
        new public View<T1, T2, T3> Require<U>() where U : struct { base.Require<U>(); return this; }

        /// <summary>
        /// Constructs a new view over a single component type.
        /// </summary>
        /// <param name="world">World this view inspects.</param>
        public View(World world) : base(world) {
            _table1 = world.GetTable<T1>();
            _table2 = world.GetTable<T2>();
            _table3 = world.GetTable<T3>();
        }

        /// <summary>
        /// Enumerator for matched entities and components.
        /// 
        /// Note that components are returned _by value_ and hence
        /// should not be modified.
        /// </summary>
        /// <returns>Entity and component enumerator.</returns>
        public IEnumerator<(Entity, T1, T2, T3)> GetEnumerator() {
            foreach ((Entity entity, T1 data1) in _table1) {
                if (IsAllowed(entity) && _table2.TryGet(entity, out T2 data2) && _table3.TryGet(entity, out T3 data3)) {
                    yield return (entity, data1, data2, data3);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        /// <summary>
        /// Invokes the given callback for each matched entity and components.
        /// 
        /// Note that the components are reference types, and hence can be safely
        /// modified in this callback.
        /// </summary>
        /// <param name="callback">Callback to invoke for each match.</param>
        public void Each(Callback callback) {
            _table1.Each((Entity entity, ref T1 component1) => {
                if (IsAllowed(entity) && _table2.Contains(entity) && _table3.Contains(entity)) {
                    callback(entity, ref component1, ref _table2[entity], ref _table3[entity]);
                }
            });
        }
    }
}
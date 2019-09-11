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
    public class ViewBase {
        private World _world;
        private List<IComponentTable> _required = new List<IComponentTable>();
        private List<IComponentTable> _excluded = new List<IComponentTable>();

        protected ViewBase(World world) => _world = world;

        public void Exclude<T>() where T : struct {
            _excluded.Add(_world.GetTable<T>());
        }

        public void Require<T>() where T : struct {
            _required.Add(_world.GetTable<T>());
        }

        protected bool IsExcluded(Entity entity) {
            foreach (IComponentTable table in _excluded) {
                if (table.Contains(entity)) {
                    return true;
                }
            }
            return false;
        }

        protected bool HasRequired(Entity entity) {
            foreach (IComponentTable table in _required) {
                if (!table.Contains(entity)) {
                    return false;
                }
            }
            return true;
        }

        protected bool IsAllowed(Entity entity) {
            return !IsExcluded(entity) && HasRequired(entity);
        }
    }

    /// <summary>
    /// Iterators over all entities with a particular component type.
    /// </summary>
    /// <typeparam name="T">Type of required component.</typeparam>
    public class View<T> : ViewBase, IEnumerable<(Entity, T)> where T : struct {
        private ComponentTable<T> _table;

        public View(World world) : base(world) => _table = world.GetTable<T>();

        public delegate void Callback(Entity entity, ref T component);

        new public View<T> Exclude<U>() where U : struct { base.Exclude<U>(); return this; }
        new public View<T> Require<U>() where U : struct { base.Require<U>(); return this; }

        public IEnumerator<(Entity, T)> GetEnumerator() {
            foreach ((Entity entity, T component) in _table) {
                if (IsAllowed(entity)) {
                    yield return (entity, component);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public void Each(Callback callback) {
            _table.Each((Entity entity, ref T component) => {
                if (IsAllowed(entity)) {
                    callback(entity, ref component);
                }
            });
        }
    }

    /// <summary>
    /// Iterators over all entities with a particular component types.
    /// </summary>
    /// <typeparam name="T1">Type of required component.</typeparam>
    /// <typeparam name="T2">Type of required component.</typeparam>
    public class View<T1, T2> : ViewBase, IEnumerable<(Entity, T1, T2)> where T1 : struct where T2 : struct {
        private ComponentTable<T1> _table1;
        private ComponentTable<T2> _table2;

        public delegate void Callback(Entity entity, ref T1 component1, ref T2 component2);

        new public View<T1, T2> Exclude<U>() where U : struct { base.Exclude<U>(); return this; }
        new public View<T1, T2> Require<U>() where U : struct { base.Require<U>(); return this; }

        public View(World world) : base(world) {
            _table1 = world.GetTable<T1>();
            _table2 = world.GetTable<T2>();
        }

        public IEnumerator<(Entity, T1, T2)> GetEnumerator() {
            foreach ((Entity entity, T1 data1) in _table1) {
                if (IsAllowed(entity) && _table2.TryGet(entity, out T2 data2)) {
                    yield return (entity, data1, data2);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public void Each(Callback callback) {
            _table1.Each((Entity entity, ref T1 component1) => {
                if (IsAllowed(entity) && _table2.Contains(entity)) {
                    callback(entity, ref component1, ref _table2[entity]);
                }
            });
        }
    }

    /// <summary>
    /// Iterators over all entities with a particular component types.
    /// </summary>
    /// <typeparam name="T1">Type of required component.</typeparam>
    /// <typeparam name="T2">Type of required component.</typeparam>
    /// <typeparam name="T3">Type of required component.</typeparam>
    public class View<T1, T2, T3> : ViewBase, IEnumerable<(Entity, T1, T2, T3)> where T1 : struct where T2 : struct where T3 : struct{
        private ComponentTable<T1> _table1;
        private ComponentTable<T2> _table2;
        private ComponentTable<T3> _table3;

        public delegate void Callback(Entity entity, ref T1 component1, ref T2 component2, ref T3 component3);

        new public View<T1, T2, T3> Exclude<U>() where U : struct { base.Exclude<U>(); return this; }
        new public View<T1, T2, T3> Require<U>() where U : struct { base.Require<U>(); return this; }

        public View(World world) : base(world) {
            _table1 = world.GetTable<T1>();
            _table2 = world.GetTable<T2>();
            _table3 = world.GetTable<T3>();
        }

        public IEnumerator<(Entity, T1, T2, T3)> GetEnumerator() {
            foreach ((Entity entity, T1 data1) in _table1) {
                if (IsAllowed(entity) && _table2.TryGet(entity, out T2 data2) && _table3.TryGet(entity, out T3 data3)) {
                    yield return (entity, data1, data2, data3);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public void Each(Callback callback) {
            _table1.Each((Entity entity, ref T1 component1) => {
                if (IsAllowed(entity) && _table2.Contains(entity) && _table3.Contains(entity)) {
                    callback(entity, ref component1, ref _table2[entity], ref _table3[entity]);
                }
            });
        }
    }
}
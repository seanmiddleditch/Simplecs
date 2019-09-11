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
    /// Iterators over all entities with a particular component type.
    /// </summary>
    /// <typeparam name="T">Type of required component.</typeparam>
    public class View<T> : IEnumerable<(Entity, T)> where T : struct {
        private ComponentTable<T> _table;

        public View(World world) => _table = world.GetTable<T>();

        public delegate void Callback(Entity entity, ref T component);

        public IEnumerator<(Entity, T)> GetEnumerator() {
            return _table.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public void Each(Callback callback) {
            _table.Each((Entity entity, ref T component) => callback(entity, ref component));
        }
    }

    /// <summary>
    /// Iterators over all entities with a particular component types.
    /// </summary>
    /// <typeparam name="T1">Type of required component.</typeparam>
    /// <typeparam name="T2">Type of required component.</typeparam>
    public class View<T1, T2> : IEnumerable<(Entity, T1, T2)> where T1 : struct where T2 : struct {
        private ComponentTable<T1> _table1;
        private ComponentTable<T2> _table2;

        public delegate void Callback(Entity entity, ref T1 component1, ref T2 component2);

        public View(World world) {
            _table1 = world.GetTable<T1>();
            _table2 = world.GetTable<T2>();
        }

        public IEnumerator<(Entity, T1, T2)> GetEnumerator() {
            foreach ((Entity entity, T1 data1) in _table1) {
                if (_table2.TryGet(entity, out T2 data2)) {
                    yield return (entity, data1, data2);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return this.GetEnumerator();
        }

        public void Each(Callback callback) {
            _table1.Each((Entity entity, ref T1 component1) => {
                if (_table2.Contains(entity)) {
                    callback(entity, ref component1, ref _table2[entity]);
                }
            });
        }
    }
}
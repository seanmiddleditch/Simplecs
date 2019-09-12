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
    /// Iterates over all entities with a particular component type.
    /// </summary>
    /// <typeparam name="T">Type of required component.</typeparam>
    public sealed class View<T> : IEnumerableView<ViewTuple<T>> where T : struct {
        private ComponentTable<T> _table;
        private ViewPredicate _predicate;

        internal View(ComponentTable<T> table, ViewPredicate predicate) {
            _table = table;
            _predicate = predicate;
        }

        /// <summary>
        /// Enumerator for matched entities and components.
        /// 
        /// Note that components are returned _by value_ and hence
        /// should not be modified.
        /// </summary>
        /// <returns>Entity and component enumerator.</returns>
        public ViewEnumerator<View<T>, ViewTuple<T>> GetEnumerator() => new ViewEnumerator<View<T>, ViewTuple<T>>(this);

        IEnumerator<ViewTuple<T>> IEnumerable<ViewTuple<T>>.GetEnumerator() => this.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        int IEnumerableView<ViewTuple<T>>.MaximumIndex => _table.Count;
        bool IEnumerableView<ViewTuple<T>>.TryGetAt(int index, ref ViewTuple<T> tuple) {
            if (_predicate.IsAllowed(_table[index])) {
                tuple = new ViewTuple<T>(_table[index], _table);
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Iterates over all entities with a particular component types.
    /// </summary>
    /// <typeparam name="T1">Type of required component.</typeparam>
    /// <typeparam name="T2">Type of required component.</typeparam>
    public sealed class View<T1, T2> : IEnumerableView<ViewTuple<T1, T2>> where T1 : struct where T2 : struct {
        private ComponentTable<T1> _table1;
        private ComponentTable<T2> _table2;
        private ViewPredicate _predicate;

        internal View(ComponentTable<T1> table1, ComponentTable<T2> table2, ViewPredicate predicate) {
            _table1 = table1;
            _table2 = table2;
            _predicate = predicate;
        }

        /// <summary>
        /// Enumerator for matched entities and components.
        /// 
        /// Note that components are returned _by value_ and hence
        /// should not be modified.
        /// </summary>
        /// <returns>Entity and component enumerator.</returns>
        public ViewEnumerator<View<T1, T2>, ViewTuple<T1, T2>> GetEnumerator() => new ViewEnumerator<View<T1, T2>, ViewTuple<T1, T2>>(this);

        IEnumerator<ViewTuple<T1, T2>> IEnumerable<ViewTuple<T1, T2>>.GetEnumerator() => this.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        int IEnumerableView<ViewTuple<T1, T2>>.MaximumIndex => _table1.Count;
        bool IEnumerableView<ViewTuple<T1, T2>>.TryGetAt(int index, ref ViewTuple<T1, T2> tuple) {
            if (_predicate.IsAllowed(_table1[index]) && _table2.Contains(_table1[index])) {
                tuple = new ViewTuple<T1, T2>(_table1[index], _table1, _table2);
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// Iterates over all entities with a particular component types.
    /// </summary>
    /// <typeparam name="T1">Type of required component.</typeparam>
    /// <typeparam name="T2">Type of required component.</typeparam>
    /// <typeparam name="T3">Type of required component.</typeparam>
    public sealed class View<T1, T2, T3> : IEnumerableView<ViewTuple<T1, T2, T3>> where T1 : struct where T2 : struct where T3 : struct {
        private ComponentTable<T1> _table1;
        private ComponentTable<T2> _table2;
        private ComponentTable<T3> _table3;
        private ViewPredicate _predicate;

        internal View(ComponentTable<T1> table1, ComponentTable<T2> table2, ComponentTable<T3> table3, ViewPredicate predicate) {
            _table1 = table1;
            _table2 = table2;
            _table3 = table3;
            _predicate = predicate;
        }

        /// <summary>
        /// Enumerator for matched entities and components.
        /// 
        /// Note that components are returned _by value_ and hence
        /// should not be modified.
        /// </summary>
        /// <returns>Entity and component enumerator.</returns>
        public ViewEnumerator<View<T1, T2, T3>, ViewTuple<T1, T2, T3>> GetEnumerator() => new ViewEnumerator<View<T1, T2, T3>, ViewTuple<T1, T2, T3>>(this);

        IEnumerator<ViewTuple<T1, T2, T3>> IEnumerable<ViewTuple<T1, T2, T3>>.GetEnumerator() => this.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        int IEnumerableView<ViewTuple<T1, T2, T3>>.MaximumIndex => _table1.Count;
        bool IEnumerableView<ViewTuple<T1, T2, T3>>.TryGetAt(int index, ref ViewTuple<T1, T2, T3> tuple) {
            if (_predicate.IsAllowed(_table1[index]) && _table2.Contains(_table1[index]) && _table3.Contains(_table1[index])) {
                tuple = new ViewTuple<T1, T2, T3>(_table1[index], _table1, _table2, _table3);
                return true;
            }
            return false;
        }
    }
}
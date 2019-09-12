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
    /// Iterator over values in a View.
    /// </summary>
    /// <typeparam name="T">Component type of View.</typeparam>
    public sealed class ViewIterator<T> : IEnumerator<ViewTuple<T>> where T : struct {
        private ComponentTable<T> _table;
        private ViewPredicate _predicate;
        private int _index = -1;

        /// <returns>Current value of iterator.</returns>
        public ViewTuple<T> Current => new ViewTuple<T>(_table[_index], _table);

        object? IEnumerator.Current => this.Current;

        internal ViewIterator(ComponentTable<T> table, ViewPredicate predicate) {
            _table = table;
            _predicate = predicate;
        }

        /// <summary>
        /// Dispose of iterator.
        /// </summary>
        public void Dispose() { }

        /// <summary>
        /// Advances the iterator.
        /// </summary>
        /// <returns>True if there is more data.</returns>
        public bool MoveNext() {
            if (_index == _table.Count) {
                return false;
            }

            ++_index;
            while (_index < _table.Count && !_predicate.IsAllowed(_table[_index])) {
                ++_index;
            }

            return _index != _table.Count;
        }

        /// <summary>
        /// Resets the iterator to the beginning.
        /// </summary>
        public void Reset() => _index = -1;
    }

    /// <summary>
    /// Iterator over values in a View.
    /// </summary>
    /// <typeparam name="T1">Component type of View.</typeparam>
    /// <typeparam name="T2">Component type of View.</typeparam>
    public sealed class ViewIterator<T1, T2> : IEnumerator<ViewTuple<T1, T2>> where T1 : struct where T2 : struct {
        private ComponentTable<T1> _table1;
        private ComponentTable<T2> _table2;
        private ViewPredicate _predicate;
        private int _index = -1;

        /// <returns>Current value of iterator.</returns>
        public ViewTuple<T1, T2> Current => new ViewTuple<T1, T2>(_table1[_index], _table1, _table2);

        object? IEnumerator.Current => this.Current;

        internal ViewIterator(ComponentTable<T1> table1, ComponentTable<T2> table2, ViewPredicate predicate) {
            _table1 = table1;
            _table2 = table2;
            _predicate = predicate;
        }

        /// <summary>
        /// Dispose of iterator.
        /// </summary>
        public void Dispose() { }

        /// <summary>
        /// Advances the iterator.
        /// </summary>
        /// <returns>True if there is more data.</returns>
        public bool MoveNext() {
            if (_index == _table1.Count) {
                return false;
            }

            ++_index;
            while (_index < _table1.Count && (!_predicate.IsAllowed(_table1[_index]) || !_table2.Contains(_table1[_index]))) {
                ++_index;
            }

            return _index != _table1.Count;
        }

        /// <summary>
        /// Resets the iterator to the beginning.
        /// </summary>
        public void Reset() => _index = -1;
    }

    /// <summary>
    /// Iterator over values in a View.
    /// </summary>
    /// <typeparam name="T1">Component type of View.</typeparam>
    /// <typeparam name="T2">Component type of View.</typeparam>
    /// <typeparam name="T3">Component type of View.</typeparam>
    public sealed class ViewIterator<T1, T2, T3> : IEnumerator<ViewTuple<T1, T2, T3>> where T1 : struct where T2 : struct where T3 : struct {
        private ComponentTable<T1> _table1;
        private ComponentTable<T2> _table2;
        private ComponentTable<T3> _table3;
        private ViewPredicate _predicate;
        private int _index = -1;

        /// <returns>Current value of iterator.</returns>
        public ViewTuple<T1, T2, T3> Current => new ViewTuple<T1, T2, T3>(_table1[_index], _table1, _table2, _table3);

        object? IEnumerator.Current => this.Current;

        internal ViewIterator(ComponentTable<T1> table1, ComponentTable<T2> table2, ComponentTable<T3> table3, ViewPredicate predicate) {
            _table1 = table1;
            _table2 = table2;
            _table3 = table3;
            _predicate = predicate;
        }

        /// <summary>
        /// Dispose of iterator.
        /// </summary>
        public void Dispose() { }

        /// <summary>
        /// Advances the iterator.
        /// </summary>
        /// <returns>True if there is more data.</returns>
        public bool MoveNext() {
            if (_index == _table1.Count) {
                return false;
            }

            ++_index;
            while (_index < _table1.Count && (!_predicate.IsAllowed(_table1[_index]) || !_table2.Contains(_table1[_index]) || !_table3.Contains(_table1[_index]))) {
                ++_index;
            }

            return _index != _table1.Count;
        }

        /// <summary>
        /// Resets the iterator to the beginning.
        /// </summary>
        public void Reset() => _index = -1;
    }
}
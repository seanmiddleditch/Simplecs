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
    public class ViewIterator<T> : IEnumerator<ViewTuple<T>> where T : struct {
        private ComponentTable<T> _table;
        private ViewPredicate _predicate;
        private int _index = -1;

        public ViewTuple<T> Current => new ViewTuple<T>(_table[_index], _table);
        object? IEnumerator.Current => this.Current;

        internal ViewIterator(ComponentTable<T> table, ViewPredicate predicate) {
            _table = table;
            _predicate = predicate;
        }

        public void Dispose() {}

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

        public void Reset() => _index = -1;
    }
}
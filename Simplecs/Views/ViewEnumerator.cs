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
using System.Collections;
using System.Collections.Generic;

namespace Simplecs.Views {
    /// <summary>
    /// Enumerator for a View.
    /// </summary>
    /// <note>
    /// The Enumerator is invalidated if the underlying component tables are modified.
    /// 
    /// We currently do not protected against nor detect this situation.
    /// </note>
    /// <typeparam name="Binder">View being enumerated.</typeparam>
    /// <typeparam name="Binding">Type of iteration.</typeparam>
    public struct ViewEnumerator<Binder, Binding> : IEnumerator<Binding> where Binder : IBinder<Binding> where Binding : struct {
        private Entity _current;
        private Binder _binder;
        private int _index;

        /// <returns>Current value of iterator.</returns>
        public Binding Current => _binder.Bind(_current);

        Binding IEnumerator<Binding>.Current => Current;
        object? IEnumerator.Current => throw new NotImplementedException();

        internal ViewEnumerator(Binder binder) => (_current, _binder, _index) = (Entity.Invalid, binder, -1);

        /// <summary>
        /// Dispose of iterator.
        /// </summary>
        public void Dispose() {}

        /// <summary>
        /// Advances the iterator.
        /// </summary>
        /// <returns>True if there is more data.</returns>
        public bool MoveNext() {
            // If the current entity has changed (e.g. been deleted from under us)
            // then don't initially increment the index. This allows Views to be used
            // to loop over entities and destroy them.
            //
            if (_index == -1 || _current == _binder.PotentialEntityAt(_index)) {
                ++_index;
            }

            while (true) {
                _current = _binder.PotentialEntityAt(_index);
                if (_current == Entity.Invalid) {
                    return false;
                }

                if (_binder.Contains(_current)) {
                    return true;
                }

                ++_index;
            }
        }

        /// <summary>
        /// Resets the iterator to the beginning.
        /// </summary>
        public void Reset() => (_current, _index) = (Entity.Invalid, -1);
    }
}
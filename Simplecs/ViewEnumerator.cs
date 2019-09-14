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

namespace Simplecs {
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
        private Binder _binder;
        private int _index;

        /// <returns>Current value of iterator.</returns>
        public Binding Current => _binder.Bind(_binder.PotentialEntities[_index]);

        Binding IEnumerator<Binding>.Current => Current;
        object? IEnumerator.Current => throw new NotImplementedException();

        internal ViewEnumerator(Binder binder) => (_binder, _index) = (binder, -1);

        /// <summary>
        /// Dispose of iterator.
        /// </summary>
        public void Dispose() {}

        /// <summary>
        /// Advances the iterator.
        /// </summary>
        /// <returns>True if there is more data.</returns>
        public bool MoveNext() {
            var entities = _binder.PotentialEntities;

            if (_index == entities.Count) {
                return false;
            }

            while (++_index < entities.Count) {
                var entity = entities[_index];
                if (_binder.Contains(entity)) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Resets the iterator to the beginning.
        /// </summary>
        public void Reset() => _index = -1;
    }
}
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
    /// Minimum interface necessary for an enumerable view.
    /// 
    /// This is effectively an internal interface.
    /// </summary>
    /// <typeparam name="Tuple">Tuple type being enumerated.</typeparam>
    public interface IEnumerableView<Tuple> : IEnumerable<Tuple> where Tuple : struct {
        /// <value>Maximum (exclusive) index that is valid for the view.</value>
        int MaximumIndex { get; }

        /// <param name="index">Index to query.</param>
        /// <param name="tuple">Tuple which will contain the updated values.</param>
        /// <returns>True if the index references a matching entity.</returns>
        bool TryGetAt(int index, ref Tuple tuple);
    }

    /// <summary>
    /// Enumerator for a View.
    /// </summary>
    /// <typeparam name="View">View being enumerated.</typeparam>
    /// <typeparam name="Tuple">Tuple type of iteration.</typeparam>
    public sealed class ViewEnumerator<View, Tuple> : IEnumerator<Tuple> where View : IEnumerableView<Tuple> where Tuple : struct {
        private View _view;
        private int _index = -1;
        private Tuple _tuple;

        /// <returns>Current value of iterator.</returns>
        public ref Tuple Current => ref _tuple;

        Tuple IEnumerator<Tuple>.Current => _tuple;
        object? IEnumerator.Current => _tuple;

        internal ViewEnumerator(View view) => _view = view;

        /// <summary>
        /// Dispose of iterator.
        /// </summary>
        public void Dispose() { }

        /// <summary>
        /// Advances the iterator.
        /// </summary>
        /// <returns>True if there is more data.</returns>
        public bool MoveNext() {
            int max = _view.MaximumIndex;

            if (_index == max) {
                return false;
            }

            while (++_index < max) {
                if (_view.TryGetAt(_index, ref _tuple)) {
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
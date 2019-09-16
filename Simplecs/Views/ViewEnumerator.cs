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
using Simplecs.Containers;

namespace Simplecs.Views {
    /// <summary>
    /// Enumerator for rows in a view.
    /// </summary>
    public struct ViewEnumerator<RowT> : IEnumerator<RowT> where RowT : struct {
        private readonly IView<RowT> _view;
        private EntityEnumerator _entities;
        private RowT _row;

        internal ViewEnumerator(IView<RowT> view, IComponentTable table) => (_view, _entities, _row) = (view, new EntityEnumerator(table), default(RowT));

        /// <summary>Current row.</summary>
        public RowT Current => _row;
        object? IEnumerator.Current => throw new NotImplementedException();

        /// <summary>Attempt to increment enumerator.</summary>
        public bool MoveNext() => _entities.MoveNext(_view, out _row);
        /// <summary>Reset enumerator to initial state.</summary>
        public void Reset() => _entities.Reset();
        /// <summary>Dispose of the enumerator.</summary>
        public void Dispose() { }
    }
}
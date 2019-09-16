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
    public struct ViewEnumerator<T> : IEnumerator<ViewRow<T>> where T : struct {
        private readonly View<T> _view;
        private EntityEnumerator _entities;

        internal ViewEnumerator(View<T> view, ComponentTable<T> table) => (_view, _entities) = (view, new EntityEnumerator(table));

        public ViewRow<T> Current => new ViewRow<T>(_view, _entities.Current);
        object? IEnumerator.Current => throw new NotImplementedException();

        public bool MoveNext() => _entities.MoveNext(_view);
        public void Reset() => _entities.Reset();
        public void Dispose() { }
    }

    public struct ViewEnumerator<T1, T2> : IEnumerator<ViewRow<T1, T2>>
        where T1 : struct
        where T2 : struct {
        private readonly View<T1, T2> _view;
        private EntityEnumerator _entities;

        internal ViewEnumerator(View<T1, T2> view, IComponentTable table) => (_view, _entities) = (view, new EntityEnumerator(table));

        public ViewRow<T1, T2> Current => new ViewRow<T1, T2>(_view, _entities.Current);
        object? IEnumerator.Current => throw new NotImplementedException();

        public bool MoveNext() => _entities.MoveNext(_view);
        public void Reset() => _entities.Reset();
        public void Dispose() { }
    }

    public struct ViewEnumerator<T1, T2, T3> : IEnumerator<ViewRow<T1, T2, T3>>
        where T1 : struct
        where T2 : struct
        where T3 : struct {
        private readonly View<T1, T2, T3> _view;
        private EntityEnumerator _entities;

        internal ViewEnumerator(View<T1, T2, T3> view, IComponentTable table) => (_view, _entities) = (view, new EntityEnumerator(table));

        public ViewRow<T1, T2, T3> Current => new ViewRow<T1, T2, T3>(_view, _entities.Current);
        object? IEnumerator.Current => throw new NotImplementedException();

        public bool MoveNext() => _entities.MoveNext(_view);
        public void Reset() => _entities.Reset();
        public void Dispose() { }
    }
}
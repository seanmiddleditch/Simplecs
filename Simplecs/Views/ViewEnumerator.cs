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
    internal static class EnumeratorUtility {
        internal static bool MoveNext<View, Table>(ref int index, ref Entity entity, View view, Table table)
            where View : IView
            where Table : IComponentTable {
            // If the current entity has changed (e.g. been deleted from under us)
            // then don't initially increment the index. This allows Views to be used
            // to loop over entities and destroy them.
            //
            if (index == -1 || (index < table.Count && entity == table.EntityAt(index))) {
                ++index;
            }

            while (index < table.Count) {
                entity = table.EntityAt(index);
                if (view.Contains(entity)) {
                    return true;
                }

                ++index;
            }

            return false;
        }
    }

    public struct ViewEnumerator<T> : IEnumerator<ViewRow<T>> where T : struct {
        private View<T> _view;
        private ComponentTable<T> _table;
        private Entity _entity;
        private int _index;

        internal ViewEnumerator(View<T> view, ComponentTable<T> table) => (_view, _table, _entity, _index) = (view, table, Entity.Invalid, -1);

        public ViewRow<T> Current => new ViewRow<T>(_view, _entity);
        ViewRow<T> IEnumerator<ViewRow<T>>.Current => Current;
        object? IEnumerator.Current => throw new NotImplementedException();

        public bool MoveNext() => EnumeratorUtility.MoveNext(ref _index, ref _entity, _view, _table);

        public void Reset() => (_entity, _index) = (Entity.Invalid, -1);
        public void Dispose() { }
    }

    public struct ViewEnumerator2<T1, T2> : IEnumerator<ViewRow<T1, T2>>
        where T1 : struct
        where T2 : struct {
        private View<T1, T2> _view;
        private ComponentTable<T1> _table;
        private Entity _entity;
        private int _index;

        internal ViewEnumerator2(View<T1, T2> view, ComponentTable<T1> table) => (_view, _table, _entity, _index) = (view, table, Entity.Invalid, -1);

        public ViewRow<T1, T2> Current => new ViewRow<T1, T2>(_view, _entity);
        ViewRow<T1, T2> IEnumerator<ViewRow<T1, T2>>.Current => Current;
        object? IEnumerator.Current => throw new NotImplementedException();

        public bool MoveNext() => EnumeratorUtility.MoveNext(ref _index, ref _entity, _view, _table);

        public void Reset() => (_entity, _index) = (Entity.Invalid, -1);
        public void Dispose() { }
    }

    public struct ViewEnumerator2<T1, T2, T3> : IEnumerator<ViewRow<T1, T2, T3>>
        where T1 : struct
        where T2 : struct
        where T3 : struct {
        private View<T1, T2, T3> _view;
        private ComponentTable<T1> _table;
        private Entity _entity;
        private int _index;

        internal ViewEnumerator2(View<T1, T2, T3> view, ComponentTable<T1> table) => (_view, _table, _entity, _index) = (view, table, Entity.Invalid, -1);

        public ViewRow<T1, T2, T3> Current => new ViewRow<T1, T2, T3>(_view, _entity);
        ViewRow<T1, T2, T3> IEnumerator<ViewRow<T1, T2, T3>>.Current => Current;
        object? IEnumerator.Current => throw new NotImplementedException();

        public bool MoveNext() => EnumeratorUtility.MoveNext(ref _index, ref _entity, _view, _table);

        public void Reset() => (_entity, _index) = (Entity.Invalid, -1);
        public void Dispose() { }
    }
}
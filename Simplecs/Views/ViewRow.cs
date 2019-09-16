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

namespace Simplecs.Views {
    /// <summary>
    /// Single row or set of components for a given view.
    /// </summary>
    public readonly struct ViewRow<T> where T : struct {
        private readonly View<T> _view;
        private readonly int _index;

        /// <summary>Entity of current row.</summary>
        public Entity Entity { get; }
        /// <summary>Component of current row.</summary>
        public ref T Component => ref _view.Table.ReferenceAt(Entity, _index);

        internal ViewRow(View<T> view, Entity entity) => (_view, Entity, _index) = (view, entity, view.Table.IndexOf(entity));
    }

    /// <summary>
    /// Single row or set of components for a given view.
    /// </summary>
    public readonly struct ViewRow<T1, T2>
        where T1 : struct
        where T2 : struct {
        private readonly View<T1, T2> _view;
        private readonly int _index1, _index2;

        /// <summary>Entity of current row.</summary>
        public Entity Entity { get; }
        /// <summary>First component of current row.</summary>
        public ref T1 Component1 => ref _view.Table1.ReferenceAt(Entity, _index1);
        /// <summary>Second component of current row.</summary>
        public ref T2 Component2 => ref _view.Table2.ReferenceAt(Entity, _index2);

        internal ViewRow(View<T1, T2> view, Entity entity) => (_view, Entity, _index1, _index2) = (view, entity, view.Table1.IndexOf(entity), view.Table2.IndexOf(entity));
    }

    /// <summary>
    /// Single row or set of components for a given view.
    /// </summary>
    public readonly struct ViewRow<T1, T2, T3>
        where T1 : struct
        where T2 : struct
        where T3 : struct {
        private readonly View<T1, T2, T3> _view;
        private readonly int _index1, _index2, _index3;

        /// <summary>Entity of current row.</summary>
        public Entity Entity { get; }
        /// <summary>First component of current row.</summary>
        public ref T1 Component1 => ref _view.Table1.ReferenceAt(Entity, _index1);
        /// <summary>Second component of current row.</summary>
        public ref T2 Component2 => ref _view.Table2.ReferenceAt(Entity, _index2);
        /// <summary>Third component of current row.</summary>
        public ref T3 Component3 => ref _view.Table3.ReferenceAt(Entity, _index3);

        internal ViewRow(View<T1, T2, T3> view, Entity entity) => (_view, Entity, _index1, _index2, _index3) = (view, entity, view.Table1.IndexOf(entity), view.Table2.IndexOf(entity), view.Table3.IndexOf(entity));

    }
}
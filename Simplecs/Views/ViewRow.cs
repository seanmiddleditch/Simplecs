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
    public struct ViewRow<T> where T : struct {
        private View<T> _view;

        public Entity Entity { get; internal set; }

        internal ViewRow(View<T> view, Entity entity) => (_view, Entity) = (view, entity);

        public ref T Component => ref _view.Component[Entity];
    }

    public struct ViewRow<T1, T2>
        where T1 : struct
        where T2 : struct {
        private View<T1, T2> _view;

        public Entity Entity { get; internal set; }

        internal ViewRow(View<T1, T2> view, Entity entity) => (_view, Entity) = (view, entity);

        public ref T1 Component1 => ref _view.Component1[Entity];
        public ref T2 Component2 => ref _view.Component2[Entity];
    }

    public struct ViewRow<T1, T2, T3>
        where T1 : struct
        where T2 : struct
        where T3 : struct {
        private View<T1, T2, T3> _view;

        public Entity Entity { get; internal set; }

        internal ViewRow(View<T1, T2, T3> view, Entity entity) => (_view, Entity) = (view, entity);

        public ref T1 Component1 => ref _view.Component1[Entity];
        public ref T2 Component2 => ref _view.Component2[Entity];
        public ref T3 Component3 => ref _view.Component3[Entity];
    }
}
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
    public readonly struct ViewRow<T> where T : struct {
        private readonly View<T> _view;
        private readonly RowKey _key;

        public Entity Entity => _key.Entity;
        public ref T Component => ref _view.Component[_key];

        internal ViewRow(View<T> view, RowKey key) => (_view, _key) = (view, key);
    }

    public readonly struct ViewRow<T1, T2>
        where T1 : struct
        where T2 : struct {
        private readonly View<T1, T2> _view;
        private readonly RowKey _key;
        private readonly int _index2;

        public Entity Entity => _key.Entity;
        public ref T1 Component1 => ref _view.Component1[_key];
        public ref T2 Component2 => ref _view.Component2[new RowKey(Entity, _index2)];

        internal ViewRow(View<T1, T2> view, RowKey key) => (_view, _key, _index2) = (view, key, view.Component2.IndexOf(key.Entity));
    }

    public readonly struct ViewRow<T1, T2, T3>
        where T1 : struct
        where T2 : struct
        where T3 : struct {
        private readonly View<T1, T2, T3> _view;
        private readonly RowKey _key;
        private readonly int _index2, _index3;

        public Entity Entity => _key.Entity;
        public ref T1 Component1 => ref _view.Component1[_key];
        public ref T2 Component2 => ref _view.Component2[new RowKey(Entity, _index2)];
        public ref T3 Component3 => ref _view.Component3[new RowKey(Entity, _index3)];

        internal ViewRow(View<T1, T2, T3> view, RowKey key) => (_view, _key, _index2, _index3) = (view, key, view.Component2.IndexOf(key.Entity), view.Component3.IndexOf(key.Entity));

    }
}
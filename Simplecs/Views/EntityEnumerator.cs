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

using Simplecs.Containers;

namespace Simplecs.Views {
    internal struct EntityEnumerator {
        private readonly IComponentTable _table;
        private Entity _entity;
        private int _index;

        public RowKey Current => new RowKey(_entity, _index);

        public EntityEnumerator(IComponentTable table) => (_table, _entity, _index) = (table, Entity.Invalid, -1);

        internal bool MoveNext<View>(View view) where View : IView {
            // If the current entity has changed (e.g. been deleted from under us)
            // then don't initially increment the index. This allows Views to be used
            // to loop over entities and destroy them.
            //
            if (_index == -1 || (_index < _table.Count && _entity == _table.EntityAt(_index))) {
                ++_index;
            }

            while (_index < _table.Count) {
                _entity = _table.EntityAt(_index);
                if (view.Contains(_entity)) {
                    return true;
                }

                ++_index;
            }

            return false;
        }

        internal void Reset() => (_entity, _index) = (Entity.Invalid, -1);
    }
}
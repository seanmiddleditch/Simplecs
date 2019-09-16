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

        public Entity Entity { get; private set; }
        public int Index { get; private set; }

        public EntityEnumerator(IComponentTable table) => (_table, Entity, Index) = (table, Entity.Invalid, -1);

        internal bool MoveNext<RowT>(IView<RowT> view, out RowT row) where RowT : struct {
            // If the current entity has changed (e.g. been deleted from under us)
            // then don't initially increment the index. This allows Views to be used
            // to loop over entities and destroy them.
            //
            if (Index == -1 || (Index < _table.Count && Entity == _table.EntityAt(Index))) {
                ++Index;
            }

            while (Index < _table.Count) {
                Entity = _table.EntityAt(Index);
                if (view.TryBindRow(Entity, out row)) {
                    return true;
                }

                ++Index;
            }

            row = default(RowT);
            return false;
        }

        internal void Reset() => (Entity, Index) = (Entity.Invalid, -1);
    }
}
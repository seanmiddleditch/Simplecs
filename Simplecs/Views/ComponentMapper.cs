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

namespace Simplecs {
    /// <summary>
    /// Wrapper that provides array-like indexing from an entity to a component.
    /// </summary>
    public struct ComponentMapper<T> where T : struct {
        private ComponentTable<T> _table;

        internal ComponentMapper(ComponentTable<T> table) => _table = table;

        public ref T this[Entity entity] => ref _table.GetComponentRef(entity, _table.IndexOf(entity));
    }

}
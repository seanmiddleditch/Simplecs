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

namespace Simplecs {
    /// <summary>
    /// Accessor for an entry from a View iterator.
    /// </summary>
    public struct ViewTuple<T> where T : struct {
        private ComponentTable<T> _table;

        /// <value>Current entity key.</value>
        public Entity Entity { get; }

        /// <value>Current component data.</value>
        public ref T Component => ref _table[Entity];

        internal ViewTuple(Entity entity, ComponentTable<T> table) {
            Entity = entity;
            _table = table;
        }
    }
}
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

    /// <summary>
    /// Accessor for an entry from a View iterator.
    /// </summary>
    public struct ViewTuple<T1, T2> where T1 : struct where T2 : struct {
        private ComponentTable<T1> _table1;
        private ComponentTable<T2> _table2;

        /// <value>Current entity key.</value>
        public Entity Entity { get; }

        /// <value>Current component data.</value>
        public ref T1 Component1 => ref _table1[Entity];

        /// <value>Current component data.</value>
        public ref T2 Component2 => ref _table2[Entity];

        internal ViewTuple(Entity entity, ComponentTable<T1> table1, ComponentTable<T2> table2) {
            Entity = entity;
            _table1 = table1;
            _table2 = table2;
        }
    }

    /// <summary>
    /// Accessor for an entry from a View iterator.
    /// </summary>
    public struct ViewTuple<T1, T2, T3> where T1 : struct where T2 : struct where T3 : struct {
        private ComponentTable<T1> _table1;
        private ComponentTable<T2> _table2;
        private ComponentTable<T3> _table3;

        /// <value>Current entity key.</value>
        public Entity Entity { get; }

        /// <value>Current component data.</value>
        public ref T1 Component1 => ref _table1[Entity];

        /// <value>Current component data.</value>
        public ref T2 Component2 => ref _table2[Entity];

        /// <value>Current component data.</value>
        public ref T3 Component3 => ref _table3[Entity];

        internal ViewTuple(Entity entity, ComponentTable<T1> table1, ComponentTable<T2> table2, ComponentTable<T3> table3) {
            Entity = entity;
            _table1 = table1;
            _table2 = table2;
            _table3 = table3;
        }
    }
}
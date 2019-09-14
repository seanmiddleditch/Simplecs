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

using System.Collections.Generic;

namespace Simplecs {
    /// <summary>
    /// Factory for bindings.
    /// </summary>
    /// <typeparam name="Binding">Binding accessed by this Binder.</typeparam>
    public interface IBinder<Binding> where Binding : struct {
        /// <summary>
        /// Checks if a given entity can be bound by this binder.
        /// </summary>
        /// <param name="entity">Entity to check.</param>
        /// <returns>True if the entity can be bound.</returns>
        bool Contains(Entity entity);

        /// <summary>
        /// Binds an entity.
        /// </summary>
        /// <note>
        /// It is illegal to call this for entities that are not bindable.
        /// 
        /// Use <see cref="Contains">Contains</see> to check if an entity is bindable.
        /// </note>
        /// <param name="entity">Entity to bind.</param>
        /// <returns>Binding.</returns>
        Binding Bind(Entity entity);

        /// <summary>
        /// Retrieves a list of _potential_ entities that may be bound.
        /// </summary>
        /// <value>List of entities that may be bound.</value>
        IReadOnlyList<Entity> PotentialEntities { get; }
    }

    /// <summary>
    /// Accessor for an entry from a View iterator.
    /// </summary>
    public struct Binding<T> where T : struct {
        private ComponentTable<T> _table;

        /// <value>Current entity key.</value>
        public Entity Entity { get; }

        /// <value>Current component data.</value>
        public ref T Component => ref _table[Entity];

        internal Binding(Entity entity, ComponentTable<T> table) {
            Entity = entity;
            _table = table;
        }

        /// <summary>
        /// Binder for this binding.
        /// </summary>
        public struct Binder : IBinder<Binding<T>> {
            internal ViewPredicate Predicate;
            internal ComponentTable<T> Table;

            internal Binder(ComponentTable<T> table, ViewPredicate predicate) => (Table, Predicate) = (table, predicate);
            
            bool IBinder<Binding<T>>.Contains(Entity entity) => Predicate.IsAllowed(entity) && Table.Contains(entity);
            Binding<T> IBinder<Binding<T>>.Bind(Entity entity) => new Binding<T>(entity, Table);
            IReadOnlyList<Entity> IBinder<Binding<T>>.PotentialEntities => Table.Entities;
        }
    }

    /// <summary>
    /// Accessor for an entry from a View iterator.
    /// </summary>
    public struct Binding<T1, T2> where T1 : struct where T2 : struct {
        private ComponentTable<T1> _table1;
        private ComponentTable<T2> _table2;

        /// <value>Current entity key.</value>
        public Entity Entity { get; }

        /// <value>Current component data.</value>
        public ref T1 Component1 => ref _table1[Entity];

        /// <value>Current component data.</value>
        public ref T2 Component2 => ref _table2[Entity];

        internal Binding(Entity entity, ComponentTable<T1> table1, ComponentTable<T2> table2) {
            Entity = entity;
            _table1 = table1;
            _table2 = table2;
        }

        /// <summary>
        /// Binder for this binding.
        /// </summary>
        public struct Binder : IBinder<Binding<T1, T2>> {
            internal ViewPredicate Predicate;
            internal ComponentTable<T1> Table1;
            internal ComponentTable<T2> Table2;

            internal Binder(ComponentTable<T1> table1, ComponentTable<T2> table2, ViewPredicate predicate) => (Table1, Table2, Predicate) = (table1, table2, predicate);

            bool IBinder<Binding<T1, T2>>.Contains(Entity entity) => Predicate.IsAllowed(entity) && Table1.Contains(entity) && Table2.Contains(entity);
            Binding<T1, T2> IBinder<Binding<T1, T2>>.Bind(Entity entity) => new Binding<T1, T2>(entity, Table1, Table2);
            IReadOnlyList<Entity> IBinder<Binding<T1, T2>>.PotentialEntities => Table1.Entities;
        }
    }

    /// <summary>
    /// Accessor for an entry from a View iterator.
    /// </summary>
    public struct Binding<T1, T2, T3> where T1 : struct where T2 : struct where T3 : struct {
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

        internal Binding(Entity entity, ComponentTable<T1> table1, ComponentTable<T2> table2, ComponentTable<T3> table3) {
            Entity = entity;
            _table1 = table1;
            _table2 = table2;
            _table3 = table3;
        }

        /// <summary>
        /// Binder for this binding.
        /// </summary>
        public struct Binder : IBinder<Binding<T1, T2, T3>> {
            internal ViewPredicate Predicate;
            internal ComponentTable<T1> Table1;
            internal ComponentTable<T2> Table2;
            internal ComponentTable<T3> Table3;

            internal Binder(ComponentTable<T1> table1, ComponentTable<T2> table2, ComponentTable<T3> table3, ViewPredicate predicate) => (Table1, Table2, Table3, Predicate) = (table1, table2, table3, predicate);

            bool IBinder<Binding<T1, T2, T3>>.Contains(Entity entity) => Predicate.IsAllowed(entity) && Table1.Contains(entity) && Table2.Contains(entity) && Table3.Contains(entity);
            Binding<T1, T2, T3> IBinder<Binding<T1, T2, T3>>.Bind(Entity entity) => new Binding<T1, T2, T3>(entity, Table1, Table2, Table3);
            IReadOnlyList<Entity> IBinder<Binding<T1, T2, T3>>.PotentialEntities => Table1.Entities;
        }
    }
}
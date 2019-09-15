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
using Simplecs.Containers;

namespace Simplecs.Views {
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
        private Entity _entity;
        private ComponentTable<T> _table;
        private int _index;

        /// <value>Current entity key.</value>
        public Entity Entity => _entity;

        /// <value>Current component data.</value>
        public ref T Component => ref _table.GetComponentRef(Entity, _index);

        internal Binding(Entity entity, ComponentTable<T> table) {
            _entity = entity;
            _table = table;
            _index = _table.IndexOf(entity);
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
        private Entity _entity;
        private Tables _tables;
        private int _index1, _index2;

        /// <value>Current entity key.</value>
        public Entity Entity => _entity;

        /// <value>Current component data.</value>
        public ref T1 Component1 => ref _tables.Table1.GetComponentRef(Entity, _index1);

        /// <value>Current component data.</value>
        public ref T2 Component2 => ref _tables.Table2.GetComponentRef(Entity, _index2);

        internal Binding(Entity entity, Tables tables) => (_entity, _tables, _index1, _index2) = (entity, tables, tables.Table1.IndexOf(entity), tables.Table2.IndexOf(entity));

        internal struct Tables {
            public ComponentTable<T1> Table1;
            public ComponentTable<T2> Table2;
        }

        /// <summary>
        /// Binder for this binding.
        /// </summary>
        public struct Binder : IBinder<Binding<T1, T2>> {
            private ViewPredicate _predicate;
            private Tables _tables;

            internal Binder(ComponentTable<T1> table1, ComponentTable<T2> table2, ViewPredicate predicate) => (_tables.Table1, _tables.Table2, _predicate) = (table1, table2, predicate);

            bool IBinder<Binding<T1, T2>>.Contains(Entity entity) => _predicate.IsAllowed(entity) && _tables.Table1.Contains(entity) && _tables.Table2.Contains(entity);
            Binding<T1, T2> IBinder<Binding<T1, T2>>.Bind(Entity entity) => new Binding<T1, T2>(entity, _tables);
            IReadOnlyList<Entity> IBinder<Binding<T1, T2>>.PotentialEntities => _tables.Table1.Entities;
        }
    }

    /// <summary>
    /// Accessor for an entry from a View iterator.
    /// </summary>
    public struct Binding<T1, T2, T3> where T1 : struct where T2 : struct where T3 : struct {
        private Entity _entity;
        private Tables _tables;
        private int _index1, _index2, _index3;

        /// <value>Current entity key.</value>
        public Entity Entity => _entity;

        /// <value>Current component data.</value>
        public ref T1 Component1 => ref _tables.Table1.GetComponentRef(Entity, _index1);

        /// <value>Current component data.</value>
        public ref T2 Component2 => ref _tables.Table2.GetComponentRef(Entity, _index2);

        /// <value>Current component data.</value>
        public ref T3 Component3 => ref _tables.Table3.GetComponentRef(Entity, _index3);

        internal Binding(Entity entity, Tables tables) => (_entity, _tables, _index1, _index2, _index3) = (entity, tables, tables.Table1.IndexOf(entity), tables.Table2.IndexOf(entity), tables.Table3.IndexOf(entity));

        internal struct Tables {
            public ComponentTable<T1> Table1;
            public ComponentTable<T2> Table2;
            public ComponentTable<T3> Table3;
        }

        /// <summary>
        /// Binder for this binding.
        /// </summary>
        public struct Binder : IBinder<Binding<T1, T2, T3>> {
            private ViewPredicate _predicate;
            private Tables _tables;

            internal Binder(ComponentTable<T1> table1, ComponentTable<T2> table2, ComponentTable<T3> table3, ViewPredicate predicate) => (_tables.Table1, _tables.Table2, _tables.Table3, _predicate) = (table1, table2, table3, predicate);

            bool IBinder<Binding<T1, T2, T3>>.Contains(Entity entity) => _predicate.IsAllowed(entity) && _tables.Table1.Contains(entity) && _tables.Table2.Contains(entity) && _tables.Table3.Contains(entity);
            Binding<T1, T2, T3> IBinder<Binding<T1, T2, T3>>.Bind(Entity entity) => new Binding<T1, T2, T3>(entity, _tables);
            IReadOnlyList<Entity> IBinder<Binding<T1, T2, T3>>.PotentialEntities => _tables.Table1.Entities;
        }
    }
}
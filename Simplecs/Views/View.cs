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

using System;
using System.Collections;
using System.Collections.Generic;
using Simplecs.Containers;

namespace Simplecs.Views {
    /// <summary>
    /// A collection of entities that match a particular signature.
    /// </summary>
    public interface IView {
        /// <summary>
        /// Checks if the given entity is contained by the view.
        /// </summary>
        /// <param name="entity">Entity to check.</param>
        /// <returns>True if the entity matches the view's signature.</returns>
        bool Contains(Entity entity);
    }

    public sealed class View<T> : IView, IEnumerable<ViewRow<T>> where T : struct {
        private readonly ViewPredicate _predicate;

        internal readonly ComponentTable<T> Table;

        internal View(ComponentTable<T> table, ViewPredicate predicate) => (_predicate, Table) = (predicate, table);

        /// <summary>
        /// Checks if the View contains a given entity.
        /// </summary>
        /// <param name="entity">Entity to check.</param>
        /// <returns>True if the entity is contained in the view.</returns>
        public bool Contains(Entity entity) => Table.Contains(entity) && _predicate.IsAllowed(entity);

        /// <summary>
        /// Enumerator for matched entities and components.
        /// 
        /// Note that components are returned _by value_ and hence
        /// should not be modified.
        /// </summary>
        /// <returns>Entity and component enumerator.</returns>
        public ViewEnumerator<T> GetEnumerator() => new ViewEnumerator<T>(this, Table);
        IEnumerator<ViewRow<T>> IEnumerable<ViewRow<T>>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }

    public sealed class View<T1, T2> : IView, IEnumerable<ViewRow<T1, T2>>
        where T1 : struct
        where T2 : struct {
        private readonly ViewPredicate _predicate;

        internal readonly ComponentTable<T1> Table1;
        internal readonly ComponentTable<T2> Table2;

        internal View(ComponentTable<T1> table1, ComponentTable<T2> table2, ViewPredicate predicate) => (_predicate, Table1, Table2) = (predicate, table1, table2);

        /// <summary>
        /// Checks if the View contains a given entity.
        /// </summary>
        /// <param name="entity">Entity to check.</param>
        /// <returns>True if the entity is contained in the view.</returns>
        public bool Contains(Entity entity) => Table1.Contains(entity) && Table2.Contains(entity) && _predicate.IsAllowed(entity);

        /// <summary>
        /// Enumerator for matched entities and components.
        /// 
        /// Note that components are returned _by value_ and hence
        /// should not be modified.
        /// </summary>
        /// <returns>Entity and component enumerator.</returns>
        public ViewEnumerator<T1, T2> GetEnumerator() => new ViewEnumerator<T1, T2>(this, Table1);
        IEnumerator<ViewRow<T1, T2>> IEnumerable<ViewRow<T1, T2>>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }

    public sealed class View<T1, T2, T3> : IView, IEnumerable<ViewRow<T1, T2, T3>>
        where T1 : struct
        where T2 : struct
        where T3 : struct {
        private readonly ViewPredicate _predicate;

        internal readonly ComponentTable<T1> Table1;
        internal readonly ComponentTable<T2> Table2;
        internal readonly ComponentTable<T3> Table3;

        internal View(ComponentTable<T1> table1, ComponentTable<T2> table2, ComponentTable<T3> table3, ViewPredicate predicate) => (_predicate, Table1, Table2, Table3) = (predicate, table1, table2, table3);

        /// <summary>
        /// Checks if the View contains a given entity.
        /// </summary>
        /// <param name="entity">Entity to check.</param>
        /// <returns>True if the entity is contained in the view.</returns>
        public bool Contains(Entity entity) => Table1.Contains(entity) && Table2.Contains(entity) && Table3.Contains(entity) && _predicate.IsAllowed(entity);

        /// <summary>
        /// Enumerator for matched entities and components.
        /// 
        /// Note that components are returned _by value_ and hence
        /// should not be modified.
        /// </summary>
        /// <returns>Entity and component enumerator.</returns>
        public ViewEnumerator<T1, T2, T3> GetEnumerator() => new ViewEnumerator<T1, T2, T3>(this, Table1);
        IEnumerator<ViewRow<T1, T2, T3>> IEnumerable<ViewRow<T1, T2, T3>>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }
}
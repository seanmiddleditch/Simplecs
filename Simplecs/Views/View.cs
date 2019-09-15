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
        private ComponentTable<T> _table;
        private ViewPredicate _predicate;

        internal View(ComponentTable<T> table, ViewPredicate predicate) => (_table, _predicate) = (table, predicate);

        internal ComponentMapper<T> Component => new ComponentMapper<T>(_table);

        /// <summary>
        /// Checks if the View contains a given entity.
        /// </summary>
        /// <param name="entity">Entity to check.</param>
        /// <returns>True if the entity is contained in the view.</returns>
        public bool Contains(Entity entity) => _table.Contains(entity) && _predicate.IsAllowed(entity);

        /// <summary>
        /// Enumerator for matched entities and components.
        /// 
        /// Note that components are returned _by value_ and hence
        /// should not be modified.
        /// </summary>
        /// <returns>Entity and component enumerator.</returns>
        public ViewEnumerator<T> GetEnumerator() => new ViewEnumerator<T>(this, _table);
        IEnumerator<ViewRow<T>> IEnumerable<ViewRow<T>>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }

    public sealed class View<T1, T2> : IView, IEnumerable<ViewRow<T1, T2>>
        where T1 : struct
        where T2 : struct {
        private ComponentTable<T1> _table1;
        private ComponentTable<T2> _table2;
        private ViewPredicate _predicate;

        internal View(ComponentTable<T1> table1, ComponentTable<T2> table2, ViewPredicate predicate) => (_table1, _table2, _predicate) = (table1, table2, predicate);

        internal ComponentMapper<T1> Component1 => new ComponentMapper<T1>(_table1);
        internal ComponentMapper<T2> Component2 => new ComponentMapper<T2>(_table2);

        /// <summary>
        /// Checks if the View contains a given entity.
        /// </summary>
        /// <param name="entity">Entity to check.</param>
        /// <returns>True if the entity is contained in the view.</returns>
        public bool Contains(Entity entity) => _table1.Contains(entity) && _table2.Contains(entity) && _predicate.IsAllowed(entity);

        /// <summary>
        /// Enumerator for matched entities and components.
        /// 
        /// Note that components are returned _by value_ and hence
        /// should not be modified.
        /// </summary>
        /// <returns>Entity and component enumerator.</returns>
        public ViewEnumerator<T1, T2> GetEnumerator() => new ViewEnumerator<T1, T2>(this, _table1);
        IEnumerator<ViewRow<T1, T2>> IEnumerable<ViewRow<T1, T2>>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }

    public sealed class View<T1, T2, T3> : IView, IEnumerable<ViewRow<T1, T2, T3>>
        where T1 : struct
        where T2 : struct
        where T3 : struct {
        private ComponentTable<T1> _table1;
        private ComponentTable<T2> _table2;
        private ComponentTable<T3> _table3;
        private ViewPredicate _predicate;

        internal View(ComponentTable<T1> table1, ComponentTable<T2> table2, ComponentTable<T3> table3, ViewPredicate predicate) => (_table1, _table2, _table3, _predicate) = (table1, table2, table3, predicate);

        internal ComponentMapper<T1> Component1 => new ComponentMapper<T1>(_table1);
        internal ComponentMapper<T2> Component2 => new ComponentMapper<T2>(_table2);
        internal ComponentMapper<T3> Component3 => new ComponentMapper<T3>(_table3);

        /// <summary>
        /// Checks if the View contains a given entity.
        /// </summary>
        /// <param name="entity">Entity to check.</param>
        /// <returns>True if the entity is contained in the view.</returns>
        public bool Contains(Entity entity) => _table1.Contains(entity) && _table2.Contains(entity) && _predicate.IsAllowed(entity);

        /// <summary>
        /// Enumerator for matched entities and components.
        /// 
        /// Note that components are returned _by value_ and hence
        /// should not be modified.
        /// </summary>
        /// <returns>Entity and component enumerator.</returns>
        public ViewEnumerator<T1, T2, T3> GetEnumerator() => new ViewEnumerator<T1, T2, T3>(this, _table1);
        IEnumerator<ViewRow<T1, T2, T3>> IEnumerable<ViewRow<T1, T2, T3>>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }
}
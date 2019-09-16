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
    internal static class ViewUtility {
        internal static IComponentTable SmallestTable(IComponentTable table, IComponentTable? extraTable) => extraTable != null && extraTable.Count < table.Count ? extraTable : table;
    }

    /// <summary>
    /// A collection of entities that match a particular signature.
    /// </summary>
    public interface IView<RowT> : IEnumerable<RowT> where RowT : struct {
        /// <summary>
        /// Retrieves the row for a given entity.
        /// </summary>
        /// <returns>True if the entity is contained by view.</returns>
        bool TryBindRow(Entity entity, out RowT row);

        /// <summary>
        /// Enumerator for matched entities and components.
        /// </summary>
        new ViewEnumerator<RowT> GetEnumerator();

        IEnumerator<RowT> IEnumerable<RowT>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }

    /// <summary>
    /// View over entities matching a specific signature.
    /// </summary>
    public sealed class View<T> : IView<ViewRow<T>> where T : struct {
        private readonly ViewPredicate _predicate;

        internal readonly ComponentTable<T> Table;

        internal View(ComponentTable<T> table, ViewPredicate predicate) => (_predicate, Table) = (predicate, table);

        bool IView<ViewRow<T>>.TryBindRow(Entity entity, out ViewRow<T> row) {
            row = default(ViewRow<T>);
            int index = Table.IndexOf(entity);
            if (index == -1) return false;
            row = new ViewRow<T>(this, entity, index);
            return _predicate.IsAllowed(entity);
        }

        ViewEnumerator<ViewRow<T>> IView<ViewRow<T>>.GetEnumerator() => new ViewEnumerator<ViewRow<T>>(this, ViewUtility.SmallestTable(Table, _predicate.SmallestTable()));
    }

    /// <summary>
    /// View over entities matching a specific signature.
    /// </summary>
    public sealed class View<T1, T2> : IView<ViewRow<T1, T2>>
        where T1 : struct
        where T2 : struct {
        private readonly ViewPredicate _predicate;

        internal readonly ComponentTable<T1> Table1;
        internal readonly ComponentTable<T2> Table2;

        internal View(ComponentTable<T1> table1, ComponentTable<T2> table2, ViewPredicate predicate) => (_predicate, Table1, Table2) = (predicate, table1, table2);

        bool IView<ViewRow<T1, T2>>.TryBindRow(Entity entity, out ViewRow<T1, T2> row) {
            row = default(ViewRow<T1, T2>);
            int index1 = Table1.IndexOf(entity);
            if (index1 == -1) return false;
            int index2 = Table2.IndexOf(entity);
            if (index2 == -1) return false;
            row = new ViewRow<T1, T2>(this, entity, index1, index2);
            return _predicate.IsAllowed(entity);
        }

        ViewEnumerator<ViewRow<T1, T2>> IView<ViewRow<T1, T2>>.GetEnumerator() => new ViewEnumerator<ViewRow<T1, T2>>(this, ViewUtility.SmallestTable(SmallestTable(), _predicate.SmallestTable()));
        private IComponentTable SmallestTable() {
            if (Table2.Count < Table1.Count) return Table2;
            return Table1;
        }
    }

    /// <summary>
    /// View over entities matching a specific signature.
    /// </summary>
    public sealed class View<T1, T2, T3> : IView<ViewRow<T1, T2, T3>>
        where T1 : struct
        where T2 : struct
        where T3 : struct {
        private readonly ViewPredicate _predicate;

        internal readonly ComponentTable<T1> Table1;
        internal readonly ComponentTable<T2> Table2;
        internal readonly ComponentTable<T3> Table3;

        internal View(ComponentTable<T1> table1, ComponentTable<T2> table2, ComponentTable<T3> table3, ViewPredicate predicate) => (_predicate, Table1, Table2, Table3) = (predicate, table1, table2, table3);

        bool IView<ViewRow<T1, T2, T3>>.TryBindRow(Entity entity, out ViewRow<T1, T2, T3> row) {
            row = default(ViewRow<T1, T2, T3>);
            int index1 = Table1.IndexOf(entity);
            if (index1 == -1) return false;
            int index2 = Table2.IndexOf(entity);
            if (index2 == -1) return false;
            int index3 = Table3.IndexOf(entity);
            if (index3 == -1) return false;
            row = new ViewRow<T1, T2, T3>(this, entity, index1, index2, index3);
            return _predicate.IsAllowed(entity);
        }

        ViewEnumerator<ViewRow<T1, T2, T3>> IView<ViewRow<T1, T2, T3>>.GetEnumerator() => new ViewEnumerator<ViewRow<T1, T2, T3>>(this, ViewUtility.SmallestTable(SmallestTable(), _predicate.SmallestTable()));
        private IComponentTable SmallestTable() {
            if (Table2.Count < Table1.Count && Table2.Count < Table3.Count) return Table2;
            if (Table3.Count < Table1.Count && Table3.Count < Table2.Count) return Table3;
            return Table1;
        }
    }
}
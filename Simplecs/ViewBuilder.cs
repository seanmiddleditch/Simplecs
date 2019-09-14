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
using System.Linq;

namespace Simplecs {
    /// <summary>
    /// Builder for a View object.
    /// 
    /// Options for required and excluded components can be set.
    /// 
    /// The Select method creates the View.
    /// </summary>
    public class ViewBuilder {
        private World _world;
        private List<IComponentTable>? _required;
        private List<IComponentTable>? _excluded;

        internal ViewBuilder(World world) => _world = world;

        /// <summary>
        /// Marks the specified component as required for the view,
        /// though it will not be in the selected component set.
        /// </summary>
        public ViewBuilder Require<T>() where T : struct {
            _required ??= new List<IComponentTable>();
            _required.Add(_world.GetTable<T>());
            return this;
        }

        /// <summary>
        /// Marks the specified component as excluded for the view,
        /// meaning that it cannot be present on matched entities.
        /// </summary>
        public ViewBuilder Exclude<T>() where T : struct {
            _excluded ??= new List<IComponentTable>();
            _excluded.Add(_world.GetTable<T>());
            return this;
        }

        /// <summary>
        /// Creates a View that selects the specified component.
        /// </summary>
        public View<Binding<T>.Binder, Binding<T>> Select<T>() where T : struct => new View<Binding<T>.Binder, Binding<T>>(new Binding<T>.Binder(Table<T>(), Predicate()));

        /// <summary>
        /// Creates a View that selects the specified components.
        /// </summary>
        public View<Binding<T1, T2>.Binder, Binding<T1, T2>> Select<T1, T2>() where T1 : struct where T2 : struct => new View<Binding<T1, T2>.Binder, Binding<T1, T2>>(new Binding<T1, T2>.Binder(Table<T1>(), Table<T2>(), Predicate()));

        /// <summary>
        /// Creates a View that selects the specified components.
        /// </summary>
        public View<Binding<T1, T2, T3>.Binder, Binding<T1, T2, T3>> Select<T1, T2, T3>() where T1 : struct where T2 : struct where T3 : struct => new View<Binding<T1, T2, T3>.Binder, Binding<T1, T2, T3>>(new Binding<T1, T2, T3>.Binder(Table<T1>(), Table<T2>(), Table<T3>(), Predicate()));

        private ViewPredicate Predicate() => new ViewPredicate(tables: _excluded != null ? (_required != null ? _excluded.Concat(_required).ToArray() : _excluded.ToArray()) : _required?.ToArray(), excludedCount: _excluded?.Count ?? 0);
        private ComponentTable<T> Table<T>() where T : struct => _world.GetTable<T>();
    }
}
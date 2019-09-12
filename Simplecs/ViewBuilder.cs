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
            if (_required == null) {
                _required = new List<IComponentTable>();
            }
            _required.Add(_world.GetTable<T>());
            return this;
        }

        /// <summary>
        /// Marks the specified component as excluded for the view,
        /// meaning that it cannot be present on matched entities.
        /// </summary>
        public ViewBuilder Exclude<T>() where T : struct {
            if (_excluded == null) {
                _excluded = new List<IComponentTable>();
            }
            _excluded.Add(_world.GetTable<T>());
            return this;
        }

        /// <summary>
        /// Creates a View that selects the specified component.
        /// </summary>
        public View<T> Select<T>() where T : struct {
            var view = new View<T>(table: _world.GetTable<T>(), required: _required, excluded: _excluded);
            _required = _excluded = null;
            return view;
        }

        /// <summary>
        /// Creates a View that selects the specified components.
        /// </summary>
        public View<T1, T2> Select<T1, T2>() where T1 : struct where T2 : struct {
            var view = new View<T1, T2>(table1: _world.GetTable<T1>(), table2: _world.GetTable<T2>(), required: _required, excluded: _excluded);
            _required = _excluded = null;
            return view;
        }

        /// <summary>
        /// Creates a View that selects the specified components.
        /// </summary>
        public View<T1, T2, T3> Select<T1, T2, T3>() where T1 : struct where T2 : struct where T3 : struct {
            var view = new View<T1, T2, T3>(table1: _world.GetTable<T1>(), table2: _world.GetTable<T2>(), table3: _world.GetTable<T3>(), required: _required, excluded: _excluded);
            _required = _excluded = null;
            return view;
        }
    }
}
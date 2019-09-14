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

    /// <summary>
    /// A collection of entities that match a particular signature.
    /// </summary>
    public sealed class View<Binder, Binding> : IView, IEnumerable<Binding> where Binder : IBinder<Binding> where Binding : struct {
        private Binder _binder;

        internal View(Binder binder) => _binder = binder;

        /// <summary>
        /// Checks if the View contains a given entity.
        /// </summary>
        /// <param name="entity">Entity to check.</param>
        /// <returns>True if the entity is contained in the view.</returns>
        public bool Contains(Entity entity) => _binder.Contains(entity);

        /// <summary>
        /// Attempts to retrieve the binding for a particular entity.
        /// </summary>
        /// <param name="entity">Entity to query.</param>
        /// <param name="binding">Component binding, if available.</param>
        /// <returns>True if the entity is contained in the view.</returns>
        public bool TryGet(Entity entity, out Binding binding) {
            if (Contains(entity)) {
                binding = _binder.Bind(entity);
                return true;
            }

            binding = default(Binding);
            return false;
        }

        /// <summary>
        /// Enumerator for matched entities and components.
        /// 
        /// Note that components are returned _by value_ and hence
        /// should not be modified.
        /// </summary>
        /// <returns>Entity and component enumerator.</returns>
        public ViewEnumerator<Binder, Binding> GetEnumerator() => new ViewEnumerator<Binder, Binding>(_binder);
        IEnumerator<Binding> IEnumerable<Binding>.GetEnumerator() => this.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => throw new NotImplementedException();
    }
}
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
    internal class ViewPredicate {
        private List<IComponentTable> _required = new List<IComponentTable>();
        private List<IComponentTable> _excluded = new List<IComponentTable>();

        internal void Require(IComponentTable table) => _required.Add(table);
        internal void Exclude(IComponentTable table) => _excluded.Add(table);

        internal bool IsAllowed(Entity entity) => !IsExcluded(entity) && HasRequired(entity);

        private bool IsExcluded(Entity entity) {
            foreach (IComponentTable table in _excluded) {
                if (table.Contains(entity)) {
                    return true;
                }
            }
            return false;
        }

        private bool HasRequired(Entity entity) {
            foreach (IComponentTable table in _required) {
                if (!table.Contains(entity)) {
                    return false;
                }
            }
            return true;
        }
    }
}
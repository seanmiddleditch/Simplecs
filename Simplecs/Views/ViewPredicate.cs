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

using Simplecs.Containers;

namespace Simplecs.Views {
    internal struct ViewPredicate {
        private IComponentTable[]? _tables;
        private int _excludedCount;

        internal ViewPredicate(IComponentTable[]? tables, int excludedCount) => (_tables, _excludedCount) = (tables, excludedCount);

        internal bool IsAllowed(Entity entity) {
            // If we have no tables at all, there's nothing to check.
            //
            if (_tables == null) {
                return true;
            }

            // If the entity exists in any excluded table, the entity is not allowed.
            //
            for (int index = 0; index != _excludedCount; ++index) {
                if (_tables[index].Contains(entity)) {
                    return false;
                }
            }

            // If the entity is missing from any required table, the entity is not allowed.
            //
            for (int index = _excludedCount; index != _tables.Length; ++index) {
                if (!_tables[index].Contains(entity)) {
                    return false;
                }
            }

            // The entity was not rejected by any test so it must be allowed.
            //
            return true;
        }

        internal IComponentTable? SmallestTable() {
            IComponentTable? bestTable = null;
            int bestCount = int.MaxValue;

            if (_tables == null) {
                return null;
            }

            // Find the smallest _required component_ table
            //
            for (int index = _excludedCount; index != _tables.Length; ++index) {
                if (_tables[index].Count < bestCount) {
                    bestTable = _tables[index];
                }
            }

            return bestTable;
        }
    }
}
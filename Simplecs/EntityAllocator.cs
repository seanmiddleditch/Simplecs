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
    /// Allocates and maintains a unique set of entity keys.
    /// </summary>
    internal class EntityAllocator {
        private uint _nextId = 1;
        private List<uint> _freeList = new List<uint>();

        public const uint invalid = 0;

        public uint Allocate() {
            if (_freeList.Count != 0) {
                uint id = _freeList[_freeList.Count - 1];
                _freeList.RemoveAt(_freeList.Count - 1);
                return id;
            }

            return _nextId++;
        }

        public void Deallocate(uint id) {
            _freeList.Add(id);
        }
    }
}
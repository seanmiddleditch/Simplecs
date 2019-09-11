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
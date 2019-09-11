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
    /// Allocates and maintains a unique set of entity keys.
    /// </summary>
    internal class EntityAllocator {
        private int _nextIndex = 1;
        private List<int> _freeList = new List<int>();
        private List<byte> _generations = new List<byte>();
        private int _freeHead = 0;
        private int _freeCount = 0;
        private uint _freeMinimum = 1000;

        public const uint invalid = 0;

        public EntityAllocator() { }

        public EntityAllocator(uint freeMinimum) {
            _freeMinimum = freeMinimum;
        }

        public Entity Allocate() {
            int index = AllocateIndex();

            // Determine the generation of the key at the given index;
            // for new indices, create a new generation.
            //
            if (index >= _generations.Count) {
                _generations.AddRange(Enumerable.Repeat((byte)0, (int)index - _generations.Count + 1));
            }
            byte generation = _generations[(int)index];

            return EntityUtil.MakeKey(index, generation);
        }

        public bool Deallocate(Entity entity) {
            (int index, byte generation) = EntityUtil.DecomposeKey(entity);
            if (index > 0 && index < _generations.Count && _generations[(int)index] == generation) {
                ++_generations[(int)index];
                AddToFreeList(index);
                return true;
            }
            return false;
        }

        public bool Validate(Entity entity) {
            (int index, byte generation) = EntityUtil.DecomposeKey(entity);
            return index > 0 && index < _generations.Count &&  _generations[(int)index] == generation;
        }

        private void AddToFreeList(int index) {
            // If the freelist is full, grow.
            //
            if (_freeCount == _freeList.Count) {
                _freeList.Add(index);
                ++_freeCount;
                return;
            }

            // Insert into the circular buffer.
            //
            int freeIndex = _freeHead + _freeCount;
            if (freeIndex > _freeList.Count) {
                freeIndex -= _freeList.Count;
            }

            _freeList[freeIndex] = index;
            ++_freeCount;
        }

        private int AllocateIndex() {
            // Only consume from the freelist if we have some items in it,
            // to avoid recycling the same id too often.
            //
            if (_freeCount <= _freeMinimum) {
                return _nextIndex++;
            }

            int index = _freeList[_freeHead];

            // Update the circular buffer after consuming the head item.
            //
            --_freeCount;
            if (++_freeHead >= _freeList.Count) {
                _freeHead = 0;
            }

            return index;
        }
    }
}
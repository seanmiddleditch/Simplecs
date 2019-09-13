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
        private List<byte> _generations = new List<byte>();
        private List<int> _freeIndices = new List<int>();
        private int _freeHead = 0;
        private int _freeCount = 0;
        private int _nextUnusedIndex = 0;
        private uint _freeMinimum = 1000;

        public const uint invalid = 0;

        public EntityAllocator() { }

        public EntityAllocator(uint freeMinimum) => _freeMinimum = freeMinimum;

        public Entity Allocate() {
            int index = AcquireIndex();

            // Determine the generation of the key at the given index;
            // for new indices, create a new generation.
            //
            if (index >= _generations.Count) {
                _generations.AddRange(Enumerable.Repeat((byte)1, index - _generations.Count + 1));
            }

            return EntityUtil.MakeKey(index, _generations[index]);
        }

        public bool Deallocate(Entity entity) {
            if (!IsValid(entity)) {
                return false;
            }

            (int index, byte generation) = EntityUtil.DecomposeKey(entity);

            // Bump generation so the Entity key remains stale for a long time even
            // when the index portion is recycled.
            //
            if (++_generations[index] == 0) {
                // Ensure generation can never be 0, so we can find
                // invalid keys easily.
                //
                _generations[index] = 1;
            }

            ReleaseIndex(index);
            
            return true;
        }

        public bool IsValid(Entity entity) {
            (int index, byte generation) = EntityUtil.DecomposeKey(entity);
            return index >= 0 && index < _generations.Count && _generations[index] == generation;
        }

        private int AcquireIndex() {
            // Only consume from the freelist if we have some items in it,
            // to avoid recycling the same id too often.
            //
            if (_freeCount <= _freeMinimum) {
                return _nextUnusedIndex++;
            }

            int index = _freeIndices[_freeHead];

            // Update the circular buffer after consuming the head item.
            //
            --_freeCount;
            if (++_freeHead >= _freeIndices.Count) {
                _freeHead = 0;
            }

            return index;
        }
        
        private void ReleaseIndex(int index) {
            // If the freelist is full, grow.
            //
            if (_freeCount == _freeIndices.Count) {
                _freeIndices.Add(index);
                ++_freeCount;
                return;
            }

            // Insert into the circular buffer.
            //
            int freeIndex = _freeHead + _freeCount;
            if (freeIndex > _freeIndices.Count) {
                freeIndex -= _freeIndices.Count;
            }

            _freeIndices[freeIndex] = index;
            ++_freeCount;
        }
    }
}
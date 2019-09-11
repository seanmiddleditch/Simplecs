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
using System.Runtime.CompilerServices;

namespace Simplecs {
    /// <summary>
    /// Allocates and maintains a unique set of entity keys.
    /// </summary>
    internal class EntityAllocator {
        private uint _nextIndex = 1;
        private List<uint> _freeList = new List<uint>();
        private List<byte> _generations = new List<byte>();
        private int _freeHead = 0;
        private int _freeCount = 0;
        private uint _freeMinimum = 1000;

        public const uint invalid = 0;

        public EntityAllocator() { }

        public EntityAllocator(uint freeMinimum) {
            _freeMinimum = freeMinimum;
        }

        public uint Allocate() {
            uint index = AllocateIndex();

            // Determine the generation of the key at the given index;
            // for new indices, create a new generation.
            //
            if (index >= _generations.Count) {
                _generations.AddRange(Enumerable.Repeat((byte)0, (int)index - _generations.Count + 1));
            }
            byte generation = _generations[(int)index];

            return MakeKey(index, generation);
        }

        public void Deallocate(uint key) {
            (uint index, byte generation) = DecomposeKey(key);
            if (index != 0 && index < _generations.Count && _generations[(int)index] == generation) {
                ++_generations[(int)index];
                AddToFreeList(index);
            }
        }

        public bool Validate(uint key) {
            (uint index, byte generation) = DecomposeKey(key);
            return index != 0 && key < _generations.Count &&  _generations[(int)index] == generation;
        }

        private void AddToFreeList(uint key) {
            // If the freelist is full, grow.
            //
            if (_freeCount == _freeList.Count) {
                _freeList.Add(key);
                ++_freeCount;
                return;
            }

            // Insert into the circular buffer.
            //
            int index = _freeHead + _freeCount;
            if (index > _freeList.Count) {
                index -= _freeList.Count;
            }

            _freeList[index] = key;
            ++_freeCount;
        }

        private uint AllocateIndex() {
            // Only consume from the freelist if we have some items in it,
            // to avoid recycling the same id too often.
            //
            if (_freeCount <= _freeMinimum) {
                return _nextIndex++;
            }

            uint index = _freeList[_freeHead];

            // Update the circular buffer after consuming the head item.
            //
            --_freeCount;
            if (++_freeHead >= _freeList.Count) {
                _freeHead = 0;
            }

            return index;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static uint MakeKey(uint index, byte generation) {
            return ((uint)generation << 24) | (index & 0x00FFFFFF);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static (uint index, byte generation) DecomposeKey(uint key) {
            uint index = key & 0x00FFFFFF;
            byte generation = (byte)(key >> 24);
            return (index, generation);
        }
    }
}
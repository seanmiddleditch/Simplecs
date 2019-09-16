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
using System.Runtime.InteropServices;

namespace Simplecs.Containers {
    /// <summary>
    /// Stores a collection of values in semi-contiguous but stable memory.
    /// 
    /// Backing stores are allocated in fixed-sized chunks. This storage can
    /// be indexed efficiently in O(1). Further, insertion is always append and
    /// also O(1), and removal always uses swap-and-pop and is also O(1).
    /// 
    /// Note that as consequence, ordering is _not_ maintained in this container.
    /// </summary>
    /// <typeparam name="T">Type stored.</typeparam>
    public class ChunkedStorage<T> where T : struct {
        private readonly List<T[]> _chunks = new List<T[]>();
        private readonly int _chunkElementCount = 1024;
        private int _count = 0;

        /// <summary>
        /// Default size of chunks in bytes.
        /// </summary>
        public const int defaultChunkSize = 16 * 1024;

        /// <summary>
        /// Number of elements stored.
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// Creates a new chunked storage container.
        /// </summary>
        /// <param name="chunkSize">Size of chunks in bytes.</param>
        public ChunkedStorage(int chunkSize = defaultChunkSize) {
            int elementSize = Marshal.SizeOf<T>();
            _chunkElementCount = chunkSize / elementSize;
        }

        /// <summary>
        /// Adds a value to container.
        /// </summary>
        /// <param name="value">Value to add.</param>
        public void Add(in T value) {
            var (chunk, index) = AllocateSlot();
            chunk[index] = value;
        }

        /// <summary>
        /// Removes an element at a given index.
        /// 
        /// This will swap in the last element in the container and
        /// hence mutates the order of elements.
        /// </summary>
        /// <param name="index">Index at which to remove an item.</param>
        public void RemoveAt(int index) {
            if (index < 0 || index >= _count) {
                return;
            }

            if (index < _count - 1) {
                int chunkIndex = index / _chunkElementCount;
                int slotIndex = index % _chunkElementCount;

                _chunks[chunkIndex][slotIndex] = _chunks[_chunks.Count - 1][(_count % _chunkElementCount) - 1];
            }

            --_count;
        }

        /// <summary>
        /// Removes all values from the container.
        /// </summary>
        public void Clear() {
            _count = 0;
        }

        /// <summary>
        /// Accesses the value at the given index.
        /// </summary>
        public ref T this[int index] => ref _chunks[index / _chunkElementCount][index % _chunkElementCount];

        private (T[] chunk, int index) AllocateSlot() {
            int chunkIndex = _count / _chunkElementCount;
            int slotIndex = _count % _chunkElementCount;

            if (_chunks.Count == chunkIndex) {
                AllocateChunk();
            }

            ++_count;
            return (_chunks[_chunks.Count - 1], slotIndex);
        }

        private T[] AllocateChunk() {
            T[] chunk = new T[_chunkElementCount];
            _chunks.Add(chunk);
            return chunk;
        }
    }
}
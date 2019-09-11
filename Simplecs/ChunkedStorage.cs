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

namespace Simplecs {
    public class ChunkedStorage<T> where T : struct {
        private List<T[]> _chunks = new List<T[]>();
        private List<int> _chunkCounts = new List<int>();
        private int _chunkElementCount = 1024;
        private int _count = 0;

        public const int defaultChunkSize = 16 * 1024;

        public int Count => _count;

        public ChunkedStorage(int chunkSize = defaultChunkSize) {
            int elementSize = Marshal.SizeOf<T>();
            _chunkElementCount = chunkSize / elementSize;
        }

        public void Add(in T value) {
            var (chunk, index) = AllocateSlot();
            chunk[index] = value;
        }

        public void RemoveAt(int index) {
            if (index < 0 || index >= _count) {
                return;
            }

            if (index < _count - 1) {
                int chunkIndex = index / _chunkElementCount;
                int slotIndex = index % _chunkElementCount;

                _chunks[chunkIndex][slotIndex] = _chunks[_chunks.Count - 1][_chunkCounts[_chunkCounts.Count - 1] - 1];
            }

            --_count;
            --_chunkCounts[_chunkCounts.Count - 1];
        }

        public void Clear() {
            _count = 0;
            for (int index = 0; index != _chunkCounts.Count; ++index) {
                _chunkCounts[index] = 0;
            }
        }

        public ref T this[int index] => ref _chunks[index / _chunkElementCount][index % _chunkElementCount];

        private (T[] chunk, int index) AllocateSlot() {
            if (_chunks.Count == 0 || _chunkCounts[_chunkCounts.Count - 1] == _chunkElementCount) {
                AllocateChunk();
            }

            ++_count;
            return (_chunks[_chunks.Count - 1], _chunkCounts[_chunkCounts.Count - 1]++);
        }

        private T[] AllocateChunk() {
            T[] chunk = new T[_chunkElementCount];
            _chunks.Add(chunk);
            _chunkCounts.Add(0);
            return chunk;
        }
    }
}
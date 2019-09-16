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
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Simplecs.Containers {
    /// <summary>
    /// Simple circule buffer implementation.
    /// </summary>
    public class CircularBuffer<T> {
        private readonly List<T> _data = new List<T>();
        private int _head = 0;
        private int _count = 0;

        /// <summary>
        /// Number of elements in the container.
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// Add a new element to the container at the end.
        /// </summary>
        public void Add(T value) {
            if (_count < _data.Count) {
                _data[OffsetOf(_head + _count)] = value;
            } else {
                _data.Add(value);
            }

            ++_count;
        }

        /// <summary>
        /// Pop an element from the front of the buffer and return it.
        /// </summary>
        public T PopFront() {
            if (_count == 0) {
                throw new InvalidOperationException(message: "CircularBuffer is empty");
            }

            T value = _data[_head];
            ++_head;
            --_count;
            return value;
        }

        /// <summary>
        /// Pop an element from the back of the buffer and return it.
        /// </summary>
        public T PopBack() {
            if (_count == 0) {
                throw new InvalidOperationException(message: "CircularBuffer is empty");
            }

            T value = _data[OffsetOf(_head + _count - 1)];
            --_count;
            return value;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private int OffsetOf(int index) => index % _data.Count;
    }
}
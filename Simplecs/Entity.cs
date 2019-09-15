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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Simplecs {
    /// <summary>
    /// Entity key holder.
    /// </summary>
    public struct Entity : IEquatable<Entity> {
        internal uint key;

        /// <summary>
        /// Invalid Entity constant.
        /// </summary>
        public static Entity Invalid = new Entity();

        /// <summary>
        /// Tests equality.
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Entity lhs, Entity rhs) => lhs.key == rhs.key;

        /// <summary>
        /// Tests inequality.
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Entity lhs, Entity rhs) => lhs.key != rhs.key;

        /// <summary>
        /// Comparison for sorted containers.
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator <(Entity lhs, Entity rhs) => lhs.key < rhs.key;

        /// <summary>
        /// Comparison for sorted containers.
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static bool operator >(Entity lhs, Entity rhs) => lhs.key > rhs.key;

        /// <summary>
        /// Tests equality.
        /// </summary>
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Entity rhs) => key == rhs.key;

        /// <summary>
        /// Tests equality.
        /// </summary>
        public override bool Equals(object? rhs) => rhs is Entity rhsEntity && key == rhsEntity.key;

        /// <summary>
        /// Hash code of the entity.
        /// </summary>
        public override int GetHashCode() {
            return key.GetHashCode();
        }
    }

    internal static class EntityUtil {
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Entity MakeKey(int index, byte generation = 1) {
            return new Entity { key = ((uint)generation << 24) | ((uint)index & 0x00FFFFFF) };
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static (int index, byte generation) DecomposeKey(Entity entity) {
            int index = DecomposeIndex(entity);
            byte generation = (byte)(entity.key >> 24);
            return (index, generation);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int DecomposeIndex(Entity entity) {
            return (int)(entity.key & 0x00FFFFFF);
        }
    }
}
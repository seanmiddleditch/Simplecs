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
        /// Compares to entities for quality.
        /// </summary>
        /// <param name="other">Entity to compare.</param>
        /// <returns>True if both entities represent the same key.</returns>
        public bool Equals(Entity other) {
            return key == other.key;
        }

        /// <summary>
        /// Hash code of the entity.
        /// </summary>
        /// <returns>Hash of the entity.</returns>
        override public int GetHashCode() {
            return key.GetHashCode();
        }
    }

    internal static class EntityUtil {
        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static Entity MakeKey(int index, byte generation = 1) {
            return new Entity{key=((uint)generation << 24) | ((uint)index & 0x00FFFFFF)};
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
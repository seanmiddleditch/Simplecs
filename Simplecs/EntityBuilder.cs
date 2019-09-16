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

namespace Simplecs {
    /// <summary>
    /// Assists in creating new entities.
    /// </summary>
    public readonly struct EntityBuilder {
        private readonly World _world;
        private readonly Entity _entity;

        /// <summary>
        /// Retrieves the created entity.
        /// </summary>
        public Entity Entity => _entity;

        internal EntityBuilder(World world, Entity entity) {
            _world = world;
            _entity = entity;
        }

        /// <summary>
        /// Attaches a component to the newly created entity.
        /// </summary>
        public EntityBuilder Attach<T>(in T component) where T : struct {
            _world.Attach(_entity, component);
            return this;
        }
    }
}
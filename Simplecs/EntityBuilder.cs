namespace Simplecs {
    /// <summary>
    /// Assists in creating new entities.
    /// </summary>
    public struct EntityBuilder {
        World _world;
        Entity _entity;

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
        public EntityBuilder Attach<T>(T component) where T : struct {
            _world.Attach(_entity, component);
            return this;
        }
    }
}
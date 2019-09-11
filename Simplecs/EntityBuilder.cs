namespace Simplecs {
    public struct EntityBuilder {
        World _world;
        Entity _entity;

        public Entity Entity => _entity;

        public EntityBuilder(World world, Entity entity) {
            _world = world;
            _entity = entity;
        }

        public EntityBuilder Attach<T>(T component) where T : struct {
            _world.Attach(_entity, component);
            return this;
        }
    }
}
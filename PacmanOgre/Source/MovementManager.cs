using org.ogre;
using PacmanOgre.Components;
using SharpEngine;

namespace PacmanOgre
{
    using static PacmanOgre.Utilities.EnumerableExtensions;

    class MovementManager : IMovementManager
    {
        private readonly EntityManager _entityManager;

        public MovementManager(EntityManager entityManager)
        {
            _entityManager = entityManager;
        }

        public void Update(float timeDelta)
        {
            _entityManager.GetEntitiesWithComponents<PositionComponent, VelocityComponent>().ForEach(
                entity =>
                {
                    VelocityComponent velocityComponent = entity.GetComponent<VelocityComponent>();
                    PositionComponent positionComponent = entity.GetComponent<PositionComponent>();
                    Vector3 velocity = velocityComponent.Velocity;
                    Vector3 delta = velocity.Multiply(timeDelta);

                    positionComponent.Position = positionComponent.Position.Add(delta);
                });
        }
    }
}

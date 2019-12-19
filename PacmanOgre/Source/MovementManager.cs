using org.ogre;
using PacmanOgre.Components;
using SharpEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanOgre
{
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

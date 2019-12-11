using SharpEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacmanOgre.Components
{
    class MovementComponent : IComponent
    {
        private IEntity _entity;

        public MovementComponent(IContext context, IEntity entity)
        {
            _entity = entity;
            context.GetITickManager().OnFrameStart += OnFrameStart;
        }

        public void OnEntityCreated()
        {
            if (!_entity.HasComponent<TransformComponent>())
            {
                throw new Exception("Requires TransformComponent");
            }
        }

        public void OnTransformUpdated(object source, TransformChangedEventArgs eventArgs)
        {
            if (_entity.IsValid())
            {
                RenderableComponent renderableComponent = _entity.GetComponent<RenderableComponent>();
                renderableComponent.SceneNode.setPosition(eventArgs.Transform);
            }
        }

        public void OnFrameStart(object source, TickEventArgs eventArgs)
        {

        }
    }
}

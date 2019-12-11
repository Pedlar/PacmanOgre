using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using org.ogre;
using SharpEngine;

namespace PacmanOgre.Components
{
    public class RenderableComponent : IComponent
    {
        private readonly IContext _context;
        private readonly IEntity _entity;

        public SceneNode SceneNode { get; private set; }

        public RenderableComponent(IContext context, IEntity entity, SceneNode sceneNode)
        {
            _context = context;
            _entity = entity;
            SceneNode = sceneNode;
        }

        public void OnEntityCreated()
        {
            if (_entity.HasComponent<TransformComponent>())
            {
                var transformComponent = _entity.GetComponent<TransformComponent>();
                SceneNode.setPosition(transformComponent.Transform);
            }


        }
    }
}

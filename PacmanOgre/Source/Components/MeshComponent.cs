using System;

using SharpEngine;
using org.ogre;
using OgreEntity = org.ogre.Entity;

namespace PacmanOgre.Components
{
    public class MeshComponent : IComponent
    {
        private readonly IContext context;
        private readonly IRenderer renderer;
        private readonly string _meshName;
        private readonly IEntity entity;
        public OgreEntity OgreEntity { get; private set; }

        public MeshComponent(IContext context, IEntity entity, string meshName)
        {
            this.context = context;
            _meshName = meshName;
            this.entity = entity;
        }

        public void OnEntityCreated()
        {
            if (!entity.HasComponent<TransformComponent>())
            {
                throw new Exception("Requires TransformComponent");
            }

            var scnMgr = renderer.GetSceneManager();
            OgreEntity = scnMgr.createEntity(_meshName);

            RenderableComponent renderableComponent = entity.GetComponent<RenderableComponent>();
            var sceneNode = renderableComponent.SceneNode;

            sceneNode.attachObject(OgreEntity);
        }

        public void OnFrameStart(object source, TickEventArgs eventArgs)
        {

        }
    }
}

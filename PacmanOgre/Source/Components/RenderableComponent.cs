using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using org.ogre;
using OgreEntity = org.ogre.Entity;
using SharpEngine;

namespace PacmanOgre.Components
{
    public class RenderableComponent : IComponent
    {
        private readonly IContext _context;
        private readonly IEntity _entity;

        public SceneNode SceneNode { get; set; }
        public OgreEntity OgreEntity { get; set; }

        public RenderableComponent(IContext context, IEntity entity)
        {
            _context = context;
            _entity = entity;

            _entity.AddActivationListener(OnActivated);
            _entity.AddDestroyListener(OnDestroy);
        }

        public void Setup()
        {
            if (!_entity.HasComponent<PositionComponent>())
            {
                throw new Exception("Requires PositionComponent");
            }
            PositionComponent positionComponent = _entity.GetComponent<PositionComponent>();
            positionComponent.OnPositionChanged += OnPositionChanged;
        }

        public void OnLoaded()
        {

        }

        #region events
        public void OnPositionChanged(object source, PositionChangedEventArgs eventArgs)
        {
            if (_entity.IsValid())
            {
                SceneNode.setPosition(eventArgs.Position);
            }
        }

        public void OnActivated(object source, EntityEventArgs eventArgs)
        {
            SceneNode.attachObject(OgreEntity);
        }

        public void OnDestroy(object source, EntityEventArgs eventArgs)
        {
            SceneNode.detachObject(OgreEntity);
            OgreEntity.Dispose();
        }
        #endregion
    }
}

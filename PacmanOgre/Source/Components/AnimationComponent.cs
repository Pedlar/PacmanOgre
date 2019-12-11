using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpEngine;
using org.ogre;

namespace PacmanOgre.Components
{
    class AnimationComponent : IComponent
    {
        public string AnimationState { get; set; }

        private readonly IRenderer renderer;
        private readonly IEntity entity;

        private AnimationState ogreAnimationState;

        public AnimationComponent(IContext context, IEntity entity, string animationState)
        {
            this.entity = entity;
            renderer = context.GetRenderer();
            AnimationState = animationState;
            context.GetITickManager().OnFrameStart += OnFrameStart;
        }

        public void OnEntityCreated()
        {
            if (!entity.HasComponent<MeshComponent>())
            {
                throw new Exception("Requires MeshComponenet");
            }

            MeshComponent meshComponent = entity.GetComponent<MeshComponent>();
            var ogreEntity = meshComponent.OgreEntity;
            ogreAnimationState = ogreEntity.getAnimationState(AnimationState);
            ogreAnimationState.setEnabled(true);
            ogreAnimationState.setLoop(true);
        }

        private void OnFrameStart(object sender, TickEventArgs e)
        {
            ogreAnimationState.addTime(e.TimeDelta);
        }
    }
}

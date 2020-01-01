using SharpEngine;
using org.ogre;

namespace PacmanOgre.Components
{
    class AnimationComponent : IComponent
    {
        public string AnimationState { get; set; }

        private readonly IEntity entity;

        private AnimationState ogreAnimationState;

        public AnimationComponent(IContext context, IEntity entity, string animationState)
        {
            this.entity = entity;
            AnimationState = animationState;
            context.GetITickManager().OnFrameStart += OnFrameStart;
        }

        public void Setup()
        {
            /*if (!entity.HasComponent<MeshComponent>())
            {
                throw new Exception("Requires MeshComponenet");
            }

            MeshComponent meshComponent = entity.GetComponent<MeshComponent>();
            var ogreEntity = meshComponent.OgreEntity;
            ogreAnimationState = ogreEntity.getAnimationState(AnimationState);
            ogreAnimationState.setEnabled(true);
            ogreAnimationState.setLoop(true);
            */
        }

        public void OnLoaded()
        {

        }

        private void OnFrameStart(object sender, TickEventArgs e)
        {
            ogreAnimationState.addTime(e.TimeDelta);
        }
    }
}

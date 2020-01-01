using Entity.Component;
using org.ogre;
using PacmanOgre.Utilities;
using SharpEngine;

namespace PacmanOgre.Components
{
    class VelocityComponent : IComponent
    {
        [Description("Velocity")]
        public Vector3 Velocity { get; set; } = VectorUtils.Vector3.ZERO;

        public VelocityComponent(IContext context, IEntity entity)
        {

        }

        public void OnLoaded()
        {

        }

        public void Setup()
        {

        }
    }
}

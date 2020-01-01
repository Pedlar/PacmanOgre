using Entity.Component;
using org.ogre;
using SharpEngine;

namespace PacmanOgre.Components
{
    public class ScaleComponent : IComponent
    {
        [Description("Scale")]
        public Vector3 Scale { get; set; }

        public ScaleComponent(IContext context, IEntity entiy)
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

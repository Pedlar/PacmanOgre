using org.ogre;
using PacmanOgre.Utilities;
using SharpEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PacmanOgre.Components
{
    class VelocityComponent : IComponent
    {
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

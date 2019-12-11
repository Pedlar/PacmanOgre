using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpEngine;
using org.ogre;

namespace PacmanOgre.Components
{
    public class TransformChangedEventArgs : EventArgs
    {
        public Vector3 Transform { get; set; }
    }

    public class TransformComponent : IComponent
    {
        public event EventHandler<TransformChangedEventArgs> OnTransformChanged;

        public Vector3 Transform { get; set; }
        public TransformComponent(IContext context, IEntity entity, Vector3 vector3)
        {
            Transform = vector3;
            OnTransformChangedEvent(vector3);
        }

        protected virtual void OnTransformChangedEvent(Vector3 vector3)
        {
            OnTransformChanged?.Invoke(this, new TransformChangedEventArgs { Transform = vector3 });
        }

        public void OnEntityCreated()
        {

        }
    }
}

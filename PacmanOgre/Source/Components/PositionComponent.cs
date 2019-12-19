using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpEngine;
using org.ogre;
using PacmanOgre.Utilities;

namespace PacmanOgre.Components
{
    public class PositionChangedEventArgs : EventArgs
    {
        public Vector3 Position { get; set; }
    }

    public class PositionComponent : IComponent
    {
        public event EventHandler<PositionChangedEventArgs> OnPositionChanged;

        private Vector3 _position;
        public Vector3 Position
        {
            get
            {
                return _position;
            }
            set
            {
                if(_position == null || _position.isNaN())
                {
                    _position = VectorUtils.Vector3.ZERO;
                }
                _position = value;
                OnPositionChangedEvent(_position);
            }
        }

        public PositionComponent(IContext context, IEntity entity, Vector3 vector3)
        {
            Position = vector3;
        }

        protected virtual void OnPositionChangedEvent(Vector3 vector3)
        {
            OnPositionChanged?.Invoke(this, new PositionChangedEventArgs { Position = vector3 });
        }

        public void Setup()
        {

        }

        public void OnLoaded()
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpEngine;
using org.ogre;
using OgreEntity = org.ogre.Entity;

namespace PacmanOgre.Components
{
    class CameraComponent : IComponent
    {
        private readonly IContext _context;
        private readonly IEntity _entity;

        public bool ActiveCamera { get; set; }

        private Camera _camera;
        public Camera Camera { get { return _camera; } set { setupCamera(value); } }
        public SceneNode SceneNode { get; set; }

        public CameraComponent(IContext context, IEntity entity)
        {
            _context = context;
            _entity = entity;
        }

        public void Setup()
        {

        }

        public void OnLoaded()
        {
            
        }

        private void setupCamera(Camera camera)
        {
            PositionComponent transformComponent = _entity.GetComponent<PositionComponent>();
            float z = transformComponent.Position.z;

            Matrix4 p = this.BuildScaledOrthoMatrix(z / -2.0f,
                                                    z / 2.0f,
                                                    z / -2.0f,
                                                    z / 2.0f, 0, 1000);

            camera.setAutoAspectRatio(true);
            camera.setNearClipDistance(5);
            camera.setProjectionType(ProjectionType.PT_ORTHOGRAPHIC);
            camera.setCustomProjectionMatrix(true, p);

            _camera = camera;
        }

        public Matrix4 BuildScaledOrthoMatrix(float left, float right, float bottom, float top, float near, float far)
        {
            float invw = 1 / (right - left);
            float invh = 1 / (top - bottom);
            float invd = 1 / (far - near);

            var zerozero = 2 * invw;
            var zerothree = -(right + left) * invw;
            var oneone = 2 * invh;
            var onethree = -(top + bottom) * invh;
            var twotwo = -2 * invd;
            var twothree = -(far + near) * invd;
            var threethree = 1;

            return new Matrix4(zerozero, 0, 0, zerothree,
                               0, oneone, 0, onethree,
                               0, 0, twotwo, twothree,
                               0, 0, 0, threethree);
        }
    }
}

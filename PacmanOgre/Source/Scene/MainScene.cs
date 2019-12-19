using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PacmanOgre.Components;
using SharpEngine;
using org.ogre;

using OgreSceneManager = org.ogre.SceneManager;
using OgreEntity = org.ogre.Entity;
using SharpEngine.Component;

namespace PacmanOgre.Scene
{
    class MainScene : IScene
    {
        public static readonly SceneId MainSceneId = new SceneId { SceneName = "MainScene" };
        private IContext _context;
        private EntityManager _entityManager;
        private OgreSceneManager _sceneManager;
        private IMovementManager _movementManager;

        public MainScene(IContext context, EntityManager entityManager, IMovementManager movementManager)
        {
            _context = context;
            _entityManager = entityManager;
            _sceneManager = _context.GetRenderer().GetSceneManager();
            _movementManager = movementManager;
        }

        public SceneId SceneId { get; set; } = MainSceneId;

        public void Setup()
        {
            
            var entity = _entityManager.CreateEntity(configure: e =>
            {
                e.AddComponent<PositionComponent>(new ComponentProperties()
                {
                    ["Position"] = new Vector3(10f, 10f, 0f)
                }, _context, e);
                e.AddComponent<VelocityComponent>(_context, e);
                e.AddComponent<MeshComponent>(new ComponentProperties()
                {
                    ["MeshName"] = "Sinbad.mesh"
                }, _context, e);
                e.AddComponent<PlayerInputComponent>(_context, e);
                e.AddComponent<RenderableComponent>(_context, e);
            });

            var cameraEntity = _entityManager.CreateEntity(configure: e =>
            {
                e.AddComponent<PositionComponent>(new ComponentProperties()
                {
                    ["Position"] = new Vector3(0f, 0f, 50f)
                }, _context, e);

                e.AddComponent<CameraComponent>(new ComponentProperties()
                {
                    ["ActiveCamera"] = true
                }, _context, e);
            });

        }

        public void LoadScene()
        {
            List<IEntity> renderableEntities =_entityManager.GetEntitiesWithComponents<RenderableComponent>();
            renderableEntities.ForEach(entity =>
            {
                MeshComponent meshComponent = entity.GetComponent<MeshComponent>();
                OgreEntity ogreEntity = _sceneManager.createEntity(meshComponent.MeshName);

                SceneNode sceneNode = _sceneManager.getRootSceneNode().createChildSceneNode(/* TODO: Name Component? */);

                PositionComponent transformComponent = entity.GetComponent<PositionComponent>();
                sceneNode.setPosition(transformComponent.Position);

                RenderableComponent renderableComponent = entity.GetComponent<RenderableComponent>();
                renderableComponent.OgreEntity = ogreEntity;
                renderableComponent.SceneNode = sceneNode;

                entity.IsActive = true;
            });

            List<IEntity> cameraEntities = _entityManager.GetEntitiesWithComponents<CameraComponent>();
            cameraEntities.ForEach(entity =>
            {
                CameraComponent cameraComponent = entity.GetComponent<CameraComponent>();

                Camera camera = _sceneManager.createCamera("Camera"/*TODO Name From Component*/);
                cameraComponent.Camera = camera;

                PositionComponent transformComponent = entity.GetComponent<PositionComponent>();

                SceneNode camnode = _sceneManager.getRootSceneNode().createChildSceneNode();
                camnode.translate(transformComponent.Position, Node.TransformSpace.TS_LOCAL);
                camnode.yaw(new Radian(0f));
                camnode.pitch(new Radian(0f));

                cameraComponent.SceneNode = camnode;
                camnode.attachObject(camera);
            });

            _entityManager.GetEntities().ForEach(entity =>
            {
                entity.GetComponents().ForEach(component =>
                {
                    component.OnLoaded();
                });
            });

            // Set the current Active Camera
            _entityManager.GetEntitiesWithComponents<CameraComponent>().ForEach(entity =>
            {
                CameraComponent cameraComponent = entity.GetComponent<CameraComponent>();
                if(cameraComponent.ActiveCamera)
                {
                    var vp = _context.GetRenderWindow().addViewport(cameraComponent.Camera);
                    vp.setBackgroundColour(new ColourValue(.1f, .3f, .3f));
                }
            });
        }

        public void UnloadScene()
        {
            _sceneManager.clearScene();
        }

        public void Update(float timeDelta, bool sceneActive)
        {
            // Don't update if we're not active, this is only so background stuff can be done if someone is in a non-safe UI
            if(!sceneActive)
            {
                return;
            }

            _entityManager.Update();
            _movementManager.Update(timeDelta);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _entityManager.Clear();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}

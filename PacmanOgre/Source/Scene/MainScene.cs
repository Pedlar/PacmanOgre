using System.Collections.Generic;

using PacmanOgre.Components;
using SharpEngine;
using org.ogre;

using OgreSceneManager = org.ogre.SceneManager;
using OgreEntity = org.ogre.Entity;
using SharpEngine.Component;

namespace PacmanOgre.Scene
{
    using PacmanOgre.Scene.Loader;
    using PacmanOgre.Utilities;
    using static PacmanOgre.Utilities.EnumerableExtensions;

    class MainScene : IScene
    {
        public static readonly SceneId MainSceneId = new SceneId { SceneName = "MainScene" };
        private IContext _context;
        private EntityManager _entityManager;
        private OgreSceneManager _sceneManager;
        private IMovementManager _movementManager;

        private SceneLoader _sceneLoader;

        public MainScene(IContext context, EntityManager entityManager, IMovementManager movementManager)
        {
            _context = context;
            _entityManager = entityManager;
            _movementManager = movementManager;

            _sceneManager = _context.GetRoot().createSceneManager();

            var shadergen = ShaderGenerator.getSingleton();
            shadergen.addSceneManager(_sceneManager); // must be done before we do anything with the scene

            _sceneManager.setAmbientLight(new ColourValue(.9f, .9f, .9f));

            _sceneLoader = new SceneLoader(context, entityManager) { FilePath = "Media/scenes/MainScene.xml" };
        }

        public SceneId SceneId { get; set; } = MainSceneId;

        public EntityManager GetEntityManager()
        {
            return _entityManager;
        }

        public OgreSceneManager GetOgreSceneManager()
        {
            return _sceneManager;
        }

        public void Setup()
        {
            _sceneLoader.LoadScene(MainSceneId.SceneName);
        }

        public void LoadScene()
        {
            IEnumerable<IEntity> renderableEntities = _entityManager.GetEntitiesWithComponents<RenderableComponent>();
            renderableEntities.ForEach(SetupRenderableEntity);

            IEnumerable<IEntity> cameraEntities = _entityManager.GetEntitiesWithComponents<CameraComponent>();
            cameraEntities.ForEach(SetupCamera);

            _entityManager.GetEntities().ForEach(entity =>
            {
                entity.GetComponents().ForEach(component =>
                {
                    component.OnLoaded();
                });
            });

            if (!_context.IsEditor)
            {
                // Set the current Active Camera
                _entityManager.GetEntitiesWithComponents<CameraComponent>().ForEach(entity =>
                {
                    CameraComponent cameraComponent = entity.GetComponent<CameraComponent>();
                    if (cameraComponent.ActiveCamera)
                    {
                        var vp = _context.GetRenderWindow().addViewport(cameraComponent.Camera);
                        vp.setBackgroundColour(new ColourValue(.1f, .3f, .3f));
                    }
                });
            }

            MeshPtr meshPtr = MeshManager.getSingleton().createPlane("background", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, new Plane(VectorUtils.Vector3.UNIT_Y, 0f), 800, 600, 4, 4, true, 1, 4, 4, VectorUtils.Vector3.UNIT_Z);
            var floorEnt = _sceneManager.createEntity("background", "background");
            floorEnt.setMaterial(MaterialManager.getSingleton().getByName("Color_009"));

            var floorNode = _sceneManager.getRootSceneNode().createChildSceneNode();
            floorNode.setPosition(new Vector3(0f, -3f, 0f));
            floorNode.attachObject(floorEnt);

            meshPtr.Dispose();
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

        private void SetupRenderableEntity(IEntity entity)
        {
            MeshComponent meshComponent = entity.GetComponent<MeshComponent>();
            OgreEntity ogreEntity = _sceneManager.createEntity(meshComponent.MeshName);

            SceneNode sceneNode = _sceneManager.getRootSceneNode().createChildSceneNode(/* TODO: Name Component? */);

            PositionComponent transformComponent = entity.GetComponent<PositionComponent>();
            sceneNode.setPosition(transformComponent.Position);

            if(entity.HasComponent<ScaleComponent>())
            {
                ScaleComponent scaleComponent = entity.GetComponent<ScaleComponent>();
                sceneNode.setScale(scaleComponent.Scale);
            }

            RenderableComponent renderableComponent = entity.GetComponent<RenderableComponent>();
            renderableComponent.OgreEntity = ogreEntity;
            renderableComponent.SceneNode = sceneNode;

            entity.IsActive = true;
        }

        private void SetupCamera(IEntity entity)
        {
            CameraComponent cameraComponent = entity.GetComponent<CameraComponent>();

            Camera camera = _sceneManager.createCamera("Camera"/*TODO Name From Component*/);
            cameraComponent.Camera = camera;

            PositionComponent transformComponent = entity.GetComponent<PositionComponent>();

            SceneNode camnode = _sceneManager.getRootSceneNode().createChildSceneNode();
            camnode.setPosition(transformComponent.Position);
            camnode.yaw(new Radian(0f));
            camnode.pitch(new Radian(-1.2f));

            cameraComponent.SceneNode = camnode;
            camnode.attachObject(camera);
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

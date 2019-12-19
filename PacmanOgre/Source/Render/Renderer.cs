using System;

using org.ogre;
using PacmanOgre.Utilities;

namespace PacmanOgre.Render
{
    internal class Renderer : IRenderer
    {
        public event EventHandler OnSetup;

        private ApplicationContext _appContext;
        private SceneManager _sceneManager;

        public Renderer(ApplicationContext applicationContext)
        {
            _appContext = applicationContext;
        }

        public void Dispose()
        {
            _appContext = null;
        }

        public SceneManager GetSceneManager() => _sceneManager;

        public void Setup()
        {
            var root = _appContext.getRoot();
            _sceneManager = root.createSceneManager();

            var shadergen = ShaderGenerator.getSingleton();
            shadergen.addSceneManager(_sceneManager); // must be done before we do anything with the scene

            _sceneManager.setAmbientLight(new ColourValue(.9f, .9f, .9f));

#if false
            var light = _sceneManager.createLight("MainLight");
            var lightnode = _sceneManager.getRootSceneNode().createChildSceneNode();
            lightnode.setPosition(0f, 10f, 15f);
            lightnode.attachObject(light); 
#endif

            MeshPtr meshPtr = MeshManager.getSingleton().createPlane("background", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, new Plane(VectorUtils.Vector3.UNIT_Z, 0f), 2000, 2000, 1, 1, true, 1, 64, 64, VectorUtils.Vector3.UNIT_Y);
            var floorEnt = _sceneManager.createEntity("background", "background");
            var floorNode = _sceneManager.getRootSceneNode().createChildSceneNode();
            floorNode.setPosition(new Vector3(0f, 0f, 0f));
            floorNode.attachObject(floorEnt);

            meshPtr.Dispose();
        }

        public void Update()
        {

        }

        protected virtual void OnSetupEvent()
        {
            OnSetup?.Invoke(this, EventArgs.Empty);
        }
    }
}
